using System.ComponentModel.DataAnnotations;

namespace Lantern.Shared;

public class RedeemPairingCodeRequestDTO
{
    [Required]
    public string Code { get; set; } = string.Empty;
    [Required]
    public string DeviceName { get; set; } = string.Empty;
}