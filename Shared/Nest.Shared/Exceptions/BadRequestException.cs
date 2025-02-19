namespace Nest.Shared.Exceptions;

public class BadRequestException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public BadRequestException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status400BadRequest;
    }

    public BadRequestException() : base("The request is invalid or malformed.")
    {
        StatusCode = StatusCodes.Status400BadRequest;
        ErrorMessage = "The request is invalid or malformed.";
    }
}

// Sorğuda səhv parametrlər və ya məlumat var.