namespace Corban.Crm.Infrastructure.Pipelines;

public static class CustomerFiltersPipelineDefinition
{
    public static PipelineDefinition<Customer, BsonDocument> FilterCustomers(
        this PipelineDefinition<Customer, BsonDocument> pipeline, CustomerFilters filters)
    {
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Customer.Identifier, filters.Id),
            FilterDefinitions.MatchIfNotEmpty(Documents.Customer.Name, filters.Name),
            FilterDefinitions.MatchIfNotEmpty(Documents.Customer.DocumentNumber, filters.Cnpj),
            FilterDefinitions.MatchIfNotEmpty(Documents.Customer.DocumentNumber, filters.Cpf),
            FilterDefinitions.MatchIfNotEmpty(Documents.Customer.PhoneNumber, filters.PhoneNumber),
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
