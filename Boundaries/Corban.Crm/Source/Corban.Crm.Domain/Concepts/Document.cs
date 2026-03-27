namespace Corban.Crm.Domain.Concepts;

public sealed record Document(String Number, DocumentKind Kind) : IValueObject<Document>
{
    public string Number { get; init; } = Number.Trim().SanitizeNumbers();

    // always initialize objects with non-null default values to prevent null reference exceptions
    // see: https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/nullable-reference-types
    public static readonly Document Undefined = new(
        Number: String.Empty,
        Kind: DocumentKind.Undefined
    );
}