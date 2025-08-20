namespace Business.Responses;

public record AddEmployeeResponse
{
    public required int EmployeeId { get; init; }
}