using DomusMind.Api.Auth;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Route("api/session")]
public sealed class SessionController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public SessionController(IWebHostEnvironment environment, IConfiguration configuration)
    {
        _environment = environment;
        _configuration = configuration;
    }

    [HttpGet("mode")]
    [AllowAnonymous]
    public IActionResult Mode() => Ok(new { mode = BffAuthenticationExtensions.IsEnabled(_environment, _configuration) ? "oidc-bff" : "local-jwt" });

    [HttpGet("login")]
    [AllowAnonymous]
    public IActionResult Login()
    {
        if (!BffAuthenticationExtensions.IsEnabled(_environment, _configuration)) return NotFound();
        return Challenge(new AuthenticationProperties { RedirectUri = "/" }, BffAuthenticationExtensions.OidcScheme);
    }

    [HttpGet("csrf")]
    [AllowAnonymous]
    public IActionResult Csrf([FromServices] IAntiforgery antiforgery)
    {
        if (!BffAuthenticationExtensions.IsEnabled(_environment, _configuration)) return NotFound();
        return Ok(new { token = antiforgery.GetAndStoreTokens(HttpContext).RequestToken });
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        if (!BffAuthenticationExtensions.IsEnabled(_environment, _configuration)) return NotFound();
        return SignOut(new AuthenticationProperties { RedirectUri = "/" },
            BffAuthenticationExtensions.CookieScheme, BffAuthenticationExtensions.OidcScheme);
    }
}
