namespace Business.Dtos;

public record EmployeeDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required string Phone { get; init; }
    public required int CompanyId { get; init; }
    public required List<PassportDto> Passports { get; init; }
    public required DepartmentDto Department { get; init; }
}