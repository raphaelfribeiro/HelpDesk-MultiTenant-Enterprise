using HelpDesk.Domain.Entities;

namespace HelpDesk.Domain.Repositories;

public interface ITicketRepository
{
    Task AddAsync(Ticket ticket);

    Task<Ticket?> GetByIdAsync(Guid id);

    Task<IEnumerable<Ticket>> GetAllAsync();
}