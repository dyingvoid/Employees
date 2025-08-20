namespace Data.Entities;

public record Passport
{
    public required string Number { get; set; }
    public required string Type { get; set; }
    public int EmployeeId { get; set; }
}