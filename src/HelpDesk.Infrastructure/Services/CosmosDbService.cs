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

        _container = database.Database.CreateContainerIfNotExistsAsync(containerName, "/tenantId").GetAwaiter().GetResult().Container;
    }

    public async Task AddLogAsync(AuditLog log)
    {
        if (string.IsNullOrWhiteSpace(log.tenantId))
        {
            throw new Exception("tenantId é obrigatório para salvar no Cosmos DB");
        }

        await _container.CreateItemAsync(log, new PartitionKey(log.tenantId));
    }
}