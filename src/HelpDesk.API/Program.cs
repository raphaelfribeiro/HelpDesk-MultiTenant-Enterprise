using HelpDesk.API.Middleware;
using HelpDesk.Application.Interfaces;
using HelpDesk.Application.Services;
using HelpDesk.Domain.Repositories;
using HelpDesk.Domain.Services;
using HelpDesk.Infrastructure.Data;
using HelpDesk.Infrastructure.Queries;
using HelpDesk.Infrastructure.Repositories;
using HelpDesk.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration
        .GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' was not found.")));

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
        config.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' was not found."));
});

builder.Services.AddScoped<IUserContext, UserContext>();

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

var jwtSettings = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSettings["Key"] ?? throw new InvalidOperationException("Jwt:Key is required.");
var jwtIssuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is required.");
var jwtAudience = jwtSettings["Audience"] ?? throw new InvalidOperationException("Jwt:Audience is required.");
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,

        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddSingleton<AuthService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var jwtKeyConfig = config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is required.");
    var jwtIssuer = config["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is required.");
    var jwtAudience = config["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is required.");

    return new AuthService(jwtKeyConfig, jwtIssuer, jwtAudience);
});

builder.Services.AddSingleton<IAuditLogService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var cosmosConnectionString = config["CosmosDb:ConnectionString"] ?? throw new InvalidOperationException("CosmosDb:ConnectionString is required.");
    var cosmosDatabase = config["CosmosDb:Database"] ?? throw new InvalidOperationException("CosmosDb:Database is required.");
    var cosmosContainer = config["CosmosDb:Container"] ?? throw new InvalidOperationException("CosmosDb:Container is required.");

    return new CosmosDbService(cosmosConnectionString, cosmosDatabase, cosmosContainer);
});

builder.Services.AddSingleton<IMessageBus, ServiceBusService>();

builder.Services.AddSingleton<IEventHubService, EventHubService>();

builder.Services.AddSingleton<IMessageBus>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new ServiceBusService(config);
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<TenantMiddleware>();

app.MapControllers();

app.Run();