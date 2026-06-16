using JOSYN.Backend.IdentityAdapter.Contract;
using JOSYN.Foundation.ResultPattern;

namespace Contoso.IdentityAdapter;

internal sealed class PasswordStore : IIdentityAdapter
{
    private readonly IReadOnlyDictionary<string, string> _passwords;

    private PasswordStore(IReadOnlyDictionary<string, string> passwords) => _passwords = passwords;

    public Task<Result<string>> GetPassword(string username)
    {
        return _passwords.TryGetValue(username, out var password)
            ? Task.FromResult(Result<string>.Success(password))
            : Task.FromResult(Result<string>.Fail($"No password found for user '{username}'."));
    }

    internal static Result<PasswordStore> Load(string filePath)
    {
        if (!File.Exists(filePath))
            return Result<PasswordStore>.Fail($"Credentials file not found: '{filePath}'");

        try
        {
            var passwords = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var trimmed = line.Trim();
                if (trimmed.Length == 0 || trimmed.StartsWith('#') || trimmed.StartsWith(';'))
                    continue;

                var sep = trimmed.IndexOf('=');
                if (sep < 1) continue;

                var key   = trimmed[..sep].Trim();
                var value = trimmed[(sep + 1)..].Trim();
                passwords[key] = value;
            }
            return new PasswordStore(passwords);
        }
        catch (Exception ex)
        {
            return Result<PasswordStore>.Fail($"Failed to read credentials file: '{filePath}'", ex);
        }
    }
}
