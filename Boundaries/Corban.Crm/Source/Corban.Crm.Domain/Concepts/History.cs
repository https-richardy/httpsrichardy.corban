namespace Corban.Crm.Domain.Concepts;

public sealed record History(String Label, String Description, DateTime Occurrence, User Performer) :
    IValueObject<History>
{
    // always initialize objects with non-null default values to prevent null reference exceptions
    // see: https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/nullable-reference-types

    public static readonly History Undefined = new(
        Label: string.Empty,
        Description: string.Empty,

        Occurrence: DateTime.MinValue,
        Performer: new User(string.Empty, string.Empty)
    );
}