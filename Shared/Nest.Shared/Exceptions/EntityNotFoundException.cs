namespace Nest.Shared.Exceptions;

public class EntityNotFoundException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public EntityNotFoundException(string message) : base(message)
    {
        StatusCode = StatusCodes.Status404NotFound;
        ErrorMessage = message;
    }

    public EntityNotFoundException() : base("The requested entity could not be found.")
    {
        StatusCode = StatusCodes.Status404NotFound;
        ErrorMessage = "The requested entity could not be found.";
    }
}

// Sorğulanan obyekt bazada tapılmadı.