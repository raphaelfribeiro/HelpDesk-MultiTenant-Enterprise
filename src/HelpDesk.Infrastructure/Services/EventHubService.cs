using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using HelpDesk.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace HelpDesk.Infrastructure.Services;

public class EventHubService : IEventHubService
{
    private readonly EventHubProducerClient _producer;
    private readonly string _connectionString;
    private readonly string _hubName;

    public EventHubService(IConfiguration config)
    {
        _connectionString = config["EventHub:ConnectionString"];
        _hubName = config["EventHub:HubName"];

        _producer = new EventHubProducerClient(_connectionString, _hubName);
    }

    public async Task PublishAsync<T>(T data)
    {
        using var batch = await _producer.CreateBatchAsync();

        var json = JsonSerializer.Serialize(data);
        batch.TryAdd(new EventData(Encoding.UTF8.GetBytes(json)));

        await _producer.SendAsync(batch);
    }
}