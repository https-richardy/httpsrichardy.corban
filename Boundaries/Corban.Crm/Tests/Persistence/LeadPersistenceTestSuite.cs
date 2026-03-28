namespace Corban.Crm.TestSuite.Persistence;

public sealed class LeadPersistenceTestSuite : IClassFixture<MongoDatabase>, IAsyncLifetime
{
    private readonly IMongoDatabase _database;
    private readonly MongoDatabase _mongo;

    private readonly ILeadRepository _repository;
    private readonly Fixture _fixture = new();

    public LeadPersistenceTestSuite(MongoDatabase mongo)
    {
        _mongo = mongo;
        _database = _mongo.Database;
        _repository = new LeadRepository(_database);
    }

    [Fact(DisplayName = "[persistence] - when inserting a lead, then it must persist in the database")]
    public async Task WhenInsertingLead_ThenItMustPersistInDatabase()
    {
        /* arrange: create lead */
        var lead = _fixture.Build<Lead>()
            .With(lead => lead.Name, "insert.lead")
            .With(lead => lead.IsDeleted, false)
            .Create();

        /* act: persist lead and query using persisted id */
        await _repository.InsertAsync(lead, cancellation: TestContext.Current.CancellationToken);

        var filters = LeadFilters.AsBuilder()
            .WithIdentifier(lead.Id)
            .Build();

        var leads = await _repository.GetLeadsAsync(filters, cancellation: TestContext.Current.CancellationToken);
        var persistedLead = leads.FirstOrDefault();

        /* assert: lead must be retrieved with persisted id and values */
        Assert.NotNull(persistedLead);

        Assert.Equal(lead.Id, persistedLead.Id);
        Assert.Equal(lead.Name, persistedLead.Name);
    }

