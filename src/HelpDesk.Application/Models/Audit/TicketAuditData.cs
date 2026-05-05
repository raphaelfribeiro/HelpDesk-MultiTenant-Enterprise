using System.Text.Json.Serialization;

namespace HelpDesk.Application.Models.Audit;

public class TicketAuditData
{
    [JsonPropertyName("title")]
    public string title { get; set; }

    [JsonPropertyName("description")]
    public string description { get; set; }
}