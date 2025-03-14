namespace NestNotification.API.DTOs.EmailTemplates;

public class DeleteTemplateAttributeDto
{
    public string AttributeId { get; set; }
    public string DeletedBy { get; set; }

    public DeleteTemplateAttributeDto()
    {
        AttributeId = string.Empty;
        DeletedBy = string.Empty;
    }
}