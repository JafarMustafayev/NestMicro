namespace NestNotification.API.DTOs.SendEmails.ScheduledEmail;

public class SendScheduleBulkEmailDto
{
    public List<string> Recipients { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
    public DateTime ScheduledAt { get; set; }
    public ICollection<(byte[] File, string FileName, string FileMimeType)>? Files { get; set; }

    public SendScheduleBulkEmailDto()
    {
        Recipients = new List<string>();
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
    }
}