using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _http;

    public UserContext(IHttpContextAccessor http)
    {
        _http = http;
    }

    public string GetUserEmail()
    {
        return _http.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
    }
}