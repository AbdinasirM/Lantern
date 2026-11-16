using System.Security.Claims;
using Lantern.Server.Services;
using Lantern.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserRegistrationService _userRegistrationService;

    public UsersController(UserRegistrationService userRegistrationService)
    {
        _userRegistrationService = userRegistrationService;
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var deviceId = User.FindFirst("device_id")?.Value;
        return Ok(new { UserId = userId , deviceId});
    }



    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequestDTO request)
    {
        if (request is null)
        {
            return BadRequest("request can not be empty");
        }
        var user = await _userRegistrationService.RegisterUserAsync(request);
        return Ok(user);
    }




}