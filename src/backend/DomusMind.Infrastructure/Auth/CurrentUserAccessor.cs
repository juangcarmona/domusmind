using System.Security.Claims;
using DomusMind.Application.Abstractions.Security;
using Microsoft.AspNetCore.Http;

namespace DomusMind.Infrastructure.Auth;

public sealed class CurrentUserAccessor : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier);

            return claim is not null && Guid.TryParse(claim.Value, out var id)
                ? id
                : null;
        }
    }

    public string? Email
    {
        get
        {
            return _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.Email)?
                .Value;
        }
    }

    public bool IsAuthenticated
        => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;

    public IReadOnlyCollection<string> Roles
        => _httpContextAccessor.HttpContext?.User?.Claims
               .Where(c => c.Type == ClaimTypes.Role)
               .Select(c => c.Value)
               .ToList()
           ?? [];
}