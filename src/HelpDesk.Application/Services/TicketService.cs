using HelpDesk.Application.DTOs;
using HelpDesk.Application.Interfaces;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Repositories;
using HelpDesk.Domain.Services;

namespace HelpDesk.Application.Services;

public class TicketService
{
    private readonly ITicketRepository _repository;
    private readonly IAuditLogService _auditLogService;
    private readonly IUserContext _userContext;
    private readonly IMessageBus _bus;

    public TicketService(ITicketRepository repository, IAuditLogService auditLogService, IUserContext userContext, IMessageBus bus)
    {
        _repository = repository;
        _auditLogService = auditLogService;
        _userContext = userContext;
        _bus = bus;
    }

    public async Task<Guid> CreateAsync(CreateTicketDto dto, Guid tenantId)
    {
        var ticket = new Ticket(dto.Title, dto.Description, tenantId);

        await _auditLogService.AddLogAsync(new AuditLog
        {
            EntityId = ticket.Id.ToString(),
            Action = "CREATE_TICKET",
            User = _userContext.GetUserEmail(),
            Data = $"Ticket criado: {ticket.Id}"
        });

        await _bus.PublishAsync(new
        {
            Event = "TicketCreated",
            TicketId = ticket.Id,
            TenantId = tenantId,
            Title = ticket.Title
        });

        await _repository.AddAsync(ticket);

        return ticket.Id;
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
}