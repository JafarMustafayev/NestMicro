namespace EventBus.RabbitMq;

public class TestEvent : IntegrationEvent
{
    public string Message { get; set; } =" test event";
}