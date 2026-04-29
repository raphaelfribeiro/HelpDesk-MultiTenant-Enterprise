using HelpDesk.API.Middleware;
using HelpDesk.Application.Services;
using HelpDesk.Domain.Repositories;
using HelpDesk.Infrastructure.Data;
using HelpDesk.Infrastructure.Queries;
using HelpDesk.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration
        .GetConnectionString("Default")));


builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<TicketQuery>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new TicketQuery(
        config.GetConnectionString("Default"));
});

var app = builder.Build();

app.UseMiddleware<TenantMiddleware>();

app.MapControllers();

app.Run();