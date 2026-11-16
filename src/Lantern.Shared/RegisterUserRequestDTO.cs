using System.ComponentModel.DataAnnotations;

namespace Lantern.Shared;

public class RegisterUserRequestDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string DeviceName { get; set; } = string.Empty;
}