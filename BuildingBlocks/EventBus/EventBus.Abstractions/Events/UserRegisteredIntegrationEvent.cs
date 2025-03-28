namespace EventBus.Abstractions.Events;

public class UserRegisteredIntegrationEvent : IntegrationEvent
{
    public string Email { get; set; }
    public string ConfirmedUrl { get; set; }
    public string UserName { get; set; }

    public UserRegisteredIntegrationEvent()
    {
        Email = string.Empty;
        ConfirmedUrl = string.Empty;
        UserName = string.Empty;
    }
}