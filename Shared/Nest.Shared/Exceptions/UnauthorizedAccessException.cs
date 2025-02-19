namespace Nest.Shared.Exceptions;

public class UnauthorizedAccessException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public UnauthorizedAccessException(string message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status401Unauthorized;
    }

    public UnauthorizedAccessException() : base("Access is denied due to invalid credentials.")
    {
        ErrorMessage = "Access is denied due to invalid credentials.";
        StatusCode = StatusCodes.Status401Unauthorized;
    }
}

// İstifadəçinin resursa daxil olmaq üçün icazəsi yoxdur.