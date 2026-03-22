using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MotoklubBezbednost.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlite(
            "Data Source=ef-design.db",
            sqlite => sqlite.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
