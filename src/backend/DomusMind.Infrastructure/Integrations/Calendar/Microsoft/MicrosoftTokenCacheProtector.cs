using System.Security.Cryptography;
using System.Text;

namespace DomusMind.Infrastructure.Integrations.Calendar.Microsoft;

internal sealed class MicrosoftTokenCacheProtector
{
    private const string EnvelopePrefix = "v1.";
    private const int NonceSize = 12;
    private const int TagSize = 16;

    private readonly byte[] _key;

    public MicrosoftTokenCacheProtector(MicrosoftGraphOptions options)
    {
        options.TryGetEncryptionKey(out _key);
    }

    public string Protect(string value)
    {
        if (_key.Length != 32)
            throw new InvalidOperationException("A valid MicrosoftGraph token encryption key is required.");

        var nonce = RandomNumberGenerator.GetBytes(NonceSize);
        var plainText = Encoding.UTF8.GetBytes(value);
        var cipherText = new byte[plainText.Length];
        var tag = new byte[TagSize];

        using var aes = new AesGcm(_key, TagSize);
        aes.Encrypt(nonce, plainText, cipherText, tag);

        var envelope = new byte[NonceSize + TagSize + cipherText.Length];
        nonce.CopyTo(envelope, 0);
        tag.CopyTo(envelope, NonceSize);
        cipherText.CopyTo(envelope, NonceSize + TagSize);
        return EnvelopePrefix + Convert.ToBase64String(envelope);
    }

    public bool TryUnprotect(string value, out string plainText)
    {
        plainText = string.Empty;
        if (_key.Length != 32 || !value.StartsWith(EnvelopePrefix, StringComparison.Ordinal))
            return false;

        try
        {
            var envelope = Convert.FromBase64String(value[EnvelopePrefix.Length..]);
            if (envelope.Length < NonceSize + TagSize)
                return false;

            var nonce = envelope.AsSpan(0, NonceSize);
            var tag = envelope.AsSpan(NonceSize, TagSize);
            var cipherText = envelope.AsSpan(NonceSize + TagSize);
            var bytes = new byte[cipherText.Length];

            using var aes = new AesGcm(_key, TagSize);
            aes.Decrypt(nonce, cipherText, tag, bytes);
            plainText = Encoding.UTF8.GetString(bytes);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
        catch (CryptographicException)
        {
            return false;
        }
    }
}
