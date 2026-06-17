using JOSYN.Adapter.IdentityAdapter.Contract;
using JOSYN.Foundation.JIP;
using JOSYN.Foundation.ResultPattern;

namespace Contoso.IdentityAdapter;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        if (!TryParseSessionGuid(args, out var sessionGuid))
            return FailWith("Usage: Contoso.IdentityAdapter.exe JOSYN-ADAPTER <session-guid>");

        var load = PasswordStore.Load(Path.Combine(AppContext.BaseDirectory, "contoso.credentials.ini"));
        if (!load.Succeeded)
            return FailWith($"Failed to load credentials: {load.ErrorMessage}");

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

    // Registers the IIdentityAdapter JIP handler on a fresh dispatcher.
    private static IJipDispatcher BuildDispatcher(PasswordStore store)
    {
        var dispatcher = new JipDispatcher();
        dispatcher.Register(nameof(IIdentityAdapter.GetPassword), GetPasswordHandler);
        return dispatcher;

        // Bridges IIdentityAdapter.GetPassword (Result<string>) → JIP wire type (Result<string?>).
        async Task<Result<string?>> GetPasswordHandler(string? username)
        {
            if (username is null)
                return Result<string?>.Fail("GetPassword: username must not be null.");
            var r = await store.GetPassword(username);
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
