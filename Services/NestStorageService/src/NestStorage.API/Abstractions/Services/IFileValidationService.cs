namespace NestStorage.API.Abstractions.Services;

public interface IFileValidationService
{
    public Task<bool> ValidateFileAsync(IFormFile file);

    public Task<bool> ScanFileForMalwareAsync(Stream fileStream);
}