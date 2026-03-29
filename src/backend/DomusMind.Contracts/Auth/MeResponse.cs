namespace DomusMind.Contracts.Auth;

/// <summary>
/// Identity projection returned by GET /api/auth/me.
/// Display priority: DisplayName > MemberName > Email.
/// </summary>
public sealed record MeResponse(
    Guid UserId,
    string? Email,
    string? DisplayName,
    Guid? MemberId,
    string? MemberName,
    bool IsManager,
    bool MustChangePassword,
    bool IsOperator);
