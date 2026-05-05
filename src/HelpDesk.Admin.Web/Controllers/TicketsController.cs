using Microsoft.AspNetCore.Mvc;
using HelpDesk.Admin.Web.Services;

namespace HelpDesk.Admin.Web.Controllers;

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