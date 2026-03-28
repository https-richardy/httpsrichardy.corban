namespace Corban.Crm.TestSuite.Persistence;

public sealed class PipelinePersistenceTestSuite : IClassFixture<MongoDatabase>, IAsyncLifetime
{
    private readonly IMongoDatabase _database;
    private readonly MongoDatabase _mongo;

    private readonly IPipelineRepository _repository;
    private readonly Fixture _fixture = new();

    public PipelinePersistenceTestSuite(MongoDatabase mongo)
    {
        _mongo = mongo;
        _database = _mongo.Database;
        _repository = new PipelineRepository(_database);
    }

    [Fact(DisplayName = "[persistence] - when inserting a pipeline, then it must persist in the database")]
    public async Task WhenInsertingAPipeline_ThenItMustPersistInDatabase()
    {
        /* arrange: create pipeline */
        var pipeline = _fixture.Build<Pipeline>()
            .With(pipeline => pipeline.Name, "insert.pipeline")
            .With(pipeline => pipeline.Description, "insert.description")
            .With(pipeline => pipeline.IsDeleted, false)
            .Create();

        /* act: persist pipeline and query using persisted id */
        await _repository.InsertAsync(pipeline, cancellation: TestContext.Current.CancellationToken);

        var filters = PipelineFilters.AsBuilder()
            .WithIdentifier(pipeline.Id)
            .Build();

        var result = await _repository.GetPipelinesAsync(filters, cancellation: TestContext.Current.CancellationToken);
        var persistedPipeline = result.FirstOrDefault();

        /* assert: pipeline must be retrieved with persisted id and values */
        Assert.NotNull(persistedPipeline);

        Assert.Equal(pipeline.Id, persistedPipeline.Id);
        Assert.Equal(pipeline.Name, persistedPipeline.Name);
    }

