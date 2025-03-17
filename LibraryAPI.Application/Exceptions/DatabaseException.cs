namespace LibraryAPI.Application.Exceptions;

public class DatabaseException : Exception
{
    public DatabaseException() : base("Database exception")
    {
    }

    public DatabaseException(string message) : base(message)
    {
    }

    public DatabaseException(string message, Exception innerException) : base(message, innerException)
    {
    }
}