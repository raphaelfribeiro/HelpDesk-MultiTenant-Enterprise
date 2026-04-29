using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Repositories;
using HelpDesk.Application.DTOs;

namespace HelpDesk.Application.Services;

public class TicketService
{
    private readonly ITicketRepository _repository;

    public TicketService(ITicketRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(CreateTicketDto dto, Guid tenantId)
    {
        var ticket = new Ticket(
            dto.Title,
            dto.Description,
            tenantId);

        await _repository.AddAsync(ticket);

        return ticket.Id;
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
}