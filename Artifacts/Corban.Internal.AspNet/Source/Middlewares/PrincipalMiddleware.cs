namespace Corban.Internal.AspNet.Middlewares;

public sealed class PrincipalMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var requiresAuth = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() != null;

        if (!requiresAuth || context.User.Identity?.IsAuthenticated != true)
        {
            await next(context);
            return;
        }

        var userName = context.User.Claims.FirstOrDefault(claim => claim.Type == "preferred_username");
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier);

        if (userId == null || string.IsNullOrWhiteSpace(userId.Value))
        {
            await next(context);
            return;
        }

        if (userName == null || string.IsNullOrWhiteSpace(userName.Value))
        {
            await next(context);
            return;
        }

        /* enriches logging and monitoring contexts with user information */
        /* enabling traceability of user actions across logs and error monitoring tools */

        using (LogContext.PushProperty("user_id", userId.Value))
        using (LogContext.PushProperty("user_name", userName.Value))

        using (SentrySdk.PushScope())
        {
            SentrySdk.ConfigureScope(scope =>
            {
                scope.SetTag("user_id", userId.Value);
                scope.SetTag("user_name", userName.Value);
            });

            await next(context);
        }
    }
}