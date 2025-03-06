namespace NestNotification.API.DTOs.EmailTemplates;

public class GetEmailTemplateDto
{
    public string TemplateName { get; set; }
    public string Subject { get; set; }
    public bool IsHtml { get; set; }
    public string Body { get; set; }

    public GetEmailTemplateDto()
    {
        TemplateName = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
    }
}