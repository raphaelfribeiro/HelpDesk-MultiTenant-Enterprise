namespace HelpDesk.Domain.Entities;

public class Ticket
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public Guid TenantId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    protected Ticket() { }

    public Ticket(
        string title,
        string description,
        Guid tenantId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        TenantId = tenantId;
        CreatedAt = DateTime.UtcNow;
    }
}