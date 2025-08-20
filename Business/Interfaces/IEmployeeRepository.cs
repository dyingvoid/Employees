using Data.Entities;

namespace Business.Interfaces;

public interface IEmployeeRepository
{
    Task Update(Employee employee, CancellationToken ct = default);
    Task<Employee?> GetByIdWithDepartmentAsync(int id, CancellationToken ct = default);
    Task<List<Employee>> GetDepartmentEmployees(long departmentId, CancellationToken ct = default);
    Task<List<Employee>> GetCompanyEmployees(int companyId, CancellationToken ct = default);
    Task DeleteByIdAsync(int id, CancellationToken ct = default);
    Task<Employee?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> Save(Employee employee, CancellationToken ct = default);
}