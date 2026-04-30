using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.Data;
using Xunit;

namespace MotoklubBezbednost.API.Tests.Migrations;

public sealed class MigrationsSmokeTests
{
    [Fact]
    public async Task CanApplyMigrationsAndQueryDatabase()
    {
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection, sqlite =>
                sqlite.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
            .Options;

        await using var context = new ApplicationDbContext(options);
        await context.Database.MigrateAsync();

        var canQuery = await context.Members.AnyAsync();
        Assert.False(canQuery);
    }
}

