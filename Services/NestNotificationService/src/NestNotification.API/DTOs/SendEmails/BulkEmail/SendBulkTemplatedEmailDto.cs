namespace NestNotification.API.DTOs.SendEmails.BulkEmail;

public class SendBulkTemplatedEmailDto
{
    public List<string> Recipients { get; set; }
    public string TemplateId { get; set; }
    public EmailPriority Priority { get; set; }
    public Dictionary<string, string> Placeholders { get; set; }
    public ICollection<(byte[] File, string FileName, string FileMimeType)>? Files { get; set; }

    public SendBulkTemplatedEmailDto()
    {
        Recipients = new List<string>();
        TemplateId = string.Empty;
        Placeholders = new Dictionary<string, string>();
    }
}