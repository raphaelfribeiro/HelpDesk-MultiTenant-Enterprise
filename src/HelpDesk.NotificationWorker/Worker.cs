using Azure.Messaging.ServiceBus;

namespace HelpDesk.NotificationWorker;

public class Worker : BackgroundService
{
    private readonly IConfiguration _config;
    private ServiceBusProcessor _processor;

    public Worker(IConfiguration config)
    {
        _config = config;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionString = _config["ServiceBus:ConnectionString"];
        var queueName = _config["ServiceBus:QueueName"];

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("ServiceBus:ConnectionString não configurada.");

        if (string.IsNullOrWhiteSpace(queueName))
            throw new InvalidOperationException("ServiceBus:QueueName não configurada.");

        var client = new ServiceBusClient(connectionString);

        _processor = client.CreateProcessor(queueName);

        _processor.ProcessMessageAsync += ProcessMessage;
        _processor.ProcessErrorAsync += ProcessError;

        await _processor.StartProcessingAsync(cancellationToken);
    }

    private async Task ProcessMessage(ProcessMessageEventArgs args)
    {
        var body = args.Message.Body.ToString();

        Console.WriteLine($"Mensagem recebida: {body}");

        // Aqui pode ser implementado:
        // - Enviar email
        // - Notificar usuário
        // - Logar no sistema

        await args.CompleteMessageAsync(args.Message);
    }

    private Task ProcessError(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.Message);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}