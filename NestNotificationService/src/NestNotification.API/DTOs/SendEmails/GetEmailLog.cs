namespace NestNotification.API.DTOs.SendEmails;

public class GetEmailLog
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
    public DateTime SentAt { get; set; }

    public GetEmailLog()
    {
        ToEmail = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
    }
}