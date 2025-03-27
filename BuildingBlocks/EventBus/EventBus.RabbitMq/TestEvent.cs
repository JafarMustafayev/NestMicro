namespace EventBus.RabbitMq;

public class TestEvent : IntegrationEvent
{
    public string Message { get; set; }
    public string SerialNumber { get; set; }
    public int Version { get; set; }

    public TestEvent()
    {
        Message = "TestEvent";
        SerialNumber = string.Empty;
    }
}