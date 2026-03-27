namespace Corban.Crm.Domain.Concepts;

public sealed record Marketing(LeadSource Source, String Audience) :
    IValueObject<Marketing>
{
    // always initialize objects with non-null default values to prevent null reference exceptions
    // see: https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/nullable-reference-types
    public static readonly Marketing Undefined = new(
        Source: LeadSource.Undefined,
        Audience: string.Empty
    );
}