namespace Corban.Crm.TestSuite.Persistence;

public sealed class CustomerPersistenceTestSuite : IClassFixture<MongoDatabase>, IAsyncLifetime
{
    private readonly IMongoDatabase _database;
    private readonly MongoDatabase _mongo;

    private readonly ICustomerRepository _repository;
    private readonly Fixture _fixture = new();

    public CustomerPersistenceTestSuite(MongoDatabase mongo)
    {
        _mongo = mongo;
        _database = _mongo.Database;
        _repository = new CustomerRepository(_database);
    }

    [Fact(DisplayName = "[persistence] - when inserting a customer, then it must persist in the database")]
    public async Task WhenInsertingACustomer_ThenItMustPersistInDatabase()
    {
        /* arrange: create customer */
        var customer = _fixture.Build<Customer>()
            .With(customer => customer.Name, "insert.customer")
            .With(customer => customer.IsDeleted, false)
            .Create();

        /* act: persist customer and query using persisted id */
        await _repository.InsertAsync(customer, cancellation: TestContext.Current.CancellationToken);

        var filters = CustomerFilters.AsBuilder()
            .WithIdentifier(customer.Id)
            .Build();

        var result = await _repository.GetCustomersAsync(filters, cancellation: TestContext.Current.CancellationToken);
        var persistedCustomer = result.FirstOrDefault();

        /* assert: customer must be retrieved with persisted id and values */
        Assert.NotNull(persistedCustomer);

        Assert.Equal(customer.Id, persistedCustomer.Id);
        Assert.Equal(customer.Name, persistedCustomer.Name);
    }

