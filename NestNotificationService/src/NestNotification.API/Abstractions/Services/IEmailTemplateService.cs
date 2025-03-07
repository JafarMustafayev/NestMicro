namespace NestNotification.API.Abstractions.Services;

public interface IEmailTemplateService
{
    ResponseDto GetEmailTemplates();

    Task<ResponseDto> GetEmailTemplateByIdAsync(string templateId);

    Task<ResponseDto> GetEmailTemplateByNameAsync(string templateName);

    Task<ResponseDto> AddEmailTemplateAsync(CreateEmailTemplateDto templateDto);

    Task<ResponseDto> UpdateEmailTemplateAsync(UpdateEmailTemplateDto templateDto);

    Task<ResponseDto> DeleteEmailTemplateAsync(DeleteEmailTemplateDto templateDto);
}