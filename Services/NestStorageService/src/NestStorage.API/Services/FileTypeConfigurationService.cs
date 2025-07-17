namespace NestStorage.API.Services;

public class FileTypeConfigurationService : IFileTypeConfigurationService
{
    private readonly IFileRepository _fileRepository;
    private readonly IMimeTypeRepository _mimeTypeRepository;
    private readonly IMimeCategoryRepository _mimeCategoryRepository;

    public FileTypeConfigurationService(
        IFileRepository fileRepository,
        IMimeCategoryRepository mimeCategoryRepository,
        IMimeTypeRepository mimeTypeRepository)
    {
        _fileRepository = fileRepository;
        _mimeCategoryRepository = mimeCategoryRepository;
        _mimeTypeRepository = mimeTypeRepository;
    }

    public void GetAllowedFileTypesAsync()
    {
    }

    public void GetMaxFileSizeAsync(string contentType)
    {
    }

    public void GetProcessingRulesAsync(string contentType)
    {
    }

    public async void GetFiletypeAsync(string contentType)
    {
    }
}