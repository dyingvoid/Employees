using System.Transactions;
using Business.Interfaces;
using Dapper;
using Data.Entities;

namespace Infrastructure.DAL.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public EmployeeRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<List<Employee>> GetDepartmentEmployees(long departmentId, CancellationToken ct = default)
    {
        using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(ct);
        const string sql = @"SELECT 
                         e.*, 
                         d.id as DepartmentId,
                         d.id as Id,
                         d.name as Name, 
                         d.phone as Phone, 
                         d.company_id as CompanyId, 
                         p.number as Number,
                         p.type as Type
                         FROM employee e
                         JOIN department d ON e.department_id = d.id
                         LEFT JOIN passport p ON p.employee_id = e.id
                         WHERE d.id = @DepartmentId";

        var employeeDict = new Dictionary<int, Employee>();
        var employees = await dbConnection.QueryAsync<Employee, Department, Passport, Employee>(
            sql, (employee, department, passport) =>
            {
                if (!employeeDict.TryGetValue(employee.Id, out var currEmployee))
                {
                    currEmployee = employee;
                    currEmployee.Department = department;
                    employeeDict.Add(employee.Id, currEmployee);
                }

                currEmployee.Passports.Add(passport);
                return currEmployee;
            }, new { DepartmentId = departmentId },
            splitOn: "DepartmentId,Number"
        );

        return employeeDict.Values.ToList();
    }

    public async Task<List<Employee>> GetCompanyEmployees(int companyId, CancellationToken ct = default)
    {
        using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(ct);
        const string sql = @"SELECT 
                             e.*, 
                             d.id as DepartmentId,
                             d.id as Id,
                             d.name as Name, 
                             d.phone as Phone, 
                             d.company_id as CompanyId, 
                             p.number as Number,
                             p.type as Type
                             FROM employee e
                             JOIN department d ON e.department_id = d.id
                             LEFT JOIN passport p ON p.employee_id = e.id
                             WHERE d.company_id = @CompanyId";

        var employeeDict = new Dictionary<int, Employee>();
        var employees = await dbConnection.QueryAsync<Employee, Department, Passport, Employee>(
            sql, (employee, department, passport) =>
            {
                if (!employeeDict.TryGetValue(employee.Id, out var currEmployee))
                {
                    currEmployee = employee;
                    currEmployee.Department = department;
                    employeeDict.Add(employee.Id, currEmployee);
                }

                currEmployee.Passports.Add(passport);
                return currEmployee;
            }, new { CompanyId = companyId },
            splitOn: "DepartmentId,Number"
        );

        return employeeDict.Values.ToList();
    }

    public async Task DeleteByIdAsync(int id, CancellationToken ct = default)
    {
        using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(ct);
        const string sql = "DELETE FROM employee WHERE id = @Id";
        await dbConnection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<Employee?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(ct);
        const string sql = "SELECT * FROM employee WHERE id = @Id";
        return await dbConnection.QueryFirstOrDefaultAsync<Employee>(sql, new { Id = id });
    }

    public async Task<Employee?> GetByIdWithDepartmentAsync(int id, CancellationToken ct = default)
    {
        using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(ct);
        const string sql = @"SELECT e.*, d.*
                             FROM employee e 
                             JOIN department d ON e.department_id = d.id
                             WHERE e.id = @Id";
        var result = await dbConnection.QueryAsync<Employee, Department, Employee>(
            sql,
            (employee, department) =>
            {
                employee.DepartmentId = department.Id;
                employee.Department = department;
                return employee;
            }, new { Id = id },
            splitOn: "id");

        return result.FirstOrDefault();
    }

    public async Task<int> Save(Employee employee, CancellationToken ct = default)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(ct);
        int employeeId;

        const string insertEmployee = @"INSERT INTO employee(name, surname, phone, department_id)
                                            VALUES (@Name, @Surname, @Phone, @DepartmentId)
                                            RETURNING id";
        employeeId = await dbConnection.QuerySingleAsync<int>(insertEmployee,
            new
            {
                employee.Name,
                employee.Surname,
                employee.Phone,
                employee.DepartmentId
            });
        if (employee.Passports.Count <= 0) return employeeId;

        const string insertPassport = @"INSERT INTO passport(number, type, employee_id)
                                                VALUES (@Number, @Type, @EmployeeId)";
        await dbConnection.ExecuteAsync(insertPassport,
            employee.Passports.Select(x => new
            {
                x.Number,
                x.Type,
                EmployeeId = employeeId
            }));
        scope.Complete();

        return employeeId;
    }

    public async Task Update(Employee employee, CancellationToken ct = default)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(ct);

        const string updateEmployee = @"UPDATE employee 
                                        SET name = @Name, 
                                        surname = @Surname, 
                                        phone = @Phone, 
                                        department_id = @DepartmentId 
                                        WHERE id = @Id";

        var updatedRows = await dbConnection.ExecuteAsync(updateEmployee,
            new
            {
                employee.Id,
                employee.Name,
                employee.Surname,
                employee.Phone,
                employee.DepartmentId
            }
        );

        const string deletePassports = "DELETE FROM passport WHERE employee_id = @EmployeeId";
        await dbConnection.ExecuteAsync(deletePassports, new { EmployeeId = employee.Id });

        if (employee.Passports?.Count > 0)
        {
            const string insertPassport = @"INSERT INTO passport(number, type, employee_id)
                                        VALUES (@Number, @Type, @EmployeeId)";
            await dbConnection.ExecuteAsync(insertPassport,
                employee.Passports.Select(p => new
                {
                    p.Number,
                    p.Type,
                    EmployeeId = employee.Id
                })
            );
        }

        scope.Complete();
    }
}