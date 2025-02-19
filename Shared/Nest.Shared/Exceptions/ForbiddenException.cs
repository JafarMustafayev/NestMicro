namespace Nest.Shared.Exceptions;

public class ForbiddenException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public ForbiddenException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status403Forbidden;
    }

    public ForbiddenException() : base("You do not have permission to access this resource.")
    {
        StatusCode = StatusCodes.Status403Forbidden;
        ErrorMessage = "You do not have permission to access this resource.";
    }
}

// İstifadəçinin bu resursa daxil olmaq icazəsi yoxdur.