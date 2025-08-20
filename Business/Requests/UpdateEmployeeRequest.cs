using Business.Dtos;

namespace Business.Requests;

public class UpdateEmployeeRequest
{
    public required int EmployeeId { get; init; }
    public string? Name { get; init; }
    public string? Surname { get; init; }
    public string? Phone { get; init; }
    public int? CompanyId { get; init; }
    public List<PassportDto>? Passports { get; init; }
    public string? DepartmentName { get; init; }
}