namespace DomusMind.Application.Abstractions.Security;

public interface ICurrentUser
{
    Guid? UserId { get; }

    string? Email { get; }

    bool IsAuthenticated { get; }

    IReadOnlyCollection<string> Roles { get; }
}
