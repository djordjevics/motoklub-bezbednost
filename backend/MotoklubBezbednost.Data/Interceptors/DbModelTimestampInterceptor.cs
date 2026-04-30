using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Data.Interceptors;

public sealed class DbModelTimestampInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        Apply(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Apply(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void Apply(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var utc = DateTime.UtcNow;
        foreach (var entry in context.ChangeTracker.Entries<DbModel>())
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity.CreationTimestamp == default)
                {
                    entry.Entity.CreationTimestamp = utc;
                }

                entry.Entity.LastModificationTimestamp = utc;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModificationTimestamp = utc;
            }
        }
    }
}
