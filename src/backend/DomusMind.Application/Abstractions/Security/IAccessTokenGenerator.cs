namespace DomusMind.Application.Abstractions.Security;

public interface IAccessTokenGenerator
{
    string Generate(Guid userId, string email, IReadOnlyCollection<string>? roles = null);
}
