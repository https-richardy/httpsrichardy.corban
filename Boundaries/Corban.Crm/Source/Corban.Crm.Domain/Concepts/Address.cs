namespace Corban.Crm.Domain.Concepts;

public sealed record Address : IValueObject<Address>
{
    public string Street { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;

    public string State { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string Neighborhood { get; init; } = string.Empty;

    public string Complement { get; init; } = string.Empty;
    public string Reference { get; init; } = string.Empty;
}