namespace Nest.Shared.Exceptions;

public class DuplicateEntityException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public DuplicateEntityException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status409Conflict;
    }

    public DuplicateEntityException() : base("The entity already exists.")
    {
        StatusCode = StatusCodes.Status409Conflict;
        ErrorMessage = "The entity already exists.";
    }
}

// Eyni unikal xüsusiyyətlərə malik obyekt artıq mövcuddur.