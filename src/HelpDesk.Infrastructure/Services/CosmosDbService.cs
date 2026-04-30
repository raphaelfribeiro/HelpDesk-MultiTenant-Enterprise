using Microsoft.Azure.Cosmos;
using HelpDesk.Domain.Services;
using System.Text.Json;

namespace HelpDesk.Infrastructure.Services;

public class CosmosDbService : IAuditLogService
{
    private readonly Container _container;

    public CosmosDbService(string connectionString, string databaseName, string containerName)
    {
        var client = new CosmosClient(connectionString);
        var database = client.CreateDatabaseIfNotExistsAsync(databaseName).GetAwaiter().GetResult();

        _container = database.Database.CreateContainerIfNotExistsAsync(containerName, "/id").GetAwaiter().GetResult().Container;
    }

    public async Task AddLogAsync(AuditLog log)
    {
        if (string.IsNullOrWhiteSpace(log.id))
        {
            log.id = Guid.NewGuid().ToString();
        }

        var json = JsonSerializer.Serialize(log);
        Console.WriteLine(json);

        await _container.CreateItemAsync(log, new PartitionKey(log.id));
    }
}