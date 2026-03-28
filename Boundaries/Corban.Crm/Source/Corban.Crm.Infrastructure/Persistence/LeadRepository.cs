namespace Corban.Crm.Infrastructure.Persistence;

public sealed class LeadRepository(IMongoDatabase database) :
    AggregateCollection<Lead>(database, Collections.Leads),
    ILeadRepository
{
    public async Task<IReadOnlyCollection<Lead>> GetLeadsAsync(
        LeadFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Lead>()
            .As<Lead, Lead, BsonDocument>()
            .FilterLeads(filters)
            .Paginate(filters.Pagination)
            .Sort(filters.Sort);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var leads = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<Lead>(bson))
            .ToList();

        return leads;
    }

    public async Task<System.Numerics.BigInteger> CountLeadsAsync(
        LeadFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Lead>()
            .As<Lead, Lead, BsonDocument>()
            .FilterLeads(filters)
            .Count();

        var aggregation = await _collection.AggregateAsync(pipeline, cancellationToken: cancellation);
        var result = await aggregation.FirstOrDefaultAsync(cancellation);

        return result?.Count ?? 0;
    }
}
