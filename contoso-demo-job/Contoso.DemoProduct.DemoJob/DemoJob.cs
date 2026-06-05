using JOSYN.JobHost.Attributes;

namespace Contoso.DemoProduct.DemoJob;

public static class DemoJob
{
    [JobEntryPoint]
    public static DemoResult Execute(DemoArguments args)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{JOSYN.JobHost.CurrentJob.Name} successfully invoked on {JOSYN.JobHost.CurrentJob.Environment}");
        Console.ResetColor();
        
        return new DemoResult
        {
            Echo = $"[{(args.IsHighPriority ? "HIGH" : "NORMAL")}] {args.Message}",
            ProcessedCount = args.RepeatCount,
            CompletedAt = DateTime.Now,
        };
    }
}
