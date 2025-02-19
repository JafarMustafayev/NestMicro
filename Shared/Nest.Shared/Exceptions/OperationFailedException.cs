namespace Nest.Shared.Exceptions;

public class OperationFailedException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public OperationFailedException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status500InternalServerError;
    }

    public OperationFailedException() : base("The operation failed to complete.")
    {
        StatusCode = StatusCodes.Status500InternalServerError;
        ErrorMessage = "The operation failed to complete.";
    }
}

// Əməliyyat icrası zamanı ümumi bir xəta baş verdi.