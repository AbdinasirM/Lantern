namespace Lantern.Shared;

public class RegisterUserResponseDTO
{
    public Guid UserId { get; set; }
    public Guid DeviceId { get; set; }
    public string DeviceToken { get; set; } = string.Empty;
}