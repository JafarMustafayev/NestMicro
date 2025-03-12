namespace NestNotification.API.DTOs.SendEmails.BasicEmail;

public class SendTemplatedEmailDto
{
    public string ToEmail { get; set; }
    public string TemplateId { get; set; }
    public Dictionary<string, string> Placeholders { get; set; }
    public EmailPriority Priority { get; set; }
    public ICollection<(byte[] File, string FileName, string FileMimeType)>? Files { get; set; }

    public SendTemplatedEmailDto()
    {
        ToEmail = string.Empty;
        TemplateId = string.Empty;
        Placeholders = new Dictionary<string, string>();
        Priority = EmailPriority.Normal;
    }
}