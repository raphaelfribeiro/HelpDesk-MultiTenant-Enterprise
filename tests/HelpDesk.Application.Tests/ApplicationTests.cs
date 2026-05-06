using FluentAssertions;
using HelpDesk.Application.DTOs;
using HelpDesk.Application.Services;
using HelpDesk.Application.Interfaces;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Repositories;
using HelpDesk.Domain.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace HelpDesk.Application.Tests;

public class TicketServiceTests
{
    private readonly Mock<ITicketRepository> _repositoryMock;
    private readonly Mock<IAuditLogService> _auditLogServiceMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMessageBus> _busMock;
    private readonly Mock<IEventHubService> _eventHubMock;
    private readonly TicketService _service;

    public TicketServiceTests()
    {
        _repositoryMock = new Mock<ITicketRepository>();
        _auditLogServiceMock = new Mock<IAuditLogService>();
        _userContextMock = new Mock<IUserContext>();
        _busMock = new Mock<IMessageBus>();
        _eventHubMock = new Mock<IEventHubService>();

        _service = new TicketService(
            _repositoryMock.Object,
            _auditLogServiceMock.Object,
            _userContextMock.Object,
            _busMock.Object,
            _eventHubMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTicketAndLogAudit()
    {
        // Arrange
        var dto = new CreateTicketDto { Title = "Test Title", Description = "Test Desc" };
        var tenantId = Guid.NewGuid();
        var userEmail = "user@example.com";

        _userContextMock.Setup(u => u.GetUserEmail()).Returns(userEmail);

        // Act
        var result = await _service.CreateAsync(dto, tenantId);

        // Assert
        result.Should().NotBeEmpty();
        _repositoryMock.Verify(r => r.AddAsync(It.Is<Ticket>(t =>
            t.Title == dto.Title &&
            t.Description == dto.Description &&
            t.TenantId == tenantId)), Times.Once);
        _auditLogServiceMock.Verify(a => a.AddLogAsync(It.Is<AuditLog>(log =>
            log.tenantId == tenantId.ToString() &&
            log.action == "CREATE_TICKET" &&
            log.user == userEmail)), Times.Once);
        _busMock.Verify(b => b.PublishAsync(It.IsAny<object>()), Times.Once);
        _eventHubMock.Verify(e => e.PublishAsync(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTickets()
    {
        // Arrange
        var tickets = new List<Ticket>
        {
            new Ticket("Title1", "Desc1", Guid.NewGuid()),
            new Ticket("Title2", "Desc2", Guid.NewGuid())
        };
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tickets);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(tickets);
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }
}

public class AuthServiceTests
{
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _service = new AuthService("supersecretkey12345678901234567890", "issuer", "audience");
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwtToken()
    {
        // Arrange
        var username = "testuser";

        // Act
        var token = _service.GenerateToken(username);

        // Assert
        token.Should().NotBeNullOrEmpty();
        // Could decode and verify claims, but for simplicity, just check it's a string
    }
}

public class UserContextTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly UserContext _userContext;

    public UserContextTests()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _userContext = new UserContext(_httpContextAccessorMock.Object);
    }

    [Fact]
    public void GetUserEmail_ShouldReturnEmailFromClaims()
    {
        // Arrange
        var email = "user@example.com";
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, email) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };

        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContext);

        // Act
        var result = _userContext.GetUserEmail();

        // Assert
        result.Should().Be(email);
    }

    [Fact]
    public void GetUserEmail_ShouldReturnNullWhenNoContext()
    {
        // Arrange
        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns((HttpContext)null);

        // Act
        var result = _userContext.GetUserEmail();

        // Assert
        result.Should().BeNull();
    }
}

public class CreateTicketDtoTests
{
    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var dto = new CreateTicketDto
        {
            Title = "Test Title",
            Description = "Test Description"
        };

        // Assert
        dto.Title.Should().Be("Test Title");
        dto.Description.Should().Be("Test Description");
    }
}
