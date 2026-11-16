using Lantern.Database;
using Lantern.Database.Models;
using Lantern.Shared;
namespace Lantern.Server.Services;


public class UserRegistrationService
{
    private readonly LanternDbContext _dbContext;
    private readonly DeviceTokenService _deviceTokenService;

    public UserRegistrationService(
        LanternDbContext dbContext,
        DeviceTokenService deviceTokenService)
    {
        _dbContext = dbContext;
        _deviceTokenService = deviceTokenService;
    }


    public async Task<RegisterUserResponseDTO> RegisterUserAsync(RegisterUserRequestDTO request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.DeviceName))
        {
            throw new ArgumentException("Name and device name cannot be empty.");
        }
        var user = new UserEntity(request.Name);
        var deviceToken = _deviceTokenService.GenerateDeviceToken();
        var tokenHash = _deviceTokenService.HashDeviceToken(deviceToken);

        var device = new DeviceEntity(user.Id, request.DeviceName, tokenHash);

        _dbContext.Users.Add(user);
        _dbContext.Devices.Add(device);
        await _dbContext.SaveChangesAsync();
        return new RegisterUserResponseDTO
        {
            UserId = user.Id,
            DeviceId = device.Id,
            DeviceToken = deviceToken,
        };
    }


}