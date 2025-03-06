namespace NestNotification.API.DTOs.SendEmails.BulkEmail;

public class SendBulkEmailDto
{
    public List<string> Recipients { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
    public ICollection<(byte[] File, string FileName, string FileMimeType)>? Files { get; set; }

    public SendBulkEmailDto()
    {
        Recipients = new List<string>();
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
    }
}