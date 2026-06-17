using JOSYN.Adapter.ConfigurationAdapter.Contract;
using JOSYN.Foundation.ResultPattern;

namespace Contoso.ConfigurationAdapter;

internal sealed class ConfigStore : IConfigurationAdapter
{
    private readonly IReadOnlyDictionary<string, string> _values;

    private ConfigStore(IReadOnlyDictionary<string, string> values) => _values = values;

    public Task<Result<string>> GetConfigValue(string settingPath)
    {
        return _values.TryGetValue(settingPath, out var value)
            ? Task.FromResult(Result<string>.Success(value))
            : Task.FromResult(Result<string>.Fail($"No configuration value found for path '{settingPath}'."));
    }

    internal static Result<ConfigStore> Load(string filePath)
    {
        if (!File.Exists(filePath))
            return Result<ConfigStore>.Fail($"Configuration file not found: '{filePath}'");

        try
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var trimmed = line.Trim();
                if (trimmed.Length == 0 || trimmed.StartsWith('#') || trimmed.StartsWith(';'))
                    continue;

                var sep = trimmed.IndexOf('=');
                if (sep < 1) continue;

                var key   = trimmed[..sep].Trim();
                var value = trimmed[(sep + 1)..].Trim();
                values[key] = value;
            }
            return new ConfigStore(values);
        }
        catch (Exception ex)
        {
            return Result<ConfigStore>.Fail($"Failed to read configuration file: '{filePath}'", ex);
        }
    }
}
