using DomusMind.Application.Abstractions.Messaging;
using DomusMind.Application.Abstractions.Security;
using DomusMind.Application.Auth;
using DomusMind.Application.Features.Auth.ChangePassword;
using DomusMind.Application.Features.Auth.GetCurrentUser;
using DomusMind.Application.Features.Auth.Login;
using DomusMind.Application.Features.Auth.Logout;
using DomusMind.Application.Features.Auth.RefreshToken;
using DomusMind.Application.Features.Auth.RegisterUser;
using DomusMind.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomusMind.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly ICurrentUser _currentUser;

    public AuthController(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    /// <summary>Returns the health status of the auth subsystem.</summary>
    [HttpGet("health")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult Health() => Ok(new { status = "ok" });

    /// <summary>
    /// Returns the identity of the currently authenticated user.
    /// Requires a valid JWT bearer token.
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(MeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Me(
        [FromServices] IQueryDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is null)
            return Unauthorized();

        try
        {
            var result = await dispatcher.Dispatch(
                new GetCurrentUserQuery(_currentUser.UserId.Value),
                cancellationToken);

            return Ok(result);
        }
        catch (AuthException ex) when (ex.Code == AuthErrorCode.UserNotFound)
        {
            return Unauthorized();
        }
    }

    /// <summary>
    /// Creates a new auth-user account.
    /// Email must be unique. Password must be at least 8 characters.
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await dispatcher.Dispatch(
                new RegisterUserCommand(request.Email, request.Password),
                cancellationToken);

            return CreatedAtAction(nameof(Me), response);
        }
        catch (AuthException ex) when (ex.Code == AuthErrorCode.EmailAlreadyTaken)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (AuthException ex) when (ex.Code == AuthErrorCode.WeakPassword)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Authenticates a user and returns an access token plus a refresh token.
    /// Copy the AccessToken value to the Authorize button above to test protected endpoints.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await dispatcher.Dispatch(
                new LoginCommand(request.Email, request.Password),
                cancellationToken);

            return Ok(response);
        }
        catch (AuthException ex) when (ex.Code == AuthErrorCode.InvalidCredentials)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (AuthException ex) when (ex.Code == AuthErrorCode.AccountDisabled)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Exchanges a valid refresh token for a new access token and rotated refresh token.
    /// The previous refresh token is revoked on success.
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenRequest request,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await dispatcher.Dispatch(
                new RefreshTokenCommand(request.RefreshToken),
                cancellationToken);

            return Ok(response);
        }
        catch (AuthException ex) when (ex.Code == AuthErrorCode.InvalidRefreshToken)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Changes the authenticated user's password.
    /// All existing refresh tokens for the user are revoked on success.
    /// </summary>
    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(typeof(ChangePasswordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        if (_currentUser.UserId is null)
            return Unauthorized();

        try
        {
            var response = await dispatcher.Dispatch(
                new ChangePasswordCommand(_currentUser.UserId.Value, request.CurrentPassword, request.NewPassword),
                cancellationToken);

            return Ok(response);
        }
        catch (AuthException ex) when (
            ex.Code == AuthErrorCode.InvalidCurrentPassword ||
            ex.Code == AuthErrorCode.WeakPassword ||
            ex.Code == AuthErrorCode.SamePassword)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (AuthException ex) when (ex.Code == AuthErrorCode.UserNotFound)
        {
            return Unauthorized();
        }
    }

    /// <summary>
    /// Revokes the provided refresh token. Access tokens remain valid until expiry.
    /// </summary>
    [HttpPost("logout")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout(
        [FromBody] LogoutRequest request,
        [FromServices] ICommandDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        await dispatcher.Dispatch(
            new LogoutCommand(request.RefreshToken),
            cancellationToken);

        return NoContent();
    }
}

