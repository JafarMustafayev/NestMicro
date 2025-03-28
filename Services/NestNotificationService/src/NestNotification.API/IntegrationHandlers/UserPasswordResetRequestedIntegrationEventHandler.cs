namespace NestNotification.API.IntegrationHandlers;

public class UserPasswordResetRequestedIntegrationEventHandler : IIntegrationEventHandler<UserPasswordResetRequestedIntegrationEvent>
{
    private readonly ILogger<UserPasswordResetRequestedIntegrationEventHandler> _logger;
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateRepository _emailTemplateRepository;

    public UserPasswordResetRequestedIntegrationEventHandler(
        ILogger<UserPasswordResetRequestedIntegrationEventHandler> logger,
        IEmailService emailService,
        IEmailTemplateRepository emailTemplateRepository)
    {
        _logger = logger;
        _emailService = emailService;
        _emailTemplateRepository = emailTemplateRepository;
    }

    public async Task Handle(UserPasswordResetRequestedIntegrationEvent @event)
    {
        var template =
            await _emailTemplateRepository.GetByExpressionAsync(x =>
                x.TemplateName == EmailTemplatesNames.ResetPassword);

        if (template == null)
        {
            _logger.LogWarning("Email Template not found {}", EmailTemplatesNames.ResetPassword);
        }

        await _emailService.SendTemplatedEmailAsync(new()
        {
            TemplateId = template?.Id ?? String.Empty,
            Priority = EmailPriority.High,
            ToEmail = @event.Email,
            Placeholders = new()
            {
                { "$ResetUrl", @event.ResetUrl },
                { "$UserName", @event.UserName }
            }
        });
    }
}