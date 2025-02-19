namespace Nest.Shared.Exceptions;

public class IntegrationException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public IntegrationException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status500InternalServerError;
    }

    public IntegrationException() : base("An error occurred while integrating with another service.")
    {
        ErrorMessage = "An error occurred while integrating with another service.";
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}

// Başqa bir mikroxidmətlə inteqrasiya zamanı problem yarandı.