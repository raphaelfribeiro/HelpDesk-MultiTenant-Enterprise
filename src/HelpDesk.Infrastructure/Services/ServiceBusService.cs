using Azure.Messaging.ServiceBus;
using HelpDesk.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace HelpDesk.Infrastructure.Services;

public class ServiceBusService : IMessageBus
{
    private readonly ServiceBusSender _sender;
    private readonly string _connectionString;
    private readonly string _queueName;
    private readonly IConfiguration _config;

    public ServiceBusService(IConfiguration config)
    {
        _config = config;
        _connectionString = _config["ServiceBus:ConnectionString"];
        _queueName = _config["ServiceBus:QueueName"];

        var client = new ServiceBusClient(_connectionString);
        _sender = client.CreateSender(_queueName);
    }

    public async Task PublishAsync<T>(T message)
    {
        var json = JsonSerializer.Serialize(message);
        var busMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(json));

        await _sender.SendMessageAsync(busMessage);
    }
}