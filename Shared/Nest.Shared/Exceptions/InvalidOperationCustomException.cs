namespace Nest.Shared.Exceptions;

public class InvalidOperationCustomException : Exception, IBaseException
{
    public int StatusCode { get; }

    public string CustomMessage { get; }

    public InvalidOperationCustomException()
    {
        StatusCode = 400;
        CustomMessage = "Invalid operation";
    }

    public InvalidOperationCustomException(string message) : base(message)
    {
        CustomMessage = message;
        StatusCode = StatusCodes.Status409Conflict;
    }

    public InvalidOperationCustomException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
        CustomMessage = message;
    }
}