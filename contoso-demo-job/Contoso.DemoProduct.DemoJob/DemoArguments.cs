namespace Contoso.DemoProduct.DemoJob;

public sealed record DemoArguments
{
    public required string Message { get; init; }
    public int RepeatCount { get; init; }
    public DateOnly ScheduledFor { get; init; }
    public bool IsHighPriority { get; init; }
    public decimal Budget { get; init; }
}
