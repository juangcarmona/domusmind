namespace DomusMind.Application.Abstractions.Security;

public interface ICalendarSubscriptionTokenGenerator
{
    (string Token, string Hash) Create();
    string Hash(string token);
}
