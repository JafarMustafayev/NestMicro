namespace NestNotification.API.DTOs.EmailTemplates;

public class DeleteEmailTemplateDto
{
    public string TemplateId { get; set; }
    public string DeletedBy { get; set; }

    public DeleteEmailTemplateDto()
    {
        TemplateId = string.Empty;
        DeletedBy = string.Empty;
    }
}