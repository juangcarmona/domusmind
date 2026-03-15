namespace DomusMind.Application.Abstractions.Security;

/// <summary>
/// Foundation seam for family-scoped authorization.
/// The full membership check will be implemented once the Family context is ready.
/// </summary>
public interface IFamilyAuthorizationService
{
    /// <summary>
    /// Returns true if the user has access to the specified family.
    /// Currently always returns false until family membership is modelled.
    /// </summary>
    Task<bool> CanAccessFamilyAsync(Guid userId, Guid familyId, CancellationToken cancellationToken = default);
}
