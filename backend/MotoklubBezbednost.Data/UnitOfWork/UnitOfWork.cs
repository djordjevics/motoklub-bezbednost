using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MotoklubBezbednost.Data.Exceptions;

namespace MotoklubBezbednost.Data.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IDataExceptionTranslator _exceptionTranslator;

    public UnitOfWork(ApplicationDbContext context, IDataExceptionTranslator exceptionTranslator)
    {
        _context = context;
        _exceptionTranslator = exceptionTranslator;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw _exceptionTranslator.Translate(ex);
        }
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _context.Database.BeginTransactionAsync(cancellationToken);
    }
}

