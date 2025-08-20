namespace Business.Dtos;

public record DepartmentDto
{
    public required string Name { get; init; }
    public required string Phone { get; init; }
}