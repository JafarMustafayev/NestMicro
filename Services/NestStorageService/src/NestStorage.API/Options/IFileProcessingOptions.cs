namespace NestStorage.API.Options;

public interface IFileProcessingOptions
{
    string? OutputFileName { get; set; }
    public int CompressionLevel { get; set; } // 1-100 arası sıxılma (quality)
}