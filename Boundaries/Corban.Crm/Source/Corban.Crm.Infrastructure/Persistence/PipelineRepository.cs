namespace Corban.Crm.Infrastructure.Persistence;

public sealed class PipelineRepository(IMongoDatabase database) :
    AggregateCollection<Pipeline>(database, Collections.Pipelines),
    IPipelineRepository
{
    public async Task<IReadOnlyCollection<Pipeline>> GetPipelinesAsync(
        PipelineFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Pipeline>()
            .As<Pipeline, Pipeline, BsonDocument>()
            .FilterPipelines(filters)
            .Paginate(filters.Pagination)
            .Sort(filters.Sort);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var pipelines = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<Pipeline>(bson))
            .ToList();

        return pipelines;
    }

    public async Task<System.Numerics.BigInteger> CountPipelinesAsync(
        PipelineFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Pipeline>()
            .As<Pipeline, Pipeline, BsonDocument>()
            .FilterPipelines(filters)
            .Count();

        var aggregation = await _collection.AggregateAsync(pipeline, cancellationToken: cancellation);
        var result = await aggregation.FirstOrDefaultAsync(cancellation);

        return result?.Count ?? 0;
    }
}
