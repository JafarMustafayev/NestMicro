namespace Nest.Shared.Exceptions;

public class NotFoundCustomException : Exception, IBaseException
{
    public int StatusCode { get; }

    public string CustomMessage { get; }

    public NotFoundCustomException(string message, int statusCode = 404) : base(message)
    {
        StatusCode = statusCode;
        CustomMessage = message;
    }
}