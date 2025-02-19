namespace Nest.Shared.Exceptions;

public class AuthenticationException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public AuthenticationException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status401Unauthorized;
    }

    public AuthenticationException() : base("Authentication failed. Please check your credentials.")
    {
        ErrorMessage = "Authentication failed. Please check your credentials.";
        StatusCode = StatusCodes.Status401Unauthorized;
    }
}

// İstifadəçi autentifikasiyası uğursuz oldu.