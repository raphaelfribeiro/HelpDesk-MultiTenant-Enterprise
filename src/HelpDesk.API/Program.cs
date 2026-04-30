using HelpDesk.API.Middleware;
using HelpDesk.Application.Services;
using HelpDesk.Domain.Repositories;
using HelpDesk.Infrastructure.Data;
using HelpDesk.Infrastructure.Queries;
using HelpDesk.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services
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
    var connectionString = config.GetConnectionString("Default")
        ?? throw new InvalidOperationException("Connection string 'Default' was not found.");

    return new TicketQuery(connectionString);
});

builder.Services.AddScoped<TicketAdoRepository>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new TicketAdoRepository(
        config.GetConnectionString("Default"));
});

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var tenantSecurityScheme = new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-Tenant-Id",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "apiKey",
        Description = "Tenant Id header"
    };

    options.AddSecurityDefinition("TenantId", tenantSecurityScheme);
    options.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference("TenantId", null, null), new List<string>() }
    });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<TenantMiddleware>();

app.MapControllers();

app.Run();