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
        /* arrange: creating lead and matching filter */
        var lead = _fixture.Build<Lead>()
            .With(lead => lead.IsDeleted, false)
            .Create();

        /* act: persist lead and query using id filter */
        await _repository.InsertAsync(lead, cancellation: TestContext.Current.CancellationToken);

        var filters = LeadFilters.AsBuilder()
            .WithIdentifier(lead.Id)
            .Build();

        var leads = await _repository.GetLeadsAsync(filters, cancellation: TestContext.Current.CancellationToken);
        var persistedLead = leads.FirstOrDefault();

        /* assert: lead must be retrieved with same id and name */
        Assert.NotNull(persistedLead);

        Assert.Equal(lead.Id, persistedLead.Id);
        Assert.Equal(lead.Name, persistedLead.Name);
    }

    public async ValueTask DisposeAsync() => await Task.CompletedTask;
    public async ValueTask InitializeAsync()
    {
        await _mongo.CleanDatabaseAsync();
    }
}
