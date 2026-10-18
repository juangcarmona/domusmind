using System.Security.Cryptography;
using System.Text;
using DomusMind.Application.Abstractions.Security;

namespace DomusMind.Infrastructure.Auth;

public sealed class CalendarSubscriptionTokenGenerator : ICalendarSubscriptionTokenGenerator
{
    public (string Token, string Hash) Create()
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        return (token, Hash(token));
    }

    public string Hash(string token)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
