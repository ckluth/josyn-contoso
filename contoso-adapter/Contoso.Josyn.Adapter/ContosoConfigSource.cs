using JOSYN.Backend.Contracts;
using JOSYN.Foundation.ResultPattern;

namespace Contoso.Josyn.Adapter;

/// <summary>
/// Demo implementation of <see cref="IConfigSource"/> for the JOSYN adapter PoC.
/// Returns hardcoded fake values — simulates what a real company adapter would do
/// (e.g. reading from a company config service instead of the JOSYN SQL store).
/// </summary>
public sealed class ContosoConfigSource : IConfigSource
{
    private static readonly Dictionary<string, string> FakeValues = new()
    {
        ["RuntimeEnvironment"] = "INT",
    };

    /// <inheritdoc/>
    public Result<string> GetValue(string key)
        => FakeValues.TryGetValue(key, out var value)
            ? Result<string>.Success(value)
            : Result.Error($"[Contoso] Konfigurationsschlüssel nicht gefunden: '{key}'");
}
