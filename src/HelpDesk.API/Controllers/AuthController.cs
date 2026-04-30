using Microsoft.AspNetCore.Mvc;
using HelpDesk.Application.Services;

namespace HelpDesk.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] dynamic request)
    {
        string username = request.username;

        // Simulação (em produção validaria no banco)
        var token = _authService.GenerateToken(username);

        return Ok(new { token });
    }
}