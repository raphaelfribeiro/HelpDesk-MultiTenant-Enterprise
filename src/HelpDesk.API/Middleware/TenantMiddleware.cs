namespace HelpDesk.API.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantId))
        {
            if (Guid.TryParse(tenantId, out var tenantGuid))
            {
                context.Items["TenantId"] = tenantGuid;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid X-Tenant-Id header format. Expected a GUID.");
                return;
            }
        }

        await _next(context);
    }
}