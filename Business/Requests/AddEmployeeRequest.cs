using Business.Dtos;

namespace Business.Requests;

public record AddEmployeeRequest
{
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required string Phone { get; init; }
    public required int CompanyId { get; init; }
    public required PassportDto PassportDto { get; init; }
    public required string DepartmentName { get; init; }
}