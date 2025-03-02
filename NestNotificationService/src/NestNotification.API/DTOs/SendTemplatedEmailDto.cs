namespace NestNotification.API.DTOs;

public class SendTemplatedEmailDto
{
    public string To { get; set; }
    public string TemplateId { get; set; }
    public Dictionary<string, string> Placeholders { get; set; }
    public ICollection<(byte[] File, string FileName, string FileMimeType)>? Files { get; set; }
}