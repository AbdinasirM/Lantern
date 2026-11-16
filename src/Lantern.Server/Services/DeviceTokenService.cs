using System.Security.Cryptography;

namespace Lantern.Server.Services;

public class DeviceTokenService
{
    public string GenerateDeviceToken()
    {
        byte[] tokenBytes = RandomNumberGenerator.GetBytes(32);

        return Convert.ToBase64String(tokenBytes);
    }


    public byte[] HashDeviceToken(string token)
{
    byte[] tokenBytes = Convert.FromBase64String(token);

    if (tokenBytes.Length != 32)
    {
        throw new ArgumentException("Invalid device token.");
    }

    return SHA256.HashData(tokenBytes);
}
}