namespace Contoso.DemoProduct.DemoJob;

public sealed record DemoResult
{
    public required string Echo { get; init; }
    public int ProcessedCount { get; init; }
    public DateTime CompletedAt { get; init; }
}
