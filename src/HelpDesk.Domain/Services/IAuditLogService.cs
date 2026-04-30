using HelpDesk.Domain.Entities;

namespace HelpDesk.Domain.Services;

public interface IAuditLogService
{
    Task AddLogAsync(AuditLog log);
}
