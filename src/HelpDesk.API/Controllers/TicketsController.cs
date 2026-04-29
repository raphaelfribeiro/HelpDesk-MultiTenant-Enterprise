using Microsoft.AspNetCore.Mvc;
using HelpDesk.Application.Services;
using HelpDesk.Application.DTOs;

namespace HelpDesk.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly TicketService _service;

    public TicketsController(TicketService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketDto dto)
    {
        var tenantId = GetTenantId();
        var id = await _service.CreateAsync(dto, tenantId);
        
        return Ok(new { Id = id });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tickets = await _service.GetAllAsync();

        return Ok(tickets);
    }

    private Guid GetTenantId()
    {
        if (HttpContext.Items.TryGetValue("TenantId", out var tenantId))
        {
            return Guid.Parse(tenantId.ToString());
        }

        throw new Exception("TenantId not provided");
    }
}