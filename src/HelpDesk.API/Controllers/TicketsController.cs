using HelpDesk.Application.DTOs;
using HelpDesk.Application.Services;
using HelpDesk.Infrastructure.Queries;
using HelpDesk.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard([FromServices] TicketQuery query)
    {
        var result = await query.GetDashboardAsync();
        return Ok(result);
    }

    [HttpGet("tenant")]
    public async Task<IActionResult> GetByTenant([FromServices] TicketAdoRepository repo)
    {
        var tenantId = GetTenantId();

        var result = await repo.GetByTenantAsync(tenantId);

        return Ok(result);
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