using DomusMind.Application.Abstractions.Security;

namespace DomusMind.Infrastructure.Auth;

/// <summary>
/// Foundation stub for family-scoped authorization.
/// Returns false until the Family context models membership.
/// </summary>
public sealed class FamilyAuthorizationService : IFamilyAuthorizationService
{
    public Task<bool> CanAccessFamilyAsync(
        Guid userId,
        Guid familyId,
        CancellationToken cancellationToken = default)
    {
        // Placeholder: will delegate to the Family aggregate in a future slice.
        return Task.FromResult(false);
    }
}
