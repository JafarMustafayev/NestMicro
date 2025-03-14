namespace NestNotification.API.DTOs.SendEmails.BasicEmail;

public class SendEmailDto
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
    public EmailPriority Priority { get; set; }
    public ICollection<(byte[] File, string FileName, string FileMimeType)>? Files { get; set; }

    public SendEmailDto()
    {
        ToEmail = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
        Priority = EmailPriority.Normal;
    }
}