    [Fact(DisplayName = "[persistence] - when updating a lead, then updated fields must persist")]
    public async Task WhenUpdatingALead_ThenUpdatedFieldsMustPersist()
    {
        /* arrange: create and insert lead */
        var lead = _fixture.Build<Lead>()
            .With(lead => lead.Name, "update.lead")
            .With(lead => lead.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(lead, cancellation: TestContext.Current.CancellationToken);

        /* act: update lead name and persist changes */

        StateChanger.WithChanges(lead, lead =>
        {
            lead.Name = "updated.lead";
        });

        await _repository.UpdateAsync(lead, cancellation: TestContext.Current.CancellationToken);

        var filters = LeadFilters.AsBuilder()
            .WithIdentifier(lead.Id)
            .Build();

        var result = await _repository.GetLeadsAsync(filters, cancellation: TestContext.Current.CancellationToken);
        var updatedLead = result.FirstOrDefault();

        /* assert: updated lead must be retrieved with new field values */
        Assert.NotNull(updatedLead);

        Assert.Equal(lead.Id, updatedLead.Id);
        Assert.Equal("updated.lead", updatedLead.Name);
    }

    [Fact(DisplayName = "[persistence] - when deleting a lead, then it must be marked as deleted")]
    public async Task WhenDeletingALead_ThenItMustBeMarkedAsDeleted()
    {
        /* arrange: create and insert lead */
        var lead = _fixture.Build<Lead>()
            .With(lead => lead.Name, "delete.lead")
            .With(lead => lead.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(lead, cancellation: TestContext.Current.CancellationToken);

        var filters = LeadFilters.AsBuilder()
            .WithIdentifier(lead.Id)
            .Build();

        /* act: delete lead and query by id */
        var deleted = await _repository.DeleteAsync(lead, cancellation: TestContext.Current.CancellationToken);
        var resultAfterDelete = await _repository.GetLeadsAsync(filters, cancellation: TestContext.Current.CancellationToken);

        var deletedLead = resultAfterDelete.FirstOrDefault();

        /* assert: lead must remain persisted and marked as deleted */
        Assert.True(deleted);
        Assert.True(lead.IsDeleted);

        Assert.NotNull(deletedLead);
        Assert.Equal(lead.Id, deletedLead.Id);
        Assert.True(deletedLead.IsDeleted);
    }

    [Fact(DisplayName = "[persistence] - when filtering leads by id, then it must return matching lead")]
    public async Task WhenFilteringLeadsById_ThenItMustReturnMatchingLead()
    {
        /* arrange: insert two leads */
        var lead1 = _fixture.Build<Lead>()
            .With(lead => lead.IsDeleted, false)
            .Create();

        var lead2 = _fixture.Build<Lead>()
            .With(lead => lead.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(lead1, cancellation: TestContext.Current.CancellationToken);
        await _repository.InsertAsync(lead2, cancellation: TestContext.Current.CancellationToken);

        var filters = LeadFilters.AsBuilder()
            .WithIdentifier(lead1.Id)
            .Build();

        /* act: query leads filtered by id */
        var filteredLeads = await _repository.GetLeadsAsync(filters, cancellation: TestContext.Current.CancellationToken);

        /* assert: only lead1 must be returned */
        Assert.Single(filteredLeads);
        Assert.Equal(lead1.Id, filteredLeads.First().Id);
    }

    [Fact(DisplayName = "[persistence] - when filtering leads by name, then it must return matching leads")]
    public async Task WhenFilteringLeadsByName_ThenItMustReturnMatchingLeads()
    {
        /* arrange: insert two leads with different names */
        var lead1 = _fixture.Build<Lead>()
            .With(lead => lead.Name, "name.filter.1")
            .With(lead => lead.IsDeleted, false)
            .Create();

        var lead2 = _fixture.Build<Lead>()
            .With(lead => lead.Name, "name.filter.2")
            .With(lead => lead.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(lead1, cancellation: TestContext.Current.CancellationToken);
        await _repository.InsertAsync(lead2, cancellation: TestContext.Current.CancellationToken);

        var filters = LeadFilters.AsBuilder()
            .WithName("name.filter.1")
            .Build();

        /* act: query leads filtered by name */
        var filteredLeads = await _repository.GetLeadsAsync(filters, cancellation: TestContext.Current.CancellationToken);

        /* assert: only lead1 must be returned */
        Assert.Single(filteredLeads);
        Assert.Equal(lead1.Id, filteredLeads.First().Id);
    }

    [Fact(DisplayName = "[persistence] - when paginating 10 leads with page size 5, then it must return 5 leads per page")]
    public async Task WhenPaginatingTenLeads_ThenItMustReturnFiveLeadsPerPage()
    {
        /* arrange: create and insert 10 leads */
        var leads = Enumerable.Range(1, 10)
            .Select(index => _fixture.Build<Lead>()
                .With(lead => lead.Name, $"page.lead.{index}")
                .With(lead => lead.IsDeleted, false)
                .Create())
            .ToList();

        await _repository.InsertManyAsync(leads, cancellation: TestContext.Current.CancellationToken);

        var filtersPage1 = LeadFilters.AsBuilder()
            .WithPagination(PaginationFilters.From(pageNumber: 1, pageSize: 5))
            .Build();

        /* act: query first and second pages */
        var page1Results = await _repository.GetLeadsAsync(filtersPage1, cancellation: TestContext.Current.CancellationToken);

        var filtersPage2 = LeadFilters.AsBuilder()
            .WithPagination(PaginationFilters.From(pageNumber: 2, pageSize: 5))
            .Build();

        var page2Results = await _repository.GetLeadsAsync(filtersPage2, cancellation: TestContext.Current.CancellationToken);

        /* assert: each page must return exactly 5 leads */
        Assert.Equal(5, page1Results.Count);
        Assert.Equal(5, page2Results.Count);
    }

    [Fact(DisplayName = "[persistence] - when counting 10 leads without filters, then it must return 10")]
    public async Task WhenCountingTenLeadsWithoutFilters_ThenItMustReturnTen()
    {
        /* arrange: create and insert 10 leads */
        var leads = Enumerable.Range(1, 10)
            .Select(index => _fixture.Build<Lead>()
            .With(lead => lead.Name, $"count.lead.{index}")
            .With(lead => lead.IsDeleted, false)
            .Create())
            .ToList();

        await _repository.InsertManyAsync(leads, cancellation: TestContext.Current.CancellationToken);

        /* act: count leads matching filters */

        var filters = LeadFilters.WithoutFilters;
        var total = await _repository.CountLeadsAsync(filters, cancellation: TestContext.Current.CancellationToken);

        /* assert: total must be 10 */
        Assert.Equal(10, total);
    }

    [Fact(DisplayName = "[persistence] - when counting leads filtered by name, then it must return matching total")]
    public async Task WhenCountingLeadsFilteredByName_ThenItMustReturnMatchingTotal()
    {
        /* arrange: create and insert leads with different names */
        var lead1 = _fixture.Build<Lead>()
            .With(lead => lead.Name, "count.name.match")
            .With(lead => lead.IsDeleted, false)
            .Create();

        var lead2 = _fixture.Build<Lead>()
            .With(lead => lead.Name, "count.name.other")
            .With(lead => lead.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(lead1, cancellation: TestContext.Current.CancellationToken);
        await _repository.InsertAsync(lead2, cancellation: TestContext.Current.CancellationToken);

        var filters = LeadFilters.AsBuilder()
            .WithName("count.name.match")
            .Build();

        /* act: count leads matching name filter */
        var total = await _repository.CountLeadsAsync(filters, cancellation: TestContext.Current.CancellationToken);

        /* assert: total must match filtered leads */
        Assert.Equal(1, total);
    }

    public async ValueTask DisposeAsync() => await Task.CompletedTask;
    public async ValueTask InitializeAsync()
    {
        await _mongo.CleanDatabaseAsync();
    }
}
