namespace NestNotification.API.DTOs.EmailTemplates;

public class CreateTemplateAttributeDto
{
    public string? TemplateId { get; set; }
    public string AttributeName { get; set; }
    public string AttributeValue { get; set; }
    public string? Description { get; set; }
    public bool IsRequired { get; set; }

    public CreateTemplateAttributeDto()
    {
        TemplateId = string.Empty;
        AttributeName = string.Empty;
        AttributeValue = string.Empty;
        Description = string.Empty;
        IsRequired = false;
    }
}