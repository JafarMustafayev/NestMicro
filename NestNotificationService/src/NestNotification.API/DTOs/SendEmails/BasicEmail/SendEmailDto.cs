namespace NestNotification.API.DTOs.SendEmails.BasicEmail;

public class SendEmailDto
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
    public ICollection<(byte[] File, string FileName, string FileMimeType)>? Files { get; set; }

    public SendEmailDto()
    {
        To = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
    }
}