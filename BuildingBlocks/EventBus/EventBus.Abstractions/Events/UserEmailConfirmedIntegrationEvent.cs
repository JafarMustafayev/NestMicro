namespace EventBus.Abstractions.Events;

public class UserEmailConfirmedIntegrationEvent : IntegrationEvent
{
    public string UserId { get; set; } //{{$userName}}
    public string UserName { get; set; }
    public string Email { get; set; }
    public string ClientUrl { get; set; } //{{next_step_link}}

    public UserEmailConfirmedIntegrationEvent()
    {
        UserId = string.Empty;
        UserName = string.Empty;
        Email = string.Empty;
        ClientUrl = string.Empty;
    }
}