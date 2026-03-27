namespace Corban.Crm.Domain.Repositories;

public interface ILeadRepository : IAggregateCollection<Lead>
{
    public Task<IReadOnlyCollection<Lead>> GetLeadsAsync(
        LeadFilters filters,
        CancellationToken cancellation = default
    );

    public Task<System.Numerics.BigInteger> CountLeadsAsync(
        LeadFilters filters,
        CancellationToken cancellation = default
    );
}