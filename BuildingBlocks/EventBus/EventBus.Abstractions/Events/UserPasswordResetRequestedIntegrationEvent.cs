namespace EventBus.Abstractions.Events;

public class UserPasswordResetRequestedIntegrationEvent : IntegrationEvent
{
    public string ResetUrl { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }

    public UserPasswordResetRequestedIntegrationEvent()
    {
        ResetUrl = string.Empty;
        Email = string.Empty;
        UserName = string.Empty;
    }
}