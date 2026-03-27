namespace Corban.Crm.Domain.Repositories;

public interface ICustomerRepository : IAggregateCollection<Customer>
{
    public Task<IReadOnlyCollection<Customer>> GetCustomersAsync(
        CustomerFilters filters,
        CancellationToken cancellation = default
    );

    public Task<System.Numerics.BigInteger> CountCustomersAsync(
        CustomerFilters filters,
        CancellationToken cancellation = default
    );
}