namespace Business.Requests;

public record DeleteEmployeeRequest
{
    public required int Id { get; init; }
}