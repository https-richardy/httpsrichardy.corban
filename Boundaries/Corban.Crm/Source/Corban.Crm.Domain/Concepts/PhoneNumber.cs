namespace Corban.Crm.Domain.Concepts;

public sealed record PhoneNumber(String Number) : IValueObject<PhoneNumber>
{
    public string Number = Number.SanitizeNumbers();

    // always initialize objects with non-null default values to prevent null reference exceptions
    // see: https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/nullable-reference-types
    public static readonly PhoneNumber Undefined = new(
        Number: String.Empty
    );
}