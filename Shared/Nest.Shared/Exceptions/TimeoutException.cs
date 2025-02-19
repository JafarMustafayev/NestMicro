namespace Nest.Shared.Exceptions;

public class TimeoutException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public TimeoutException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status408RequestTimeout;
    }

    public TimeoutException() : base("The operation timed out.")
    {
        StatusCode = StatusCodes.Status504GatewayTimeout;
        ErrorMessage = "The operation timed out.";
    }
}

// Əməliyyat gözlənilən zaman çərçivəsində tamamlanmadı.