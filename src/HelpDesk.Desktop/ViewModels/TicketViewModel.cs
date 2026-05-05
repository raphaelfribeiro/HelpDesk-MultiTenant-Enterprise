using System.Collections.ObjectModel;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;

namespace HelpDesk.Desktop.ViewModels;

public class TicketViewModel
{
    private readonly TicketApiService _service = new();

    public ObservableCollection<TicketModel> Tickets { get; set; } = new();

    public async Task LoadAsync()
    {
        var data = await _service.GetTicketsAsync();

        Tickets.Clear();

        foreach (var item in data)
        {
            Tickets.Add(item);
        }       
    }
}