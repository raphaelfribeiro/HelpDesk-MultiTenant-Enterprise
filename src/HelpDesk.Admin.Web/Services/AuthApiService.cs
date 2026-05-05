using System.Net.Http.Json;

public class AuthApiService
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _accessor;

    public AuthApiService(HttpClient http, IHttpContextAccessor accessor)
    {
        _http = http;
        _accessor = accessor;
    }

    public async Task<string> LoginAsync(string email)
    {
        Console.WriteLine("BASE ADDRESS: " + _http.BaseAddress);

        var response = await _http.PostAsJsonAsync("api/auth/login", new { Email = email });

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (result?.Token == null)
        {
            throw new InvalidOperationException("Failed to retrieve token from login response.");
        }

        return result.Token;
    }
}

public class LoginResponse
{
    public string? Token { get; set; }
    public string? Email { get; set; }
}