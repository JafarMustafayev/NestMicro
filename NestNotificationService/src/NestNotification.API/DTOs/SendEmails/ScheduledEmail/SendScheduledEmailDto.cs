﻿namespace NestNotification.API.DTOs.SendEmails.ScheduledEmail;

public class SendScheduledEmailDto
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
    public DateTime ScheduledAt { get; set; }
    public ICollection<(byte[] File, string FileName, string FileMimeType)>? Files { get; set; }

    public SendScheduledEmailDto()
    {
        To = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
    }
}