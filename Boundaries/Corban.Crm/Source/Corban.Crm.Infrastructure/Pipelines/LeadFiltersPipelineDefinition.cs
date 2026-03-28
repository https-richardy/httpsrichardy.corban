namespace Corban.Crm.Infrastructure.Pipelines;

public static class LeadFiltersPipelineDefinition
{
    public static PipelineDefinition<Lead, BsonDocument> FilterLeads(
        this PipelineDefinition<Lead, BsonDocument> pipeline, LeadFilters filters)
    {
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Lead.Identifier, filters.Id),
            FilterDefinitions.MatchIfNotEmpty(Documents.Lead.CustomerId, filters.CustomerId),
            FilterDefinitions.MatchIfNotEmpty(Documents.Lead.PipelineId, filters.PipelineId),
            FilterDefinitions.MatchIfNotEmpty(Documents.Lead.Stage, filters.Stage),
            FilterDefinitions.MatchIfNotEmpty(Documents.Lead.Name, filters.Name),
            FilterDefinitions.MatchIfNotEmpty(Documents.Lead.PhoneNumber, filters.PhoneNumber),
            FilterDefinitions.MatchIfNotEmpty(Documents.Lead.Document, filters.Document),
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
