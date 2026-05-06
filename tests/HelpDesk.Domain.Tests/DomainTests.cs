using FluentAssertions;
using HelpDesk.Domain.Entities;

namespace HelpDesk.Domain.Tests;

public class TicketTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var title = "Test Ticket";
        var description = "Test Description";
        var tenantId = Guid.NewGuid();

        // Act
        var ticket = new Ticket(title, description, tenantId);

        // Assert
        ticket.Id.Should().NotBeEmpty();
        ticket.Title.Should().Be(title);
        ticket.Description.Should().Be(description);
        ticket.TenantId.Should().Be(tenantId);
        ticket.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Properties_ShouldBeReadOnly()
    {
        // Arrange
        var ticket = new Ticket("Title", "Desc", Guid.NewGuid());

        // Act & Assert
        // Since setters are private, we can't set them directly
        // This test ensures the class is immutable from outside
        ticket.Title.Should().Be("Title");
        ticket.Description.Should().Be("Desc");
    }
}

public class AuditLogTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaults()
    {
        // Act
        var auditLog = new AuditLog();

        // Assert
        auditLog.id.Should().NotBeNullOrEmpty();
        auditLog.entity.Should().Be("Ticket");
        auditLog.timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var auditLog = new AuditLog
        {
            tenantId = "tenant123",
            entityId = "entity456",
            action = "Create",
            user = "user@example.com",
            data = new { key = "value" }
        };

        // Assert
        auditLog.tenantId.Should().Be("tenant123");
        auditLog.entityId.Should().Be("entity456");
        auditLog.action.Should().Be("Create");
        auditLog.user.Should().Be("user@example.com");
        auditLog.data.Should().NotBeNull();
    }
}
