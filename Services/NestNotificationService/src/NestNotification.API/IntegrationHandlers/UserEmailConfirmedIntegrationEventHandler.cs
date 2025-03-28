namespace NestNotification.API.IntegrationHandlers;

public class UserEmailConfirmedIntegrationEventHandler : IIntegrationEventHandler<UserEmailConfirmedIntegrationEvent>
{
    private readonly ILogger<UserEmailConfirmedIntegrationEventHandler> _logger;
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateRepository _emailTemplateRepository;

    public UserEmailConfirmedIntegrationEventHandler(
        IEmailService emailService,
        ILogger<UserEmailConfirmedIntegrationEventHandler> logger,
        IEmailTemplateRepository emailTemplateRepository)
    {
        _emailService = emailService;
        _logger = logger;
        _emailTemplateRepository = emailTemplateRepository;
    }

    public async Task Handle(UserEmailConfirmedIntegrationEvent @event)
    {
        var template =
            await _emailTemplateRepository.GetByExpressionAsync(x =>
                x.TemplateName == EmailTemplatesNames.Welcome);

        if (template == null)
        {
            _logger.LogWarning("Email Template not found {}", EmailTemplatesNames.EmailConfirmation);
        }

        await _emailService.SendTemplatedEmailAsync(new()
        {
            TemplateId = template?.Id ?? String.Empty,
            Priority = EmailPriority.Normal,
            ToEmail = @event.Email,
            Placeholders = new()
            {
                { "$next_step_link", @event.ClientUrl },
                { "$UserName", @event.UserName }
            }
        });
    }
}