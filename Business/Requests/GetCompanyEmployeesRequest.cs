namespace Business.Requests;

public record GetCompanyEmployeesRequest
{
    public required int CompanyId { get; init; }
}