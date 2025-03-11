namespace NestNotification.API.DTOs.EmailTemplates;

public class UpdateTemplateAttributeDto
{
    public string AttributeId { get; set; }
    public string AttributeName { get; set; }
    public string AttributeValue { get; set; }
    public string? Description { get; set; }
    public bool IsRequired { get; set; }

    public UpdateTemplateAttributeDto()
    {
        AttributeId = string.Empty;
        AttributeName = string.Empty;
        AttributeValue = string.Empty;
        Description = string.Empty;
        IsRequired = false;
    }
}