namespace EventBus.Abstractions.Events;

public class User2FaOtpSentIntegrationEvent : IntegrationEvent
{
    public string Email { get; set; }
    public string Otp { get; set; }
    public string UserName { get; set; }

    public User2FaOtpSentIntegrationEvent()
    {
        Email = string.Empty;
        Otp = string.Empty;
        UserName = string.Empty;
    }
}