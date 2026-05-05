using HelpDesk.Admin.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Admin.Web.Controllers;

[Authorize]
public class TicketsController : Controller
{
    private readonly TicketApiService _service;

    public TicketsController(TicketApiService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var tickets = await _service.GetTicketsAsync();
        return View(tickets);
    }
}