namespace NestNotification.API.Abstractions.Services;

public interface IEmailTemplateService
{
    ResponseDto GetEmailTemplates();

    Task<ResponseDto> GetEmailTemplateByIdAsync(string templateId);

    Task<ResponseDto> GetEmailTemplateByNameAsync(string templateName);

    Task<ResponseDto> CreateEmailTemplateAsync(CreateEmailTemplateDto templateDto);

    Task<ResponseDto> UpdateEmailTemplateAsync(UpdateEmailTemplateDto templateDto);

    Task<ResponseDto> DeleteEmailTemplateAsync(DeleteEmailTemplateDto templateDto);
    Task<ResponseDto> CreateTemplateAttributeAsync(CreateTemplateAttributeDto attributeDto);
    ResponseDto GetTemplateAttributes(string templateId);
    Task<ResponseDto> GetTemplateAttributeByIdAsync(string attributeId);
    Task<ResponseDto> UpdateTemplateAttributeAsync(UpdateTemplateAttributeDto attributeDto);
    Task<ResponseDto> DeleteTemplateAttributeAsync(DeleteTemplateAttributeDto attributeDto);
}