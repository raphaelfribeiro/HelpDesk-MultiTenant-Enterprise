namespace HelpDesk.Admin.Web.Models;

public class TicketViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}