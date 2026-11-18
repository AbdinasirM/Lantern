namespace Lantern.Server.Services;

using Microsoft.EntityFrameworkCore;
using Lantern.Database;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;
using Lantern.Database.Models;
using Lantern.Shared;

public class PairingService
{
    private readonly byte[] _hmacKey;
    
    private readonly LanternDbContext _dbContext;
    private readonly DeviceTokenService _deviceTokenService;

    public PairingService(IConfiguration configuration, LanternDbContext dbContext, DeviceTokenService deviceTokenService)
    {
        _dbContext = dbContext;
        _deviceTokenService = deviceTokenService;

        var secret = configuration["Pairing:HmacKey"]
            ?? throw new InvalidOperationException("Pairing HMAC key is missing.");

        _hmacKey = Convert.FromBase64String(secret);

        if (_hmacKey.Length != 32)
        {
            throw new InvalidOperationException("Pairing HMAC key must be 32 bytes.");
        }
    }



    public string GeneratePairingCode()
    {
        int number = RandomNumberGenerator.GetInt32(0, 1_000_000);
        return number.ToString("D6");
    }

    public byte[] HashPairingCode(string code)
    {
        if (code is null || code.Length != 6 ||
            code.Any(c => c < '0' || c > '9'))
        {
            throw new ArgumentException("Pairing code must contain exactly 6 digits.");
        }

        byte[] codeBytes = Encoding.UTF8.GetBytes(code);

        return HMACSHA256.HashData(_hmacKey, codeBytes);
    }

    public async Task<string> CreatePairingCodeAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.");
        }
        

        var user = await _dbContext.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        var code = GeneratePairingCode();
        var hashedCode = HashPairingCode(code);
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(5);

        var pairingCode = new PairingCodeEntity(user.Id, hashedCode, expiresAtUtc);
        
        _dbContext.PairingCodes.Add(pairingCode);
        await _dbContext.SaveChangesAsync();
        
        return code;

    }
    public async Task<PairingCodeEntity?> ValidateCode(string code)
    {

        if (code == null)
        {
            return null;
        }
        int isCodeSixDigit = code.Length;

        if (isCodeSixDigit != 6)
        {
            return null;
        }
        if(code.Any(c => c < '0' || c > '9'))
        {
            return null;
            
        }

        var hashedCode = HashPairingCode(code);
        var validCode = await _dbContext.PairingCodes.FirstOrDefaultAsync(c => c.CodeHash.SequenceEqual(hashedCode) && (c.IsUsed == false  &&  c.ExpiresAtUtc > DateTime.UtcNow) && c.FailedAttempts < 5);

        return validCode;
        
    }

    public async Task<RedeemPairingCodeResponseDTO?> RedeemPairingCodeAsync(string code, string deviceName)
    {
        if(code == null)
        {
            return null;
        }

        if(string.IsNullOrWhiteSpace(deviceName))
        {
            return null;
        }

        var validCode = await ValidateCode(code);

        if(validCode == null)
        {
            return null;
        }

        var deviceToken = _deviceTokenService.GenerateDeviceToken();

        var tokenHash = _deviceTokenService.HashDeviceToken(deviceToken);

        var device = new DeviceEntity(validCode.UserId, deviceName, tokenHash);

        
        var SetIsUsed =  validCode.TryMarkUsed();

        if(SetIsUsed == false)
        {
            return null;
        }
        _dbContext.Devices.Add(device);


        await _dbContext.SaveChangesAsync();
        
        return new RedeemPairingCodeResponseDTO
        {
            UserId = validCode.UserId,
            DeviceId = device.Id,
            DeviceToken = deviceToken
        };

    }
}