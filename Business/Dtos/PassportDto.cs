namespace Business.Dtos;

public record PassportDto
{
    public required string Type { get; init; }
    public required string Number { get; init; }
}