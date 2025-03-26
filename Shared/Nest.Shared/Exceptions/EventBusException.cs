namespace Nest.Shared.Exceptions;

public class EventBusException : Exception, IBaseException
{
    public EventBusException()
    {
    }

    public EventBusException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status500InternalServerError;
    }

    public EventBusException(string message, Exception innerException) : base(message, innerException)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status500InternalServerError;
    }

    public string ErrorMessage { get; }
    public int StatusCode { get; }
}