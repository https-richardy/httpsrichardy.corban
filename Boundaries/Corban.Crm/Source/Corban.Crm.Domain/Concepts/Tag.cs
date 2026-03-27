namespace Corban.Crm.Domain.Concepts;

public sealed record Tag(string Label) : IValueObject<Tag>
{
    public string Label { get; init; } = Label.Trim().Normalize(NormalizationForm.FormC);

    // always initialize objects with non-null default values to prevent null reference exceptions
    // see: https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/nullable-reference-types

    public static readonly Tag Undefined = new(
        Label: string.Empty
    );
}