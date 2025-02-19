namespace Nest.Shared.Exceptions;

public class ServiceUnavailableException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public ServiceUnavailableException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status503ServiceUnavailable;
    }

    public ServiceUnavailableException() : base("The service is currently unavailable")
    {
        StatusCode = StatusCodes.Status503ServiceUnavailable;
        ErrorMessage = "The service is currently unavailable";
    }
}

// Tələb olunan xidmət müvəqqəti olaraq əlçatmazdır.