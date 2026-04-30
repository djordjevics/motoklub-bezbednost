using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MotoklubBezbednost.Data.Exceptions;

public sealed class SqliteDataExceptionTranslator : IDataExceptionTranslator
{
    public Exception Translate(DbUpdateException exception)
    {
        if (exception.InnerException is SqliteException sqlite)
        {
            // SQLite error code 19 = constraint violation (unique, not null, FK, etc.)
            if (sqlite.SqliteErrorCode == 19)
            {
                var message = sqlite.Message ?? "SQLite constraint violation.";

                // Best-effort classification based on message tokens.
                if (message.Contains("FOREIGN KEY constraint failed", StringComparison.OrdinalIgnoreCase))
                {
                    return new ForeignKeyViolationException(message, exception);
                }

                if (message.Contains("UNIQUE constraint failed", StringComparison.OrdinalIgnoreCase))
                {
                    return new UniqueConstraintViolationException(message, exception);
                }

                return new DataUpdateException(message, exception);
            }
        }

        return new DataUpdateException("Database update failed.", exception);
    }
}

