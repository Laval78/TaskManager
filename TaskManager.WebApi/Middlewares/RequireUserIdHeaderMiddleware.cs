public class RequireUserIdHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public RequireUserIdHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var requireUserId = endpoint?.Metadata.GetMetadata<RequireUserIdHeaderAttribute>() != null;

        if (requireUserId)
        {
            if (!context.Request.Headers.TryGetValue("User-Id", out var userId) ||
                !int.TryParse(userId, out var parsedUserId) ||
                parsedUserId <= 0)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Missing or invalid User-Id header.");
                return;
            }

            // Можешь сохранить UserId в context.Items["UserId"] если нужно
            context.Items["UserId"] = parsedUserId;
        }

        await _next(context);
    }
}