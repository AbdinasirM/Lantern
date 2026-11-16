namespace Lantern.Server.Services;

using Microsoft.EntityFrameworkCore;
using Lantern.Database;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;
using Lantern.Database.Models;

public class PairingService
{
    private readonly byte[] _hmacKey;
    private readonly LanternDbContext _dbContext;

    public PairingService(IConfiguration configuration, LanternDbContext dbContext)
    {
        _dbContext = dbContext;

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
}