namespace EventBus.Abstractions.Events;

public class UserRegisteredEvent : IntegrationEvent
{
    public string Email { get; set; }
    public string ConfirmedUrl { get; set; }
    public string UserName { get; set; }

    public UserRegisteredEvent()
    {
        Email = string.Empty;
        ConfirmedUrl = string.Empty;
        UserName = string.Empty;
    }
}