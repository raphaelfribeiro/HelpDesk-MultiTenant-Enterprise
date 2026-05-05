using HelpDesk.AuditWorker.Models;
using System.Text.Json;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;
using System.Text;

namespace HelpDesk.AuditWorker;

public class Worker : BackgroundService
{
    private readonly IConfiguration _config;
    private EventProcessorClient _processor;
    private Container _container;

    public Worker(IConfiguration config)
    {
        _config = config;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var blobClient = new BlobContainerClient(_config["BlobStorage:ConnectionString"], _config["BlobStorage:Container"]);

        await blobClient.CreateIfNotExistsAsync();

        _processor = new EventProcessorClient(
            blobClient,
            "$Default",
            _config["EventHub:ConnectionString"],
            _config["EventHub:HubName"]);

        var cosmosClient = new CosmosClient(
            _config["CosmosDb:ConnectionString"]);

        var db = await cosmosClient
            .CreateDatabaseIfNotExistsAsync(
                _config["CosmosDb:Database"]);

        _container = await db.Database
            .CreateContainerIfNotExistsAsync(
                _config["CosmosDb:Container"],
                "/tenantId");

        _processor.ProcessEventAsync += ProcessEvent;
        _processor.ProcessErrorAsync += ProcessError;

        await _processor.StartProcessingAsync();
    }

    private async Task ProcessEvent(ProcessEventArgs args)
    {
        var data = Encoding.UTF8.GetString(args.Data.Body.ToArray());
        var auditEvent = JsonSerializer.Deserialize<AuditEvent>(data);

        Console.WriteLine($"Evento recebido: {data}");

        var log = new
        {
            id = Guid.NewGuid().ToString(),
            eventType = auditEvent.Event,
            ticketId = auditEvent.TicketId,
            tenantId = auditEvent.TenantId,
            timestamp = auditEvent.Timestamp,
            createdAt = DateTime.UtcNow
        };

        await _container.CreateItemAsync(log, new PartitionKey(log.tenantId));

        await args.UpdateCheckpointAsync(args.CancellationToken);
    }

    private Task ProcessError(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.Message);
        return Task.CompletedTask;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}