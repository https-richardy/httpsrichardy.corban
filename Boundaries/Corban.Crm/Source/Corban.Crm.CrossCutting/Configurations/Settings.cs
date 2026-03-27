namespace Corban.Crm.CrossCutting.Configurations;

public sealed record Settings : ISettings
{
    public FederationSettings Federation { get; init; } = default!;
    public ObservabilitySettings Observability { get; init; } = default!;
}
