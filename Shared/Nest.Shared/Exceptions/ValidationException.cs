namespace Nest.Shared.Exceptions;

public class ValidationException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public ValidationException(string message, int statusCode = 400) : base(message)
    {
        StatusCode = statusCode;
        ErrorMessage = message;
    }

    public ValidationException() : base("Validation failed for the provided input.")
    {
        StatusCode = StatusCodes.Status400BadRequest;
        ErrorMessage = "Validation failed for the provided input.";
    }
}

// Verilən məlumat validasiya qaydalarına uyğun deyil.