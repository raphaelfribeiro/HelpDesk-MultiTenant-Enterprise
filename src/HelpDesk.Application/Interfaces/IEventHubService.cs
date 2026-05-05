namespace HelpDesk.Application.Interfaces;

public interface IEventHubService
{
    Task PublishAsync<T>(T data);
}