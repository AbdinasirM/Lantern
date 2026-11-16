namespace Lantern.Database.Models;

public class PairingCodeEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public byte[] CodeHash { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    public bool IsUsed { get; private set; }
    public int FailedAttempts { get; private set; }
    private const int MaxFailedAttempts = 5;
    public PairingCodeEntity(Guid userId, byte[] codeHash, DateTime expiresAtUtc)
    {
        UserId = userId != Guid.Empty ? userId : throw new ArgumentException("User Id can not be empty");
        CodeHash = codeHash != null && codeHash.Length > 0 ? codeHash : throw new ArgumentException("codehash can not be null");
        ExpiresAtUtc = expiresAtUtc != DateTime.MinValue ? expiresAtUtc : throw new ArgumentException("time expiration needs to be set");
    }
    public bool TryMarkUsed()
    {
        if (IsUsed)
        {
            return false;
        }
        if (DateTime.UtcNow >= ExpiresAtUtc)
        {
            return false;
        }
        if (FailedAttempts >= MaxFailedAttempts)
        {
            return false;
        }
        IsUsed = true;
        return true;
    }
    public bool TryRecordFailedAttempt()
    {
        if (IsUsed)
        {
            return false;
        }
        if (DateTime.UtcNow >= ExpiresAtUtc)
        {
            return false;
        }
        if (FailedAttempts >= MaxFailedAttempts)
        {
            return false;
        }
        FailedAttempts++;
        return true;
    }
}