using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

namespace HelpDesk.Infrastructure.Queries;

public class TicketQuery
{
    private readonly string _connectionString;

    public TicketQuery(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<dynamic>> GetDashboardAsync()
    {
        using IDbConnection db =new SqlConnection(_connectionString);

        var sql = @"
            SELECT 
                TenantId,
                COUNT(*) AS TotalTickets
            FROM Tickets
            GROUP BY TenantId";

        return await db.QueryAsync(sql);
    }
}