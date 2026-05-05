namespace HelpDesk.AuditWorker.Models;

public class AuditEvent
{
    public string Event { get; set; }
    public string TicketId { get; set; }
    public string TenantId { get; set; }
    public DateTime Timestamp { get; set; }
}