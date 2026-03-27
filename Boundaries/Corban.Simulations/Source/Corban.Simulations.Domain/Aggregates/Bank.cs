namespace Corban.Simulations.Domain.Aggregates;

public sealed class Bank : Aggregate
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
}