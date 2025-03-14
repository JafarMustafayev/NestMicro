namespace NestNotification.API.DTOs.SendEmails;

public class GetEmailLogDto
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
    public DateTime SentAt { get; set; }

    public GetEmailLogDto()
    {
        ToEmail = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
    }
}