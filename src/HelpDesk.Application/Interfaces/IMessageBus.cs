namespace HelpDesk.Application.Interfaces;

public interface IMessageBus
{
    Task PublishAsync<T>(T message);
}