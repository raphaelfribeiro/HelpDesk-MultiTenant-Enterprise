using System.Text.Json.Serialization;

namespace HelpDesk.API.Models.Audit;

public class TicketAuditData
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}