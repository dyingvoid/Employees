namespace Data.Entities;

public record Employee
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Phone { get; set; }
    public required long DepartmentId { get; set; }
    public Department? Department { get; set; }
    public List<Passport> Passports { get; set; } = [];
}