    [Fact(DisplayName = "[persistence] - when updating a customer, then updated fields must persist")]
    public async Task WhenUpdatingACustomer_ThenUpdatedFieldsMustPersist()
    {
        /* arrange: create and insert customer */
        var customer = _fixture.Build<Customer>()
            .With(customer => customer.Name, "update.customer")
            .With(customer => customer.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(customer, cancellation: TestContext.Current.CancellationToken);

        /* act: update customer name and persist changes */

        StateChanger.WithChanges(customer, customer =>
        {
            customer.Name = "John Doe";
        });

        await _repository.UpdateAsync(customer, cancellation: TestContext.Current.CancellationToken);

        var filters = CustomerFilters.AsBuilder()
            .WithIdentifier(customer.Id)
            .Build();

        var result = await _repository.GetCustomersAsync(filters, cancellation: TestContext.Current.CancellationToken);
        var updatedCustomer = result.FirstOrDefault();

        /* assert: updated customer must be retrieved with new field values */
        Assert.NotNull(updatedCustomer);

        Assert.Equal(customer.Id, updatedCustomer.Id);
        Assert.Equal("John Doe", updatedCustomer.Name);
    }

    [Fact(DisplayName = "[persistence] - when deleting a customer, then it must be marked as deleted")]
    public async Task WhenDeletingACustomer_ThenItMustBeMarkedAsDeleted()
    {
        /* arrange: create and insert customer */
        var customer = _fixture.Build<Customer>()
            .With(customer => customer.Name, "delete.customer")
            .With(customer => customer.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(customer, cancellation: TestContext.Current.CancellationToken);

        var filters = CustomerFilters.AsBuilder()
            .WithIdentifier(customer.Id)
            .Build();

        /* act: delete customer and query by id */
        var deleted = await _repository.DeleteAsync(customer, cancellation: TestContext.Current.CancellationToken);
        var resultAfterDelete = await _repository.GetCustomersAsync(filters, cancellation: TestContext.Current.CancellationToken);

        var deletedCustomer = resultAfterDelete.FirstOrDefault();

        /* assert: customer must remain persisted and marked as deleted */
        Assert.True(deleted);
        Assert.True(customer.IsDeleted);

        Assert.NotNull(deletedCustomer);
        Assert.Equal(customer.Id, deletedCustomer.Id);
        Assert.True(deletedCustomer.IsDeleted);
    }

    [Fact(DisplayName = "[persistence] - when filtering customers by id, then it must return matching customer")]
    public async Task WhenFilteringCustomersById_ThenItMustReturnMatchingCustomer()
    {
        /* arrange: insert two customers */
        var customer1 = _fixture.Build<Customer>()
            .With(customer => customer.IsDeleted, false)
            .Create();

        var customer2 = _fixture.Build<Customer>()
            .With(customer => customer.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(customer1, cancellation: TestContext.Current.CancellationToken);
        await _repository.InsertAsync(customer2, cancellation: TestContext.Current.CancellationToken);

        var filters = CustomerFilters.AsBuilder()
            .WithIdentifier(customer1.Id)
            .Build();

        /* act: query customers filtered by id */
        var filteredCustomers = await _repository.GetCustomersAsync(filters, cancellation: TestContext.Current.CancellationToken);

        /* assert: only customer1 must be returned */
        Assert.Single(filteredCustomers);
        Assert.Equal(customer1.Id, filteredCustomers.First().Id);
    }

    [Fact(DisplayName = "[persistence] - when filtering customers by name, then it must return matching customers")]
    public async Task WhenFilteringCustomersByName_ThenItMustReturnMatchingCustomers()
    {
        /* arrange: insert two customers with different names */
        var customer1 = _fixture.Build<Customer>()
            .With(customer => customer.Name, "name.customer.1")
            .With(customer => customer.IsDeleted, false)
            .Create();

        var customer2 = _fixture.Build<Customer>()
            .With(customer => customer.Name, "name.customer.2")
            .With(customer => customer.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(customer1, cancellation: TestContext.Current.CancellationToken);
        await _repository.InsertAsync(customer2, cancellation: TestContext.Current.CancellationToken);

        var filters = CustomerFilters.AsBuilder()
            .WithName("name.customer.1")
            .Build();

        /* act: query customers filtered by name */
        var filteredCustomers = await _repository.GetCustomersAsync(filters, cancellation: TestContext.Current.CancellationToken);

        /* assert: only customer1 must be returned */
        Assert.Single(filteredCustomers);
        Assert.Equal(customer1.Id, filteredCustomers.First().Id);
    }

    [Fact(DisplayName = "[persistence] - when paginating 10 customers with page size 5, then it must return 5 customers per page")]
    public async Task WhenPaginatingTenCustomers_ThenItMustReturnFiveCustomersPerPage()
    {
        /* arrange: create and insert 10 customers */
        var customers = Enumerable.Range(1, 10)
            .Select(index => _fixture.Build<Customer>()
            .With(customer => customer.Name, $"page.customer.{index}")
            .With(customer => customer.IsDeleted, false)
            .Create())
            .ToList();

        await _repository.InsertManyAsync(customers, cancellation: TestContext.Current.CancellationToken);

        var filtersPage1 = CustomerFilters.AsBuilder()
            .WithPagination(PaginationFilters.From(pageNumber: 1, pageSize: 5))
            .Build();

        /* act: query first and second pages */
        var page1Results = await _repository.GetCustomersAsync(filtersPage1, cancellation: TestContext.Current.CancellationToken);

        var filtersPage2 = CustomerFilters.AsBuilder()
            .WithPagination(PaginationFilters.From(pageNumber: 2, pageSize: 5))
            .Build();

        var page2Results = await _repository.GetCustomersAsync(filtersPage2, cancellation: TestContext.Current.CancellationToken);

        /* assert: each page must return exactly 5 customers */
        Assert.Equal(5, page1Results.Count);
        Assert.Equal(5, page2Results.Count);
    }

    [Fact(DisplayName = "[persistence] - when counting 10 customers without filters, then it must return 10")]
    public async Task WhenCountingTenCustomersWithoutFilters_ThenItMustReturnTen()
    {
        /* arrange: create and insert 10 customers */
        var customers = Enumerable.Range(1, 10)
            .Select(index => _fixture.Build<Customer>()
            .With(customer => customer.Name, $"count.customer.{index}")
            .With(customer => customer.IsDeleted, false)
            .Create())
            .ToList();


        await _repository.InsertManyAsync(customers, cancellation: TestContext.Current.CancellationToken);

        /* act: count customers matching filters */

        var filters = CustomerFilters.WithoutFilters;
        var total = await _repository.CountCustomersAsync(filters, cancellation: TestContext.Current.CancellationToken);

        /* assert: total must be 10 */
        Assert.Equal(10, total);
    }

    [Fact(DisplayName = "[persistence] - when counting customers filtered by name, then it must return matching total")]
    public async Task WhenCountingCustomersFilteredByName_ThenItMustReturnMatchingTotal()
    {
        /* arrange: create and insert customers with different names */
        var customer1 = _fixture.Build<Customer>()
            .With(customer => customer.Name, "count.customer.match")
            .With(customer => customer.IsDeleted, false)
            .Create();

        var customer2 = _fixture.Build<Customer>()
            .With(customer => customer.Name, "count.customer.other")
            .With(customer => customer.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(customer1, cancellation: TestContext.Current.CancellationToken);
        await _repository.InsertAsync(customer2, cancellation: TestContext.Current.CancellationToken);

        var filters = CustomerFilters.AsBuilder()
            .WithName("count.customer.match")
            .Build();

        /* act: count customers matching name filter */
        var total = await _repository.CountCustomersAsync(filters, cancellation: TestContext.Current.CancellationToken);

        /* assert: total must match filtered customers */
        Assert.Equal(1, total);
    }

    public async ValueTask DisposeAsync() => await Task.CompletedTask;
    public async ValueTask InitializeAsync()
    {
        await _mongo.CleanDatabaseAsync();
    }
}
