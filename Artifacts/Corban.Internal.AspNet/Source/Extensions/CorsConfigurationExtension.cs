namespace Corban.Internal.AspNet.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection")]
public static class CorsConfigurationExtension
{
    public static void AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Expose-Headers

                policy.WithExposedHeaders(Headers.Pagination);
                policy.WithExposedHeaders(Headers.Correlation);
            });
        });
    }
}
