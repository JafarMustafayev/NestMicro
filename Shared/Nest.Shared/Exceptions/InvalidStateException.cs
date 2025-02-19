namespace Nest.Shared.Exceptions;

public class InvalidStateException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public InvalidStateException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status500InternalServerError;
    }

    public InvalidStateException() : base("The entity is in an invalid state for this operation.")
    {
        ErrorMessage = "The entity is in an invalid state for this operation.";
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}

// Əməliyyat üçün obyekt və ya sistem düzgün vəziyyətdə deyil.