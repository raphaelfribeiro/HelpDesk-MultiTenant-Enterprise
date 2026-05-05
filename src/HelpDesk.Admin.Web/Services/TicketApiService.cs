using System.Net.Http.Json;
using HelpDesk.Admin.Web.Models;

namespace HelpDesk.Admin.Web.Services;

public class TicketApiService
{
    private readonly HttpClient _http;

    public TicketApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<TicketViewModel>> GetTicketsAsync()
    {
        var response = await _http.GetFromJsonAsync<List<TicketViewModel>>("/api/tickets");
        return response;
    }
}