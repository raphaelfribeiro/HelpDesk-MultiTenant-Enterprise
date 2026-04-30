using HelpDesk.API.Middleware;
using HelpDesk.Application.Services;
using HelpDesk.Domain.Repositories;
using HelpDesk.Infrastructure.Data;
using HelpDesk.Infrastructure.Queries;
using HelpDesk.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using Microsoft.AspNetCore.Mvc;

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

var jwtSettings = builder.Configuration.GetSection("Jwt");

var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

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

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddSingleton<AuthService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new AuthService(config["Jwt:Key"], config["Jwt:Issuer"], config["Jwt:Audience"]);
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

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