using Business.Exceptions;
using Business.Interfaces;
using Business.Requests;
using Data.Entities;

namespace Business.Handlers;

public class UpdateEmployeeHandler
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public UpdateEmployeeHandler(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task HandleAsync(UpdateEmployeeRequest req, CancellationToken ct = default)
    {
        var employee = await _employeeRepository.GetByIdWithDepartmentAsync(req.EmployeeId, ct);
        NotFoundException.ThrowIfNull(employee, "employee not found");
        Department? department = null;
        if (req.DepartmentName is not null && req.CompanyId is not null)
        {
            department = await _departmentRepository.GetByName(
                (int)req.CompanyId, req.DepartmentName, ct
            );
            NotFoundException.ThrowIfNull(department, "department not found");
        }

        var passports = req.Passports?.Select(dto => new Passport
        {
            Number = dto.Number,
            Type = dto.Type,
        }).ToList();

        employee!.Name = req.Name ?? employee.Name;
        employee.Surname = req.Surname ?? employee.Surname;
        employee.Phone = req.Phone ?? employee.Phone;
        employee.DepartmentId = department?.Id ?? employee.DepartmentId;
        employee.Department = department ?? employee.Department;
        employee.Passports = passports ??  employee.Passports;
        
        await _employeeRepository.Update(employee, ct);
    }
}