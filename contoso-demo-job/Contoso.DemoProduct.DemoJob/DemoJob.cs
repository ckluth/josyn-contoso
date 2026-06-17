using JOSYN.JobHost.Attributes;

namespace Contoso.DemoProduct.DemoJob;

public static class DemoJob
{
    [JobEntryPoint]
    public static DemoResult Execute(DemoArguments args)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{JOSYN.JobHost.CurrentJob.Name} successfully invoked on {JOSYN.JobHost.CurrentJob.Environment}");
        Console.WriteLine($"technical-user: {Environment.UserName}");

        for (var i = 0; i < 5; i++)
        {
            Console.Write(".");
            Thread.Sleep(1000);
        }
        Console.ResetColor();
        
        return new DemoResult
        {
            Echo = $"[{(args.IsHighPriority ? "HIGH" : "NORMAL")}] {args.Message}",
            ProcessedCount = args.RepeatCount,
            CompletedAt = DateTime.Now,
        };
    }
}
