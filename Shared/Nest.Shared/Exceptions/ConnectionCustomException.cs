namespace Nest.Shared.Exceptions;

public class ConnectionCustomException : Exception, IBaseException
{
    public int StatusCode { get; }

    public string CustomMessage { get; }

    public ConnectionCustomException(string message) : base(message)
    {
        StatusCode = 500;
        CustomMessage = message;
    }

    public ConnectionCustomException() : base("Connection string not found")
    {
        StatusCode = 500;
        CustomMessage = "Connection string not found";
    }
}