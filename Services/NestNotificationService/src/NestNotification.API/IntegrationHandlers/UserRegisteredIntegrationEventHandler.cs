namespace NestNotification.API.IntegrationHandlers;

public class UserRegisteredIntegrationEventHandler : IIntegrationEventHandler<UserRegisteredEvent>
{
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly ILogger<UserRegisteredIntegrationEventHandler> _logger;

    public UserRegisteredIntegrationEventHandler(
        IEmailService emailService, IEmailTemplateRepository emailTemplateRepository,
        ILogger<UserRegisteredIntegrationEventHandler> logger)
    {
        _emailService = emailService;
        _emailTemplateRepository = emailTemplateRepository;
        _logger = logger;
    }

    public async Task Handle(UserRegisteredEvent @event)
    {
        var template =
            await _emailTemplateRepository.GetByExpressionAsync(x =>
                x.TemplateName == EmailTemplatesNames.EmailConfirmation);

        if (template == null)
        {
            _logger.LogWarning("Email Template not found {}", EmailTemplatesNames.EmailConfirmation);
        }

        await _emailService.SendTemplatedEmailAsync(new()
        {
            TemplateId = template?.Id ?? String.Empty,
            Priority = EmailPriority.High,
            ToEmail = @event.Email,
            Placeholders = new()
            {
                { "{$ConfirmedUrl}", @event.ConfirmedUrl }
            }
        });
    }
}