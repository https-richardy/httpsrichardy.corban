namespace Corban.Crm.Domain.Filtering;

public sealed class PipelineFilters : Filters
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public static PipelineFilters WithoutFilters => new();
}