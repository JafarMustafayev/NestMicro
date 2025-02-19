namespace Nest.Shared.Exceptions;

public class ExternalApiException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public ExternalApiException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status502BadGateway;
    }

    public ExternalApiException() : base("An error occurred while communicating with the external API.")
    {
        StatusCode = StatusCodes.Status502BadGateway;
        ErrorMessage = "An error occurred while communicating with the external API.";
    }
}

// Xarici API çağırışı zamanı xəta baş verdi.