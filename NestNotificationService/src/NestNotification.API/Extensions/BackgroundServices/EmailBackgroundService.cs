namespace NestNotification.API.Extensions.BackgroundServices;

public class EmailBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EmailBackgroundService(
        IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            await emailService.ProcessEmailQueueAsync(stoppingToken);
        }
    }
}