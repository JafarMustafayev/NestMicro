namespace NestStorage.API.Services;

public class FileValidationService : IFileValidationService
{
    private readonly IMimeTypeRepository _mimeTypeRepository;

    public FileValidationService(
        IMimeTypeRepository mimeTypeRepository)
    {
        _mimeTypeRepository = mimeTypeRepository;
    }

    public async Task<bool> ValidateFileAsync(IFormFile file)
    {
        var allowedFile = await _mimeTypeRepository.GetByExpressionAsync(x => x.Name == file.ContentType && x.IsActive);

        if (allowedFile == null || allowedFile.MaxUploadSizeInBytes > file.Length)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> ScanFileForMalwareAsync(Stream fileStream)
    {
        var environment = Configurations.IsProduction()
            ? Configurations.GetConfiguration<ExternalServices>().ClamAv.Production
            : Configurations.GetConfiguration<ExternalServices>().ClamAv.Development;
        var clam = new ClamClient(environment.BaseUrl)
        {
            Port = environment.Port,
            MaxStreamSize = environment.MaxStreamSize,
            MaxChunkSize = environment.MaxChunkSize
        };

        var scanResult = await clam.SendAndScanFileAsync(fileStream);

        if (scanResult.Result is ClamScanResults.Unknown or ClamScanResults.Error)
        {
            throw new OperationFailedException();
        }

        if (scanResult.Result == ClamScanResults.VirusDetected)
        {
            throw new OperationCanceledException("Virus Detected");
        }

        return true;
    }
}