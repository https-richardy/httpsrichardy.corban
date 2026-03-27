namespace Corban.Crm.Domain.Aggregates;

public sealed class Customer : Aggregate
{
    public String Name { get; set; } = string.Empty;
    public Concepts.Document Document { get; set; } = Concepts.Document.Undefined;
    public Marketing Marketing { get; set; } = Marketing.Undefined;

    public Gender Gender { get; set; } = Gender.Unspecified;
    public DateTime BirthDate { get; set; } = DateTime.MinValue;

    public Dictionary<string, string> Metadata { get; set; } = [];

    public ICollection<PhoneNumber> Phones { get; set; } = [];
    public ICollection<Address> Addresses { get; set; } = [];
}