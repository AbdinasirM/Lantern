using System.Security.Claims;
using Lantern.Server.Services;
using Lantern.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PairingController : ControllerBase
{
    private readonly PairingService _pairingService;

    public PairingController(PairingService pairingService)
    {
        _pairingService = pairingService;
    }

    [HttpPost("code")]
    [Authorize]
    public async Task<IActionResult> PairingCode()
    {
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var iUserIdConverted = Guid.TryParse(userId, out Guid result);
        
       if(!iUserIdConverted)
        {
            return Unauthorized(); 
        }
        var newPairingCode = await _pairingService.CreatePairingCodeAsync(result);
        

        return Ok(new {code = newPairingCode});
    }


}