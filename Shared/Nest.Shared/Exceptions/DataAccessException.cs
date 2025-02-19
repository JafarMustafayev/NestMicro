namespace Nest.Shared.Exceptions;

public class DataAccessException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public DataAccessException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status500InternalServerError;
    }

    public DataAccessException() : base("An error occurred while accessing the database.")
    {
        StatusCode = StatusCodes.Status500InternalServerError;
        ErrorMessage = "An error occurred while accessing the database.";
    }
}

//  Verilənlər bazasına çıxış zamanı xəta baş verdi