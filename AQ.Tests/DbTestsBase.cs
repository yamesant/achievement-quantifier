using System.Data.Common;
using AQ.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AQ.Tests;

public class DbTestsBase : CommandTestsBase
{
    private readonly DbContextOptions<DataContext> _contextOptions;
    protected DataContext CreateDataContext() => new(_contextOptions);

    protected DbTestsBase()
    {
        DbConnection connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        _contextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite(connection)
            .Options;

        using DataContext context = new(_contextOptions);
        context.Database.EnsureCreated();
    }
}