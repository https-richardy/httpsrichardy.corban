namespace Corban.Crm.Infrastructure.IoC.Extensions;

public static class DataPersistenceExtension
{
    public static void AddDataPersistence(this IServiceCollection services, ISettings settings)
    {
        services.AddSingleton<IMongoDatabase>(provider =>
        {
            var mongoClient = new MongoClient(settings.Database.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Database.DatabaseName);

            return database;
        });

        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<ILeadRepository, LeadRepository>();
        services.AddTransient<IPipelineRepository, PipelineRepository>();
    }
}
