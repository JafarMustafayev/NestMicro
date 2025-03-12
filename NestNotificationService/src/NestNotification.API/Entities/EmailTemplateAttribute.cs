namespace NestNotification.API.Entities;

public class EmailTemplateAttribute : BaseEntity
{
    public string TemplateId { get; set; }
    public string AttributeName { get; set; }
    public string AttributeValue { get; set; }
    public string? Description { get; set; }
    public bool IsRequired { get; set; }
    public EmailTemplate? Template { get; set; }

    public EmailTemplateAttribute()
    {
        AttributeName = string.Empty;
        AttributeValue = string.Empty;
        TemplateId = string.Empty;
        Description = string.Empty;
        IsRequired = false;
        Template = null;
    }
}