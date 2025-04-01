namespace NestNotification.API.IntegrationHandlers;

/// <inheritdoc />
public class User2FaOtpSentIntegrationEventHandler : IIntegrationEventHandler<User2FaOtpSentIntegrationEvent>
{
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly ILogger<User2FaOtpSentIntegrationEventHandler> _logger;

    public User2FaOtpSentIntegrationEventHandler(
        IEmailService emailService,
        IEmailTemplateRepository emailTemplateRepository,
        ILogger<User2FaOtpSentIntegrationEventHandler> logger)
    {
        _emailService = emailService;
        _emailTemplateRepository = emailTemplateRepository;
        _logger = logger;
    }

    public async Task Handle(User2FaOtpSentIntegrationEvent @event)
    {
        var template =
            await _emailTemplateRepository.GetByExpressionAsync(x =>
                x.TemplateName == EmailTemplatesNames.SentOtp);

        if (template == null)
        {
            _logger.LogWarning("Email Template not found {}", EmailTemplatesNames.SentOtp);
        }

        await _emailService.SendTemplatedEmailAsync(new()
        {
            TemplateId = template?.Id ?? String.Empty,
            Priority = EmailPriority.High,
            ToEmail = @event.Email,
            Placeholders = new()
            {
                { "$UserName", @event.UserName },
                { "$OtpCode", @event.Otp }
            }
        });
    }
}