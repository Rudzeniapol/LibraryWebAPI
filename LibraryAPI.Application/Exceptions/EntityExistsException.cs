namespace LibraryAPI.Application.Exceptions;

public class EntityExistsException : Exception
{
    public EntityExistsException() : base("User already exists"){ }
    public EntityExistsException(string message) : base(message) { }
    public EntityExistsException(string message, Exception inner) : base(message, inner) { }
}