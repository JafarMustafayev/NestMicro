namespace NestNotification.API.DTOs.SendEmails.ScheduledEmail;

public class SendScheduledTemplateEmailDto
{
    public string ToEmail { get; set; }
    public string TemplateId { get; set; }
    public Dictionary<string, string> Placeholders { get; set; }
    public DateTime ScheduledAt { get; set; }
    public ICollection<(byte[] File, string FileName, string FileMimeType)>? Files { get; set; }

    public SendScheduledTemplateEmailDto()
    {
        ToEmail = string.Empty;
        TemplateId = string.Empty;
        Placeholders = new Dictionary<string, string>();
    }
}