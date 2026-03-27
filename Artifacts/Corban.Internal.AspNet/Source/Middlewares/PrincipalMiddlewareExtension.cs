namespace Corban.Internal.AspNet.Middlewares;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection")]
public static class PrincipalMiddlewareExtension
{
    public static IApplicationBuilder UsePrincipalMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<PrincipalMiddleware>();
    }
}