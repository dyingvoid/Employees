using Business.Dtos;
using Business.Exceptions;
using Business.Interfaces;
using Business.Requests;

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

    public async Task<EmployeesCollection> HandleAsync(GetCompanyEmployeesRequest req, CancellationToken ct = default)
    {
        var company = await _companyRepository.GetById(req.CompanyId, ct);
        NotFoundException.ThrowIfNull(company, "company not found");
        var employees = await _employeeRepository.GetCompanyEmployees(company!.Id, ct);
        
        return EmployeesCollection.FromEmployees(employees);
    }
}