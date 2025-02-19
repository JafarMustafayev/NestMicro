namespace Nest.Shared.Exceptions;

public class ConflictException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public ConflictException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status409Conflict;
    }

    public ConflictException() : base("A conflict occurred with the current state of the resource.")
    {
        StatusCode = StatusCodes.Status409Conflict;
        ErrorMessage = "A conflict occurred with the current state of the resource.";
    }
}

// İki əməliyyat eyni məlumatı təsir etdiyi üçün konflikt yarandı.