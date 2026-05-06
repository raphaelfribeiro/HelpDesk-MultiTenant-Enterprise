using FluentAssertions;
using HelpDesk.Admin.Web.Controllers;
using HelpDesk.Admin.Web.Services;
using HelpDesk.Admin.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace HelpDesk.Admin.Web.Tests;

public class TicketsControllerTests
{
    private readonly Mock<TicketApiService> _ticketApiServiceMock;
    private readonly TicketsController _controller;

    public TicketsControllerTests()
    {
        _ticketApiServiceMock = new Mock<TicketApiService>();
        _controller = new TicketsController(_ticketApiServiceMock.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewWithTickets()
    {
        // Arrange
        var tickets = new List<TicketViewModel>
        {
            new TicketViewModel { Id = Guid.NewGuid(), Title = "Test Ticket" }
        };
        _ticketApiServiceMock.Setup(s => s.GetTicketsAsync()).ReturnsAsync(tickets);

        // Act
        var result = await _controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        viewResult!.Model.Should().BeEquivalentTo(tickets);
    }
}

public class AccountControllerTests
{
    private readonly Mock<AuthApiService> _authApiServiceMock;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        _authApiServiceMock = new Mock<AuthApiService>();
        _controller = new AccountController(_authApiServiceMock.Object);
    }

    [Fact]
    public void Login_Get_ReturnsView()
    {
        // Act
        var result = _controller.Login();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async Task Login_Post_SignsInAndRedirects()
    {
        // Arrange
        var email = "test@example.com";
        var token = "jwt.token";
        _authApiServiceMock.Setup(a => a.LoginAsync(email)).ReturnsAsync(token);

        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

        // Act
        var result = await _controller.Login(email);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        var redirectResult = result as RedirectToActionResult;
        redirectResult!.ActionName.Should().Be("Index");
        redirectResult.ControllerName.Should().Be("Tickets");
    }
}

public class HomeControllerTests
{
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        _controller = new HomeController();
    }

    [Fact]
    public void Index_ReturnsView()
    {
        // Act
        var result = _controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void Privacy_ReturnsView()
    {
        // Act
        var result = _controller.Privacy();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void Error_ReturnsViewWithModel()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

        // Act
        var result = _controller.Error();

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        viewResult!.Model.Should().BeOfType<ErrorViewModel>();
    }
}
