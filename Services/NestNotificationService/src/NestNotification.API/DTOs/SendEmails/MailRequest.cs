namespace NestNotification.API.DTOs.SendEmails;

public class MailRequest
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public ICollection<(byte[] File, string FileName, string FileMimeType)>? Attachments { get; set; }
    public string? QueueId { get; set; }

    public MailRequest()
    {
        ToEmail = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        Attachments = null;
        QueueId = string.Empty;
    }
}