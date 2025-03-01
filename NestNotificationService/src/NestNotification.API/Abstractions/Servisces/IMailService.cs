namespace NestNotification.API.Abstractions.Servisec;

public interface IMailService
{
    /// <summary>
    /// Verilmiş email ünvanına sadə məzmunlu email göndərir.
    /// Fayl əlavə etmək üçün parametrləri istifadə edir.
    /// </summary>
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = true, byte[] fileData = null, string fileName = null, string fileMimeType = null);

    /// <summary>
    /// Email templatelərindən istifadə edərək dinamik email göndərir.
    /// Fayl əlavə etmək üçün parametrləri istifadə edir.
    /// </summary>
    Task SendTemplatedEmailAsync(string to, string templateId, Dictionary<string, string> placeholders, byte[] fileData = null, string fileName = null, string fileMimeType = null);

    /// <summary>
    /// Bir neçə email ünvanına eyni emaili göndərir.
    /// Fayl əlavə etmək üçün parametrləri istifadə edir.
    /// </summary>
    Task SendBulkEmailAsync(List<string> recipients, string subject, string body, bool isHtml = true, byte[] fileData = null, string fileName = null, string fileMimeType = null);

    /// <summary>
    /// Bir neçə email ünvanına templated email göndərir.
    /// Fayl əlavə etmək üçün parametrləri istifadə edir.
    /// </summary>
    Task SendBulkTemplatedEmailAsync(List<string> recipients, string templateId, Dictionary<string, string> placeholders, byte[] fileData = null, string fileName = null, string fileMimeType = null);

    /// <summary>
    /// Göndərilmiş emaillərin tarixçəsini qaytarır.
    /// </summary>
    Task<List<EmailLog>> GetEmailLogsAsync(int page, int pageSize);

    /// <summary>
    /// Email statusunu yoxlamaq üçün istifadə olunur (məsələn, uğurla göndərilibmi, uğursuz olubmu?).
    /// </summary>
    Task<EmailStatus> GetEmailStatusAsync(string emailId);

    /// <summary>
    /// Eyni emaili təkrar göndərmək üçün istifadə olunur.
    /// </summary>
    Task ResendFailedEmailAsync(string emailId);

    /// <summary>
    /// Email templatelərini DB-dən oxuyur.
    /// </summary>
    Task<List<EmailTemplate>> GetEmailTemplatesAsync();

    /// <summary>
    /// Yeni email template əlavə edir.
    /// </summary>
    Task AddEmailTemplateAsync(EmailTemplate template);

    /// <summary>
    /// Mövcud email template-i yeniləyir.
    /// </summary>
    Task UpdateEmailTemplateAsync(EmailTemplate template);

    /// <summary>
    /// Email template-i silir.
    /// </summary>
    Task DeleteEmailTemplateAsync(string templateId);
}