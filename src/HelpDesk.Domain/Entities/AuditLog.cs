using System.Text.Json.Serialization;

public class AuditLog
{
    [JsonPropertyName("id")]
    public string id { get; set; } = Guid.NewGuid().ToString();

    public string Entity { get; set; } = "Ticket";
    public string EntityId { get; set; }
    public string Action { get; set; }
    public string User { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public object Data { get; set; }
}