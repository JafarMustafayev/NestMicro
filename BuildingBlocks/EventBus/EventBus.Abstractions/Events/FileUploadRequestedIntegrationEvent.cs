namespace EventBus.Abstractions.Events;

public class FileUploadRequestedIntegrationEvent : IntegrationEvent
{
    public string FileName { get; set; }
    public string OutputFileName { get; set; }
    public object OptinosOfProcessing { get; set; }
    public string FileType { get; set; }
    public string BucketName { get; set; }
}