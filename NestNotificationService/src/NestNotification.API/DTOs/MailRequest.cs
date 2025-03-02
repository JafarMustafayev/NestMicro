namespace NestNotification.API.DTOs;

public class MailRequest
{
    public string To { get; set; }
    public string? Subject { get; set; }
    public string Body { get; set; }
    public ICollection<(byte[] File, string FileName, string FileMimeType)>? Attachments { get; set; }

    public MailRequest()
    {
        To = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        Attachments = null;
    }
}