using Lantern.Database;
using Lantern.Database.Models;
using Lantern.Shared;
using Microsoft.EntityFrameworkCore;
namespace Lantern.Server.Services;


public class DeviceAuthenticationService
{
    private readonly LanternDbContext _dbContext;
    private readonly DeviceTokenService _deviceTokenService;

    public DeviceAuthenticationService(
        LanternDbContext dbContext,
        DeviceTokenService deviceTokenService)
    {
        _dbContext = dbContext;
        _deviceTokenService = deviceTokenService;
    }


    public async Task<DeviceEntity?> AuthenticateDeviceAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        byte[] tokenHash;

        try
        {
            tokenHash = _deviceTokenService.HashDeviceToken(token);
        }
        catch (FormatException)
        {
            return null;
        }
        catch (ArgumentException)
        {
            return null;
        }

        var device = await _dbContext.Devices.FirstOrDefaultAsync(device => device.TokenHash.SequenceEqual(tokenHash) && !device.IsRevoked);
        return device;



    }


}