namespace NestStorage.API.Options;

public class DocumentProcessingOptions : IFileProcessingOptions
{
    public bool ConvertToPdf { get; set; } = false; // DOCX -> PDF və s.

    public string? WatermarkText { get; set; }
    public bool ApplyWatermark { get; set; } = false;

    public string? OutputFileName { get; set; }
    public int CompressionLevel { get; set; }
}