namespace NestNotification.API.DTOs.EmailTemplates;

public class GetTemplateAttributeDto
{
    public string AttributeName { get; set; }
    public string AttributeValue { get; set; }
    public string? Description { get; set; }
    public bool IsRequired { get; set; }
    public string TemplateId { get; set; }

    public GetTemplateAttributeDto()
    {
        AttributeName = string.Empty;
        AttributeValue = string.Empty;
        Description = string.Empty;
        IsRequired = false;
        TemplateId = string.Empty;
    }
}