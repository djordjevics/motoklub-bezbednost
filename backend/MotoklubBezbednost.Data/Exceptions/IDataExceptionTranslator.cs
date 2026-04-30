using Microsoft.EntityFrameworkCore;

namespace MotoklubBezbednost.Data.Exceptions;

public interface IDataExceptionTranslator
{
    Exception Translate(DbUpdateException exception);
}

