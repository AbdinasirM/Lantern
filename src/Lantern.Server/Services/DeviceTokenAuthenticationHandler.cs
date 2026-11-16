using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
namespace Lantern.Server.Services;

using System.Security.Claims;

public class DeviceTokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{

    private readonly DeviceAuthenticationService _deviceAuthenticationService;
    public DeviceTokenAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, DeviceAuthenticationService deviceAuthenticationService)
    : base(options, logger, encoder)
    {
        _deviceAuthenticationService = deviceAuthenticationService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorizationHeader = Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authorizationHeader))
        {
            return AuthenticateResult.NoResult();

        }
        if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.Fail("Invalid authorization scheme.");
        }

        var token = authorizationHeader["Bearer ".Length..].Trim();

        if (string.IsNullOrEmpty(token))
        {
            return AuthenticateResult.Fail("Device token is missing.");
        }

        var device = await _deviceAuthenticationService.AuthenticateDeviceAsync(token);


        if (device is null)
        {
            return AuthenticateResult.Fail("Invalid device token.");
        }

        var userid = device.UserId.ToString();
        var deviceId = device.Id.ToString();


        var Claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userid),
            new Claim("device_id", deviceId)
        };
        var identity = new ClaimsIdentity(Claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        
        return AuthenticateResult.Success(ticket);
    }
}