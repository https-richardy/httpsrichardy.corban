namespace Corban.Simulations.Domain.Aggregates;

public sealed class LendingPartner : Aggregate
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public decimal Margin { get; set; } = 0m;
}