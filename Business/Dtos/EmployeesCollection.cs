using Data.Entities;

namespace Business.Dtos;

public record EmployeesCollection
{
    public required List<EmployeeDto> Employees { get; set; }

    public static EmployeesCollection FromEmployees(List<Employee> employees)
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

        return new EmployeesCollection
        {
            Employees = dtos
        };
    }
}