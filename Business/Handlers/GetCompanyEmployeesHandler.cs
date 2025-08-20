using Business.Dtos;
using Business.Exceptions;
using Business.Interfaces;
using Business.Requests;
using Business.Responses;

namespace Business.Handlers;

public class GetCompanyEmployeesHandler
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICompanyRepository _companyRepository;

    public GetCompanyEmployeesHandler(
        IEmployeeRepository employeeRepository,
        ICompanyRepository companyRepository)
    {
        _employeeRepository = employeeRepository;
        _companyRepository = companyRepository;
    }

    public async Task<GetCompanyEmployeesResponse> HandleAsync(GetCompanyEmployeesRequest req, CancellationToken ct = default)
    {
        var company = await _companyRepository.GetById(req.CompanyId, ct);
        NotFoundException.ThrowIfNull(company, "company not found");
        var employees = await _employeeRepository.GetCompanyEmployees(company!.Id, ct);
        var dtos = EmployeeDto.FromEmployees(employees);

        return new GetCompanyEmployeesResponse
        {
            Employees = dtos
        };
    }
}