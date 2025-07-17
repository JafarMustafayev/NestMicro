namespace NestStorage.API.Options;

public class VideoProcessingOptions : IFileProcessingOptions
{
    public TimeSpan? MaxDuration { get; set; } // Maksimum icazə verilən uzunluq
    public string? TargetResolution { get; set; } // Məs: "720p", "1080p"

    public string? OutputFormat { get; set; } // Məs: "mp4", "webm", "mov"
    public int Bitrate { get; set; } = 8000; // Kbit/s

    public bool GenerateThumbnail { get; set; } = true;
    public TimeSpan ThumbnailCaptureTime { get; set; } = TimeSpan.FromSeconds(1);

    public bool RemoveAudio { get; set; } = false; // Audio olmadan video lazımdırsa

    public string? OutputFileName { get; set; }
    public int CompressionLevel { get; set; }
}