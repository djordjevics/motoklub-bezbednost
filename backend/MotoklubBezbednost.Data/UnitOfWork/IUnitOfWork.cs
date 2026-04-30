using Microsoft.EntityFrameworkCore.Storage;

namespace MotoklubBezbednost.Data.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}

