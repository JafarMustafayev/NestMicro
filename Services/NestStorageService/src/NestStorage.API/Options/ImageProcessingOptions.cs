namespace NestStorage.API.Options;

public class ImageProcessingOptions : IFileProcessingOptions
{
    public int? TargetWidth { get; set; } // Ölçüləri dəyişmək üçün
    public int? TargetHeight { get; set; } // Ölçüləri dəyişmək üçün
    public bool MaintainAspectRatio { get; set; } = true;

    public bool ConvertToWebP { get; set; } = false; // Format çevirmə üçün
    public string? OutputFormat { get; set; } // Məsələn: "jpeg", "png", "webp"

    public bool ApplyWatermark { get; set; } = false;
    public string? WatermarkText { get; set; }
    public string? WatermarkImagePath { get; set; } // Əgər şəkil ilə watermark istəyirsənsə
    public string WatermarkPosition { get; set; } = "BottomRight"; // Mümkün: TopLeft, BottomRight, Center...

    public bool GenerateThumbnail { get; set; } = false;
    public int ThumbnailWidth { get; set; } = 150;
    public int ThumbnailHeight { get; set; } = 150;

    public bool NormalizeOrientation { get; set; } = true; // EXIF məlumatlarına əsasən düz bucaq

    public bool StripMetadata { get; set; } = true; // Şəklin içindəki EXIF, GPS və s. silinsin?

    public string? OutputFileName { get; set; } // Əgər faylın adını təyin etmək istəyirsə
    public int CompressionLevel { get; set; }
}