using Business.Dtos;
using Business.Exceptions;
using Business.Interfaces;
using Business.Requests;
using Business.Responses;

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

    public async Task<GetDepartmentEmployeesResponse> HandleAsync(
        GetDepartmentEmployeesRequest req,
        CancellationToken ct = default)
    {
        var department = await _departmentRepository.GetById(req.DepartmentId, ct);
        NotFoundException.ThrowIfNull(department, "department not found");

        var employees = await _employeeRepository.GetDepartmentEmployees(department!.Id, ct);
        var dtos = EmployeeDto.FromEmployees(employees);

        return new GetDepartmentEmployeesResponse
        {
            Employees = dtos
        };
    }
}