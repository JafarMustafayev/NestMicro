namespace Nest.Shared.Exceptions;

public class FileStorageException : Exception, IBaseException
{
    public int StatusCode { get; }
    public string ErrorMessage { get; }

    public FileStorageException(string message) : base(message)
    {
        ErrorMessage = message;
        StatusCode = StatusCodes.Status500InternalServerError;
    }

    public FileStorageException() : base("An error occurred while loading or saving the file.")
    {
        ErrorMessage = "An error occurred while loading or saving the file.";
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}

// Fayl yükləmə və ya yükləmə zamanı xəta baş verdi.