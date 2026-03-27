namespace Corban.Crm.Domain.Aggregates;

public sealed class Pipeline : Aggregate
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<Stage> Stages { get; set; } = [];
    public ICollection<Lead> Leads { get; set; } = [];
}