using Business.Dtos;
using Business.Exceptions;
using Business.Interfaces;
using Business.Requests;

namespace Business.Handlers;

public class GetDepartmentEmployeesHandler
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public GetDepartmentEmployeesHandler(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<EmployeesCollection> HandleAsync(GetDepartmentEmployeesRequest req, CancellationToken ct = default)
    {
        var department = await _departmentRepository.GetById(req.DepartmentId, ct);
        NotFoundException.ThrowIfNull(department, "department not found");

        var employees = await _employeeRepository.GetDepartmentEmployees(department!.Id, ct);
        return EmployeesCollection.FromEmployees(employees);
    }
}