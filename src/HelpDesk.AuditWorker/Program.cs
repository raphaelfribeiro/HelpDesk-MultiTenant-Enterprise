using Azure.Identity;
using HelpDesk.AuditWorker;

var builder = Host.CreateApplicationBuilder(args);

var keyVaultUri = builder.Configuration["KeyVault:Uri"];
if (!string.IsNullOrWhiteSpace(keyVaultUri))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUri),
        new DefaultAzureCredential());
}

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
