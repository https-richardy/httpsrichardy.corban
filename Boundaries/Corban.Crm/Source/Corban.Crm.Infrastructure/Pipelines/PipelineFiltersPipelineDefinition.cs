namespace Corban.Crm.Infrastructure.Pipelines;

public static class PipelineFiltersPipelineDefinition
{
    public static PipelineDefinition<Pipeline, BsonDocument> FilterPipelines(
        this PipelineDefinition<Pipeline, BsonDocument> pipeline, PipelineFilters filters)
    {
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Pipeline.Identifier, filters.Id),
            FilterDefinitions.MatchIfContains(Documents.Pipeline.Name, filters.Name),
            FilterDefinitions.MatchIfContains(Documents.Pipeline.Description, filters.Description),
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
