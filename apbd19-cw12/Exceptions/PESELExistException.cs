namespace apbd19_cw12.Exceptions;

public class PESELExistException : Exception
{
    public PESELExistException()
    {
    }

    public PESELExistException(string? message) : base(message)
    {
    }

    public PESELExistException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}