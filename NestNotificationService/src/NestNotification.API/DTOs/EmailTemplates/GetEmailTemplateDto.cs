namespace NestNotification.API.DTOs.EmailTemplates;

public class GetEmailTemplateDto
{
    public string Id { get; set; }
    public string TemplateName { get; set; }
    public string Subject { get; set; }
    public bool IsHtml { get; set; }


    public GetEmailTemplateDto()
    {
        Id = string.Empty;
        TemplateName = string.Empty;
        Subject = string.Empty;
        IsHtml = false;
    }
}