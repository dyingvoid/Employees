using Business.Exceptions;
using Business.Interfaces;
using Business.Requests;

namespace Business.Handlers;

public class DeleteEmployeeHandler
{
    private readonly IEmployeeRepository _employeeRepository;

    public DeleteEmployeeHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task HandleAsync(DeleteEmployeeRequest req, CancellationToken ct = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(req.Id, ct);
        NotFoundException.ThrowIfNull(employee, "Employee not found");
        await _employeeRepository.DeleteByIdAsync(req.Id, ct);
    }
}