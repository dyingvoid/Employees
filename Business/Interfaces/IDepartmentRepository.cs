using Data.Entities;

namespace Business.Interfaces;

public interface IDepartmentRepository
{
    Task<Department?> GetById(long id, CancellationToken ct = default);
    Task<Department?> GetByName(int companyId, string name, CancellationToken ct = default);
}