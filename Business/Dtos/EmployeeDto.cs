using Data.Entities;

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
    
    public static List<EmployeeDto> FromEmployees(List<Employee> employees)
    {
        var dtos = new List<EmployeeDto>(employees.Count);
        foreach (var e in employees)
        {
            var passports = e.Passports
                .Select(p => new PassportDto { Type = p.Type, Number = p.Number });
            var department = new DepartmentDto
            {
                Name = e.Department?.Name ?? "",
                Phone = e.Department?.Phone ?? "",
            };
            var dto = new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                Surname = e.Surname,
                Phone = e.Phone,
                CompanyId = e.Department?.CompanyId ?? -1,
                Passports = passports.ToList(),
                Department = department,
            };
            dtos.Add(dto);
        }

        return dtos;
    }
}