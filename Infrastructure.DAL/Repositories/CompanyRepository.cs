using Business.Interfaces;
using Dapper;
using Data.Entities;

namespace Infrastructure.DAL.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public CompanyRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Company?> GetById(int id, CancellationToken ct = default)
    {
        using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(ct);
        const string sql = "SELECT * FROM company WHERE id = @id";
        return await dbConnection.QueryFirstOrDefaultAsync<Company>(sql, new { id });
    }
}