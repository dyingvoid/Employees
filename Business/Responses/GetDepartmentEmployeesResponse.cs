using Business.Dtos;

namespace Business.Responses;

public record GetDepartmentEmployeesResponse
{
    public required List<EmployeeDto> Employees { get; init; }
}