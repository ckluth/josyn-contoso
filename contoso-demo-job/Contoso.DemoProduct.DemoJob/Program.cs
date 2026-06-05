namespace Contoso.DemoProduct.DemoJob;

internal class Program
{
    private static async Task<int> Main(string[] args) => await JOSYN.JobHost.Core.Run(args);
}
