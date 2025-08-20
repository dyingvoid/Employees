using Business.Dtos;
using Business.Requests;
using Business.Responses;
using Employees;
using Infrastructure.DAL;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine(opts => { opts.Durability.Mode = DurabilityMode.MediatorOnly; });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

var migrator = new Migrator(connectionString!);
migrator.Migrate();
migrator.Seed();

builder.Services.AddPersistence(connectionString);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("company/{companyId}/employees", (int companyId, IMessageBus bus, CancellationToken ct) =>
    bus.InvokeAsync<EmployeesCollection>(new GetCompanyEmployeesRequest { CompanyId = companyId }, ct)
);
app.MapGet("department/{departmentId}/employees", (long departmentId, IMessageBus bus, CancellationToken ct) =>
    bus.InvokeAsync<EmployeesCollection>(new GetDepartmentEmployeesRequest { DepartmentId = departmentId }, ct)
);
app.MapPost("/employees", (AddEmployeeRequest req, IMessageBus bus, CancellationToken ct) =>
    bus.InvokeAsync<AddEmployeeResponse>(req, ct)
);
app.MapPut("/employees", (UpdateEmployeeRequest req, IMessageBus bus, CancellationToken ct) =>
    bus.InvokeAsync(req, ct)
);
app.MapDelete("/employees/{employeeId}", (int employeeId, IMessageBus bus, CancellationToken ct) =>
    bus.InvokeAsync(new DeleteEmployeeRequest { Id = employeeId }, ct)
);

app.Run();