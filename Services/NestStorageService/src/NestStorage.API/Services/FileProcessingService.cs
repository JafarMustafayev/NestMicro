using System.Text;

namespace NestStorage.API.Services;

public class FileProcessingService : IFileProcessingService
{
    public bool ProcessImageAsync(Stream imageStream, IFileProcessingOptions options)
    {
        return true;
    }

    public string RenameFile(string oldFileName, string extension)
    {
        return $"{oldFileName}--{Guid.NewGuid()}{extension}";
    }
}