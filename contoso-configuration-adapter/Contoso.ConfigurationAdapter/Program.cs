using JOSYN.Adapter.ConfigurationAdapter.Contract;
using JOSYN.Foundation.JIP;
using JOSYN.Foundation.ResultPattern;

namespace Contoso.ConfigurationAdapter;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        if (!TryParseSessionGuid(args, out var sessionGuid))
            return FailWith("Usage: Contoso.ConfigurationAdapter.exe JOSYN-ADAPTER <session-guid>");

        var load = ConfigStore.Load(Path.Combine(AppContext.BaseDirectory, "contoso.config.ini"));
        if (!load.Succeeded)
            return FailWith($"Failed to load configuration: {load.ErrorMessage}");

        var result = await PipesServer.RunAsync(BuildServerArgs(sessionGuid, BuildDispatcher(load.Value)));

        return result.Succeeded ? 0 : 1;

        // ── helpers ───────────────────────────────────────────────────────
        static bool TryParseSessionGuid(string[] args, out Guid guid)
        {
            guid = Guid.Empty;
            return args is ["JOSYN-ADAPTER", _] && Guid.TryParse(args[1], out guid);
        }

        static int FailWith(string message)
        {
            Console.Error.WriteLine(message);
            return 1;
        }
    }

    // Registers the IConfigurationAdapter JIP handler on a fresh dispatcher.
    private static IJipDispatcher BuildDispatcher(ConfigStore store)
    {
        var dispatcher = new JipDispatcher();
        dispatcher.Register(nameof(IConfigurationAdapter.GetConfigValue), GetConfigValueHandler);
        return dispatcher;

        // Bridges IConfigurationAdapter.GetConfigValue (Result<string>) → JIP wire type (Result<string?>).
        async Task<Result<string?>> GetConfigValueHandler(string? settingPath)
        {
            if (settingPath is null)
                return Result<string?>.Fail("GetConfigValue: settingPath must not be null.");
            var r = await store.GetConfigValue(settingPath);
            return r.Succeeded ? Result<string?>.Success(r.Value) : r.ToResult<string?>();
        }
    }

    private static ServerStartArguments BuildServerArgs(Guid sessionGuid, IJipDispatcher dispatcher) =>
        new()
        {
            ConnectionTimeout       = TimeSpan.FromMinutes(30),
            HandleStringRequest     = dispatcher.Dispatch,
            SessionKey              = sessionGuid,
            HandleErrorNotification = (req, ex) =>
            {
                Console.Error.WriteLine($"Error handling '{req}': {ex}");
                return Task.CompletedTask;
            },
        };
}
