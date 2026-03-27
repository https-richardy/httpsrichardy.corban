namespace Corban.Crm.Domain.Concepts;

public sealed record Stage : IValueObject<Stage>
{
    public string Label { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public int Position { get; init; } = 0;

    // always initialize objects with non-null default values to prevent null reference exceptions
    // see: https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/nullable-reference-types
    public static readonly Stage Undefined = new()
    {
        Label = string.Empty,
        Color = string.Empty,
        Position = 0
    };
}