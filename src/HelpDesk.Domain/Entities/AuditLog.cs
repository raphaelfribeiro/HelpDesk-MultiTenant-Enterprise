using System.Text.Json.Serialization;

public class AuditLog
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    public string tenantId { get; set; }
    public string entity { get; set; } = "Ticket";
    public string entityId { get; set; }
    public string action { get; set; }
    public string user { get; set; }
    public DateTime timestamp { get; set; } = DateTime.UtcNow;
    public object data { get; set; }
}