namespace Corban.Crm.Domain.Filtering;

public sealed class CustomerFilters : Filters
{
    public string? Name { get; set; }
    public string? Cnpj { get; set; }
    public string? Cpf { get; set; }
    public string? PhoneNumber { get; set; }

    public LeadSource? Source { get; set; }
    public Gender? Gender { get ; set; }
    public DateTime? BirthDate { get; set; }
    public Dictionary<string, string>? Metadata { get; set; }

    public static CustomerFilters WithoutFilters  => new();
    public static CustomerFiltersBuilder AsBuilder() => new();
}