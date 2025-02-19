namespace Nest.Shared.Exceptions;

public class CacheException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public CacheException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status500InternalServerError;
    }

    public CacheException() : base("An error occurred while interacting with the cache.")
    {
        ErrorMessage = "An error occurred while interacting with the cache.";
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}

// Keş sistemi ilə işləyərkən xəta baş verdi.