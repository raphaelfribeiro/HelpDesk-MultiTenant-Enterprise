using FluentAssertions;
using HelpDesk.Domain.Entities;
using HelpDesk.Infrastructure.Data;
using HelpDesk.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Infrastructure.Tests;

public class TicketRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly TicketRepository _repository;

    public TicketRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _repository = new TicketRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTicket()
    {
        // Arrange
        var ticket = new Ticket("Title", "Desc", Guid.NewGuid());

        // Act
        await _repository.AddAsync(ticket);

        // Assert
        var result = await _repository.GetByIdAsync(ticket.Id);
        result.Should().NotBeNull();
        result!.Title.Should().Be("Title");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTicket()
    {
        // Arrange
        var ticket = new Ticket("Title", "Desc", Guid.NewGuid());
        await _repository.AddAsync(ticket);

        // Act
        var result = await _repository.GetByIdAsync(ticket.Id);

        // Assert
        result.Should().BeEquivalentTo(ticket);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTickets()
    {
        // Arrange
        var ticket1 = new Ticket("Title1", "Desc1", Guid.NewGuid());
        var ticket2 = new Ticket("Title2", "Desc2", Guid.NewGuid());
        await _repository.AddAsync(ticket1);
        await _repository.AddAsync(ticket2);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }
}

public class AppDbContextTests
{
    [Fact]
    public void OnModelCreating_ShouldConfigureTicket()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);

        // Act
        // The configuration is done in OnModelCreating, which is called when accessing model

        // Assert
        var entity = context.Model.FindEntityType(typeof(Ticket));
        entity.Should().NotBeNull();
        var titleProperty = entity!.FindProperty("Title");
        titleProperty.Should().NotBeNull();
        titleProperty!.IsNullable.Should().BeFalse();
        titleProperty.GetMaxLength().Should().Be(200);
    }
}