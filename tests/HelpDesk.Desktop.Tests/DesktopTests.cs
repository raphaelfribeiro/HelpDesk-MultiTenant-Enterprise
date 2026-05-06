using FluentAssertions;
using HelpDesk.Desktop.Models;
using System.Collections.ObjectModel;

namespace HelpDesk.Desktop.Tests;

public class TicketModelTests
{
    [Fact]
    public void TicketModel_Constructor_InitializesProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Test Ticket";
        var status = "Open";

        // Act
        var ticket = new TicketModel
        {
            Id = id,
            Title = title,
            Status = status
        };

        // Assert
        ticket.Id.Should().Be(id);
        ticket.Title.Should().Be(title);
        ticket.Status.Should().Be(status);
    }

    [Fact]
    public void TicketModel_PropertiesCanBeUpdated()
    {
        // Arrange
        var ticket = new TicketModel 
        { 
            Title = "Initial", 
            Status = "Open" 
        };

        // Act
        ticket.Title = "Updated";
        ticket.Status = "Closed";

        // Assert
        ticket.Title.Should().Be("Updated");
        ticket.Status.Should().Be("Closed");
    }

    [Fact]
    public void TicketModel_IdProperty_HasGetterAndSetter()
    {
        // Arrange
        var ticket = new TicketModel();
        var testId = Guid.NewGuid();

        // Act
        ticket.Id = testId;
        var result = ticket.Id;

        // Assert
        result.Should().Be(testId);
    }
}

public class TicketModelCollectionTests
{
    [Fact]
    public void TicketCollection_CanAddMultipleTickets()
    {
        // Arrange
        var tickets = new ObservableCollection<TicketModel>();
        var ticket1 = new TicketModel { Id = Guid.NewGuid(), Title = "Ticket 1", Status = "Open" };
        var ticket2 = new TicketModel { Id = Guid.NewGuid(), Title = "Ticket 2", Status = "Closed" };

        // Act
        tickets.Add(ticket1);
        tickets.Add(ticket2);

        // Assert
        tickets.Should().HaveCount(2);
        tickets.Should().Contain(ticket1);
        tickets.Should().Contain(ticket2);
    }

    [Fact]
    public void TicketCollection_CanRemoveTickets()
    {
        // Arrange
        var tickets = new ObservableCollection<TicketModel>();
        var ticket = new TicketModel { Id = Guid.NewGuid(), Title = "Test", Status = "Open" };
        tickets.Add(ticket);

        // Act
        tickets.Remove(ticket);

        // Assert
        tickets.Should().BeEmpty();
    }

    [Fact]
    public void TicketCollection_CanClearAllTickets()
    {
        // Arrange
        var tickets = new ObservableCollection<TicketModel>
        {
            new TicketModel { Id = Guid.NewGuid(), Title = "Ticket 1", Status = "Open" },
            new TicketModel { Id = Guid.NewGuid(), Title = "Ticket 2", Status = "Closed" }
        };

        // Act
        tickets.Clear();

        // Assert
        tickets.Should().BeEmpty();
    }
}

public class TicketStatusTests
{
    [Theory]
    [InlineData("Open")]
    [InlineData("Closed")]
    [InlineData("In Progress")]
    [InlineData("Resolved")]
    public void TicketModel_StatusProperty_AcceptsValidValues(string status)
    {
        // Arrange
        var ticket = new TicketModel();

        // Act
        ticket.Status = status;

        // Assert
        ticket.Status.Should().Be(status);
    }

    [Fact]
    public void TicketModel_MultipleTicketsWithDifferentStatuses()
    {
        // Arrange
        var openTicket = new TicketModel { Status = "Open" };
        var closedTicket = new TicketModel { Status = "Closed" };
        var inProgressTicket = new TicketModel { Status = "In Progress" };

        // Act & Assert
        openTicket.Status.Should().Be("Open");
        closedTicket.Status.Should().Be("Closed");
        inProgressTicket.Status.Should().Be("In Progress");
    }
}
