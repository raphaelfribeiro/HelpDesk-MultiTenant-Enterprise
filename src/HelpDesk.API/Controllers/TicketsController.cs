using HelpDesk.Application.DTOs;
using HelpDesk.Application.Services;
using HelpDesk.Infrastructure.Queries;
using HelpDesk.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TicketsController(TicketService service) : ControllerBase
{
    private readonly TicketService _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketDto dto)
    {
        if (!TryGetTenantId(out var tenantId))
        {
            return BadRequest("X-Tenant-Id header is required and must be a valid GUID.");
        }

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
        if (!TryGetTenantId(out var tenantId))
        {
            return BadRequest("X-Tenant-Id header is required and must be a valid GUID.");
        }

        var result = await repo.GetByTenantAsync(tenantId);
        return Ok(result);
    }

    private bool TryGetTenantId(out Guid tenantId)
    {
        tenantId = default;
        var httpContext = HttpContext ?? throw new InvalidOperationException("HttpContext is not available.");

        if (httpContext.Items.TryGetValue("TenantId", out var tenantIdObj) && tenantIdObj is Guid tenantGuid)
        {
            tenantId = tenantGuid;
            return true;
        }

        return false;
    }
}