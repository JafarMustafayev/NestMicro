namespace Nest.Shared.Exceptions;

public class RateLimitExceededException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public RateLimitExceededException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status429TooManyRequests;
    }

    public RateLimitExceededException() : base("Rate limit exceeded. Please try again later.")
    {
        ErrorMessage = "Rate limit exceeded. Please try again later.";
        StatusCode = StatusCodes.Status429TooManyRequests;
    }
}

// Sorğu icazə verilən sürət limitini aşdı.