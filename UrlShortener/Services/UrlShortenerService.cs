using UrlShortener.DataStructures;

namespace UrlShortener.Services;

public sealed class UrlShortenerService
{
    private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    private readonly CustomHashTable<string, string> _shortToLong = new(comparer: StringComparer.Ordinal);
    private readonly CustomHashTable<string, string> _longToShort = new(comparer: StringComparer.OrdinalIgnoreCase);
    private long _counter;

    public bool TryResolve(string shortCode, out string longUrl)
    {
        return _shortToLong.TryGetValue(shortCode, out longUrl!);
    }

    public string Shorten(string longUrl)
    {
        if (string.IsNullOrWhiteSpace(longUrl))
        {
            throw new ArgumentException("URL is required.", nameof(longUrl));
        }

        if (!Uri.TryCreate(longUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("URL must be a valid http or https address.", nameof(longUrl));
        }

        var normalizedUrl = uri.ToString();

        if (_longToShort.TryGetValue(normalizedUrl, out var existingCode))
        {
            return existingCode;
        }

        var shortCode = EncodeBase62(Interlocked.Increment(ref _counter));
        _shortToLong.AddOrUpdate(shortCode, normalizedUrl);
        _longToShort.AddOrUpdate(normalizedUrl, shortCode);

        return shortCode;
    }

    private static string EncodeBase62(long value)
    {
        if (value == 0)
        {
            return Base62Chars[0].ToString();
        }

        Span<char> buffer = stackalloc char[11];
        var index = buffer.Length;

        while (value > 0)
        {
            buffer[--index] = Base62Chars[(int)(value % 62)];
            value /= 62;
        }

        return new string(buffer[index..]);
    }
}
