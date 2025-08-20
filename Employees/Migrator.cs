using System.Reflection;
using DbUp;
using Npgsql;

namespace Employees;

public class Migrator
{
    private readonly string _connectionString;

    public Migrator(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Migrate()
    {
        var upgrader = DeployChanges.To
            .PostgresqlDatabase(_connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .WithTransaction()
            .LogToConsole()
            .Build();

        var res = upgrader.PerformUpgrade();
        if (!res.Successful)
        {
            throw res.Error;
        }
    }

    public void Seed()
    {
        var upgrader = DeployChanges.To
            .PostgresqlDatabase(_connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), script => script.Contains("seed"))
            .WithTransaction()
            .LogToConsole()
            .Build();

        var res = upgrader.PerformUpgrade();
        if (!res.Successful)
        {
            throw res.Error;
        }
    }
}