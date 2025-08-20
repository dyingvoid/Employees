using Business.Interfaces;
using Dapper;
using Data.Entities;

namespace Infrastructure.DAL.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private IDbConnectionFactory _dbConnectionFactory;

    public DepartmentRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Department?> GetById(long id, CancellationToken ct = default)
    {
        using var conn = await _dbConnectionFactory.CreateConnectionAsync(ct);
        const string sql = @"SELECT id, name, phone, company_id
                             FROM department
                             WHERE id = @id";
        var department = await conn.QuerySingleOrDefaultAsync<Department>(sql, new { id });

        return department;
    }

    public async Task<Department?> GetByName(int companyId, string name, CancellationToken ct = default)
    {
        using var conn = await _dbConnectionFactory.CreateConnectionAsync(ct);
        const string sql = @"SELECT id, name, phone, company_id
                             FROM department
                             WHERE company_id = @companyId AND name = @name";
        var department = await conn.QuerySingleOrDefaultAsync<Department>(sql, new { companyId, name });

        return department;
    }
}