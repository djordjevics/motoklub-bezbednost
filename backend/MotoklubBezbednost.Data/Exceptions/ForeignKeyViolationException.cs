namespace MotoklubBezbednost.Data.Exceptions;

public sealed class ForeignKeyViolationException : Exception
{
    public ForeignKeyViolationException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}

