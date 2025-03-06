namespace NestNotification.API.DTOs.EmailTemplates;

public record UpdateEmailTemplateDto
{
    public string Id { get; set; }
    public string TemplateName { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
    public string LastModifiedBy { get; set; }

    public UpdateEmailTemplateDto()
    {
        Id = string.Empty;
        TemplateName = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
        LastModifiedBy = string.Empty;
    }
}