namespace Lantern.Database.Models;

public class DeviceEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public bool IsRevoked { get; private set; } = false;
    public byte[] TokenHash { get; private set; }
    public DeviceEntity(Guid userId, string name, byte[] tokenHash)
    {
        UserId = userId != Guid.Empty ? userId : throw new ArgumentException("UserID can not be empty");
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("Device name can not be empty");
        TokenHash = tokenHash is { Length: 32 }
            ? tokenHash
            : throw new ArgumentException("Token hash must contain exactly 32 bytes.");
    }
    public bool RevokeAccess()
    {
        if (IsRevoked)
        {
            return false;
        }
        IsRevoked = true;
        return true;
    }
    public bool SetDeviceName(string deviceName)
    {
        if (string.IsNullOrWhiteSpace(deviceName))
        {
            return false;
        }
        Name = deviceName;
        return true;
    }


}
