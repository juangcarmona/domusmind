namespace DomusMind.Infrastructure.Persistence;

public sealed class ExternalCalendarOAuthTransaction
{
    public string StateHash { get; private set; } = string.Empty;
    public Guid RequestedByUserId { get; private set; }
    public Guid FamilyId { get; private set; }
    public Guid MemberId { get; private set; }
    public string RedirectUri { get; private set; } = string.Empty;
    public string ProtectedPkceVerifier { get; private set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; private set; }

    private ExternalCalendarOAuthTransaction()
    {
    }

    public ExternalCalendarOAuthTransaction(
        string stateHash,
        Guid requestedByUserId,
        Guid familyId,
        Guid memberId,
        string redirectUri,
        string protectedPkceVerifier,
        DateTime expiresAtUtc)
    {
        StateHash = stateHash;
        RequestedByUserId = requestedByUserId;
        FamilyId = familyId;
        MemberId = memberId;
        RedirectUri = redirectUri;
        ProtectedPkceVerifier = protectedPkceVerifier;
        ExpiresAtUtc = expiresAtUtc;
    }
}
