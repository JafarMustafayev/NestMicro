namespace NestStorage.API.Abstractions.Services;

public interface IFileProcessingService
{
    public bool ProcessImageAsync(Stream imageStream, IFileProcessingOptions options);

    // GenerateThumbnailAsync(Stream imageStream, ThumbnailOptions options)
    // CompressFileAsync(Stream fileStream, CompressionOptions options)
    // ExtractFileMetadataAsync(Stream fileStream)
    public string RenameFile(string oldFileName, string extension);
}