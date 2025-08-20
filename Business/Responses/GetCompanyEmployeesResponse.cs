using Business.Dtos;

namespace Business.Responses;

public record GetCompanyEmployeesResponse
{
    public required List<EmployeeDto> Employees { get; init; }
}