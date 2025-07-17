namespace NestStorage.API.Abstractions.Services;

public interface IFileTypeConfigurationService
{
    public void GetAllowedFileTypesAsync();
    public void GetMaxFileSizeAsync(string contentType);
    public void GetProcessingRulesAsync(string contentType);
    public void GetFiletypeAsync(string contentType);
}