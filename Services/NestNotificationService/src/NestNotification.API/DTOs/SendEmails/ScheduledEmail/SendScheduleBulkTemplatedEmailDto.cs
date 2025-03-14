namespace NestNotification.API.DTOs.SendEmails.ScheduledEmail;

public class SendScheduleBulkTemplatedEmailDto
{
    public List<string> Recipients { get; set; }
    public string TemplateId { get; set; }
    public Dictionary<string, string> Placeholders { get; set; }
    public DateTime ScheduledAt { get; set; }
    public ICollection<(byte[] File, string FileName, string FileMimeType)>? Files { get; set; }

    public SendScheduleBulkTemplatedEmailDto()
    {
        Recipients = new List<string>();
        TemplateId = string.Empty;
        Placeholders = new Dictionary<string, string>();
    }
}