    [Fact(DisplayName = "[persistence] - when updating a pipeline, then updated fields must persist")]
    public async Task WhenUpdatingAPipeline_ThenUpdatedFieldsMustPersist()
    {
        /* arrange: create and insert pipeline */
        var pipeline = _fixture.Build<Pipeline>()
            .With(pipeline => pipeline.Name, "update.pipeline")
            .With(pipeline => pipeline.Description, "update.description")
            .With(pipeline => pipeline.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(pipeline, cancellation: TestContext.Current.CancellationToken);

        /* act: update pipeline name and persist changes */

        StateChanger.WithChanges(pipeline, pipeline =>
        {
            pipeline.Name = "corban.crm.leads";
        });

        await _repository.UpdateAsync(pipeline, cancellation: TestContext.Current.CancellationToken);

        var filters = PipelineFilters.AsBuilder()
            .WithIdentifier(pipeline.Id)
            .Build();

        var result = await _repository.GetPipelinesAsync(filters, cancellation: TestContext.Current.CancellationToken);
        var updatedPipeline = result.FirstOrDefault();

        /* assert: updated pipeline must be retrieved with new field values */
        Assert.NotNull(updatedPipeline);

        Assert.Equal(pipeline.Id, updatedPipeline.Id);
        Assert.Equal("corban.crm.leads", updatedPipeline.Name);
    }

    [Fact(DisplayName = "[persistence] - when deleting a pipeline, then it must be marked as deleted")]
    public async Task WhenDeletingAPipeline_ThenItMustBeMarkedAsDeleted()
    {
        /* arrange: create and insert pipeline */
        var pipeline = _fixture.Build<Pipeline>()
            .With(pipeline => pipeline.Name, "delete.pipeline")
            .With(pipeline => pipeline.Description, "delete.description")
            .With(pipeline => pipeline.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(pipeline, cancellation: TestContext.Current.CancellationToken);

        var filters = PipelineFilters.AsBuilder()
            .WithIdentifier(pipeline.Id)
            .Build();

        /* act: delete pipeline and query by id */
        var deleted = await _repository.DeleteAsync(pipeline, cancellation: TestContext.Current.CancellationToken);
        var resultAfterDelete = await _repository.GetPipelinesAsync(filters, cancellation: TestContext.Current.CancellationToken);

        var deletedPipeline = resultAfterDelete.FirstOrDefault();

        /* assert: pipeline must remain persisted and marked as deleted */
        Assert.True(deleted);
        Assert.True(pipeline.IsDeleted);

        Assert.NotNull(deletedPipeline);
        Assert.Equal(pipeline.Id, deletedPipeline.Id);
        Assert.True(deletedPipeline.IsDeleted);
    }

    [Fact(DisplayName = "[persistence] - when filtering pipelines by id, then it must return matching pipeline")]
    public async Task WhenFilteringPipelinesById_ThenItMustReturnMatchingPipeline()
    {
        /* arrange: insert two pipelines */
        var pipeline1 = _fixture.Build<Pipeline>()
            .With(pipeline => pipeline.Description, "desc.pipeline.1")
            .With(pipeline => pipeline.IsDeleted, false)
            .Create();

        var pipeline2 = _fixture.Build<Pipeline>()
            .With(pipeline => pipeline.Description, "desc.pipeline.2")
            .With(pipeline => pipeline.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(pipeline1, cancellation: TestContext.Current.CancellationToken);
        await _repository.InsertAsync(pipeline2, cancellation: TestContext.Current.CancellationToken);

        var filters = PipelineFilters.AsBuilder()
            .WithIdentifier(pipeline1.Id)
            .Build();

        /* act: query pipelines filtered by id */
        var filteredPipelines = await _repository.GetPipelinesAsync(filters, cancellation: TestContext.Current.CancellationToken);

        /* assert: only pipeline1 must be returned */
        Assert.Single(filteredPipelines);
        Assert.Equal(pipeline1.Id, filteredPipelines.First().Id);
    }

    [Fact(DisplayName = "[persistence] - when filtering pipelines by name, then it must return matching pipelines")]
    public async Task WhenFilteringPipelinesByName_ThenItMustReturnMatchingPipelines()
    {
        /* arrange: insert two pipelines with different names */
        var pipeline1 = _fixture.Build<Pipeline>()
            .With(pipeline => pipeline.Name, "name.pipeline.1")
            .With(pipeline => pipeline.Description, "desc.pipeline.1")
            .With(pipeline => pipeline.IsDeleted, false)
            .Create();

        var pipeline2 = _fixture.Build<Pipeline>()
            .With(pipeline => pipeline.Name, "name.pipeline.2")
            .With(pipeline => pipeline.Description, "desc.pipeline.2")
            .With(pipeline => pipeline.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(pipeline1, cancellation: TestContext.Current.CancellationToken);
        await _repository.InsertAsync(pipeline2, cancellation: TestContext.Current.CancellationToken);

        var filters = PipelineFilters.AsBuilder()
            .WithName("name.pipeline.1")
            .Build();

        /* act: query pipelines filtered by name */
        var filteredPipelines = await _repository.GetPipelinesAsync(filters, cancellation: TestContext.Current.CancellationToken);

        /* assert: only pipeline1 must be returned */
        Assert.Single(filteredPipelines);
        Assert.Equal(pipeline1.Id, filteredPipelines.First().Id);
    }

    [Fact(DisplayName = "[persistence] - when paginating 10 pipelines with page size 5, then it must return 5 pipelines per page")]
    public async Task WhenPaginatingTenPipelines_ThenItMustReturnFivePipelinesPerPage()
    {
        /* arrange: create and insert 10 pipelines */
        var pipelines = Enumerable.Range(1, 10)
            .Select(index => _fixture.Build<Pipeline>()
            .With(pipeline => pipeline.Name, $"page.pipeline.{index}")
            .With(pipeline => pipeline.Description, $"page.description.{index}")
            .With(pipeline => pipeline.IsDeleted, false)
            .Create())
            .ToList();

        await _repository.InsertManyAsync(pipelines, cancellation: TestContext.Current.CancellationToken);

        var filtersPage1 = PipelineFilters.AsBuilder()
            .WithPagination(PaginationFilters.From(pageNumber: 1, pageSize: 5))
            .Build();

        /* act: query first and second pages */
        var page1Results = await _repository.GetPipelinesAsync(filtersPage1, cancellation: TestContext.Current.CancellationToken);

        var filtersPage2 = PipelineFilters.AsBuilder()
            .WithPagination(PaginationFilters.From(pageNumber: 2, pageSize: 5))
            .Build();

        var page2Results = await _repository.GetPipelinesAsync(filtersPage2, cancellation: TestContext.Current.CancellationToken);

        /* assert: each page must return exactly 5 pipelines */
        Assert.Equal(5, page1Results.Count);
        Assert.Equal(5, page2Results.Count);
    }

    [Fact(DisplayName = "[persistence] - when counting 10 pipelines without filters, then it must return 10")]
    public async Task WhenCountingTenPipelinesWithoutFilters_ThenItMustReturnTen()
    {
        /* arrange: create and insert 10 pipelines */
        var pipelines = Enumerable.Range(1, 10)
            .Select(index => _fixture.Build<Pipeline>()
            .With(pipeline => pipeline.Name, $"count.pipeline.{index}")
            .With(pipeline => pipeline.Description, $"count.description.{index}")
            .With(pipeline => pipeline.IsDeleted, false)
            .Create())
            .ToList();

        await _repository.InsertManyAsync(pipelines, cancellation: TestContext.Current.CancellationToken);

        /* act: count pipelines matching filters */
        var filters = PipelineFilters.WithoutFilters;
        var total = await _repository.CountPipelinesAsync(filters, cancellation: TestContext.Current.CancellationToken);

        /* assert: total must be 10 */
        Assert.Equal(10, total);
    }

    [Fact(DisplayName = "[persistence] - when counting pipelines filtered by name, then it must return matching total")]
    public async Task WhenCountingPipelinesFilteredByName_ThenItMustReturnMatchingTotal()
    {
        /* arrange: create and insert pipelines with different names */
        var pipeline1 = _fixture.Build<Pipeline>()
            .With(pipeline => pipeline.Name, "count.pipeline.match")
            .With(pipeline => pipeline.Description, "count.description.match")
            .With(pipeline => pipeline.IsDeleted, false)
            .Create();

        var pipeline2 = _fixture.Build<Pipeline>()
            .With(pipeline => pipeline.Name, "count.pipeline.other")
            .With(pipeline => pipeline.Description, "count.description.other")
            .With(pipeline => pipeline.IsDeleted, false)
            .Create();

        await _repository.InsertAsync(pipeline1, cancellation: TestContext.Current.CancellationToken);
        await _repository.InsertAsync(pipeline2, cancellation: TestContext.Current.CancellationToken);

        var filters = PipelineFilters.AsBuilder()
            .WithName("count.pipeline.match")
            .Build();

        /* act: count pipelines matching name filter */
        var total = await _repository.CountPipelinesAsync(filters, cancellation: TestContext.Current.CancellationToken);

        /* assert: total must match filtered pipelines */
        Assert.Equal(1, total);
    }

    public async ValueTask DisposeAsync() => await Task.CompletedTask;
    public async ValueTask InitializeAsync()
    {
        await _mongo.CleanDatabaseAsync();
    }
}
