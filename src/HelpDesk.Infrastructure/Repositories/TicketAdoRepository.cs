using System.Data;
using Microsoft.Data.SqlClient;

namespace HelpDesk.Infrastructure.Repositories;

public class TicketAdoRepository
{
    private readonly string _connectionString;

    public TicketAdoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<dynamic>> GetByTenantAsync(Guid tenantId)
    {
        var result = new List<dynamic>();

        using var connection = new SqlConnection(_connectionString);

        using var command = new SqlCommand("GetTicketsByTenant", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@TenantId", tenantId);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new
            {
                Id = reader["Id"],
                Title = reader["Title"],
                Description = reader["Description"]
            });
        }

        return result;
    }
}