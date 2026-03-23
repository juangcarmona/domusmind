using System.Security.Cryptography;
using DomusMind.Application.Abstractions.Security;

namespace DomusMind.Infrastructure.Auth;

/// <summary>
/// Generates short, human-shareable temporary passwords.
/// Uses <see cref="RandomNumberGenerator"/> for cryptographically safe randomness.
/// Ambiguous characters (0, O, I, l, 1) are excluded to aid human transcription.
/// </summary>
public sealed class TemporaryPasswordGenerator : ITemporaryPasswordGenerator
{
    private const string Upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
    private const string Lower = "abcdefghjkmnpqrstuvwxyz";
    private const string Digits = "23456789";
    private const string All = Upper + Lower + Digits;

    public string Generate()
    {
        // Guarantee at least 2 of each character class for policy compliance
        var chars = new char[8];
        chars[0] = PickFrom(Upper);
        chars[1] = PickFrom(Upper);
        chars[2] = PickFrom(Lower);
        chars[3] = PickFrom(Lower);
        chars[4] = PickFrom(Digits);
        chars[5] = PickFrom(Digits);
        chars[6] = PickFrom(All);
        chars[7] = PickFrom(All);

        // Fisher-Yates shuffle
        for (int i = chars.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(0, i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }

        return new string(chars);
    }

    private static char PickFrom(string charset)
        => charset[RandomNumberGenerator.GetInt32(0, charset.Length)];
}
