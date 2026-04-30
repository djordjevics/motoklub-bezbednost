namespace MotoklubBezbednost.Data.Exceptions;

public sealed class DataUpdateException : Exception
{
    public DataUpdateException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}

