namespace Nest.Shared.Exceptions;

public class QueueProcessingException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public QueueProcessingException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status500InternalServerError;
    }

    public QueueProcessingException() : base("An error occurred while processing the queue message.")
    {
        ErrorMessage = "An error occurred while processing the queue message.";
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}

// Növbədəki mesajların işlənməsi zamanı problem yarandı.