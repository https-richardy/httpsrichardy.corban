namespace Corban.Crm.Domain.Filtering;

public sealed class LeadFilters : Filters
{
    public string? CustomerId { get; set; }
    public string? PipelineId { get; set; }

    public string? Stage { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Document { get; set; }

    public static LeadFilters WithoutFilters  => new();
    public static LeadFiltersBuilder AsBuilder() => new();
}