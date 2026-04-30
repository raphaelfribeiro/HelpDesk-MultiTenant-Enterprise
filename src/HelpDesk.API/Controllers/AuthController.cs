using HelpDesk.Application.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { message = "E-mail é obrigatório." });
        }

        // ⚠️ Em produção: validar e-mail no banco / Identity
        var token = _authService.GenerateToken(request.Email);

        return Ok(new
        {
            token,
            email = request.Email
        });
    }
}