namespace NestNotification.API.DTOs.EmailTemplates;

public class CreateEmailTemplateDto
{
    public string TemplateName { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
    public string CreatedBy { get; set; }

    public CreateEmailTemplateDto()
    {
        TemplateName = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
        CreatedBy = string.Empty;
    }
}