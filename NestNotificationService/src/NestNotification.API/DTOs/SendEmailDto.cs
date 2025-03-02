namespace NestNotification.API.DTOs;

public class SendEmailDto
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; } = true;
    public ICollection<(byte[] File, string FileName, string FileMimeType)>? Files { get; set; }
}