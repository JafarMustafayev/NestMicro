namespace Nest.Shared.Exceptions;

public class DuplicateCustomException : Exception, IBaseException
{
    public int StatusCode { get; }

    public string CustomMessage { get; }

    public DuplicateCustomException(string message) : base(message)
    {
        CustomMessage = message;
        StatusCode = StatusCodes.Status409Conflict;
    }
}