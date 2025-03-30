namespace NestNotification.API.IntegrationHandlers;

public class NewUserLoginDetectedIntegrationEventHandler : IIntegrationEventHandler<NewUserLoginDetectedIntegrationEvent>
{
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly ILogger<NewUserLoginDetectedIntegrationEventHandler> _logger;

    public NewUserLoginDetectedIntegrationEventHandler(
        IEmailService emailService,
        IEmailTemplateRepository emailTemplateRepository,
        ILogger<NewUserLoginDetectedIntegrationEventHandler> logger)
    {
        _emailService = emailService;
        _emailTemplateRepository = emailTemplateRepository;
        _logger = logger;
    }

    public async Task Handle(NewUserLoginDetectedIntegrationEvent @event)
    {
        Console.WriteLine("User logged in successfully");

        var template =
            await _emailTemplateRepository.GetByExpressionAsync(x =>
                x.TemplateName == EmailTemplatesNames.NewLogin);

        if (template == null)
        {
            _logger.LogWarning("Email Template not found {}", EmailTemplatesNames.NewLogin);
        }

        await _emailService.SendTemplatedEmailAsync(new()
        {
            TemplateId = template?.Id ?? String.Empty,
            Priority = EmailPriority.High,
            ToEmail = @event.Email,
            Placeholders = new()
            {
                { "$UserName", @event.UserName },
                { "$Ip_Address", @event.IpAddress },
                { "$Location", (@event.City + ", " + @event.Country) },
                { "$Utc_Time", @event.UtcTime },
                { "$Local_Time", @event.LocalTime },
                { "$Time_Zone", @event.TimeZone },
                { "$Device_Name", @event.DeviceName },
                { "$Device_Type", @event.DeviceType },
                { "$Operating_System", @event.OperatingSystem },
                { "$next_step_link", @event.ManageAccountUrl }
            }
        });
    }
}