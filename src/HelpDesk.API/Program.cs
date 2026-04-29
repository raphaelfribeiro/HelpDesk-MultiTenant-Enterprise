using HelpDesk.Infrastructure.Data;
using HelpDesk.Infrastructure.Repositories;
using HelpDesk.Domain.Repositories;
using HelpDesk.API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration
        .GetConnectionString("Default")));

builder.Services.AddScoped<ITicketRepository, TicketRepository>();

var app = builder.Build();

app.UseMiddleware<TenantMiddleware>();

app.MapControllers();

app.Run();