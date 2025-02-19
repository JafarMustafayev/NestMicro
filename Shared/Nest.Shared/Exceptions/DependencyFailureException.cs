namespace Nest.Shared.Exceptions;

public class DependencyFailureException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public DependencyFailureException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status503ServiceUnavailable;
    }

    public DependencyFailureException() : base("A dependent service or resource is unavailable.")
    {
        ErrorMessage = "A dependent service or resource is unavailable.";
        StatusCode = StatusCodes.Status503ServiceUnavailable;
    }
}

// Asılı olan xidmət və ya resurs cavab vermədi və ya əlçatmazdır.