using Business.Exceptions;
using Business.Interfaces;
using Business.Requests;
using Business.Responses;
using Data.Entities;
using Microsoft.Extensions.Logging;

namespace Business.Handlers;

public class AddEmployeeHandler
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public AddEmployeeHandler(
        IDepartmentRepository departmentRepository,
        IEmployeeRepository employeeRepository,
        ILogger<AddEmployeeHandler> logger)
    {
        _departmentRepository = departmentRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<AddEmployeeResponse> HandleAsync(AddEmployeeRequest req, CancellationToken ct = default)
    {
        var department = await _departmentRepository
            .GetByName(req.CompanyId, req.DepartmentName, ct);
        if (department is null)
        {
            throw new NotFoundException("department not found");
        }

        var employee = new Employee
        {
            Name = req.Name,
            Surname = req.Surname,
            Phone = req.Phone,
            DepartmentId = department.Id,
            Passports =
            [
                new Passport
                {
                    Number = req.PassportDto.Number,
                    Type = req.PassportDto.Type,
                }
            ]
        };
        var id = await _employeeRepository.Save(employee, ct);

        return new AddEmployeeResponse
        {
            EmployeeId = id,
        };
    }
}