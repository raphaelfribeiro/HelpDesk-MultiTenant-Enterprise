using System.Net.Http.Headers;

public class AuthenticationHeaderHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _accessor;

    public AuthenticationHeaderHandler(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _accessor.HttpContext?.Request.Cookies["auth_token"];

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        // Add default tenant id
        request.Headers.Add("X-Tenant-Id", "00000000-0000-0000-0000-000000000001");

        return base.SendAsync(request, cancellationToken);
    }
}