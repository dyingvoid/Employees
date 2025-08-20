namespace Data.Entities;

public record Department
{
    public long Id { get; init; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public required int CompanyId { get; set; }
    public List<Employee> Employees { get; set; } = [];
}