namespace Corban.Crm.CrossCutting.Configurations;

public interface ISettings
{
    public FederationSettings Federation { get; }
    public ObservabilitySettings Observability { get; }
}
