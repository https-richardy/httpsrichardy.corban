namespace Corban.Crm.Domain.Repositories;

public interface IPipelineRepository : IAggregateCollection<Pipeline>
{
    public Task<IReadOnlyCollection<Pipeline>> GetPipelinesAsync(
        PipelineFilters filters,
        CancellationToken cancellation = default
    );

    public Task<System.Numerics.BigInteger> CountPipelinesAsync(
        PipelineFilters filters,
        CancellationToken cancellation = default
    );
}