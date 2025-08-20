namespace Data.Entities;

public record Company
{
    public int Id { get; init; }
    public List<Department> Departments { get; set; } = [];
}