namespace apbd19_cw12.Exceptions;

public class AssignedTripsException : Exception
{
    public AssignedTripsException()
    {
    }

    public AssignedTripsException(string? message) : base(message)
    {
    }

    public AssignedTripsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}