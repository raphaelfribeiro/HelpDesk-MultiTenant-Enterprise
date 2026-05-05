using HelpDesk.Desktop.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace HelpDesk.Desktop.Services;

public class TicketApiService
{
    private readonly HttpClient _http = new();

    public async Task<List<TicketModel>> GetTicketsAsync()
    {
        var response = await _http.GetFromJsonAsync<List<TicketModel>>("/api/tickets");
        return response;
    }
}