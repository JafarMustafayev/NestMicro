namespace EventBus.Abstractions.Events;

public class IntegrationEvent
{
    public string Id { get; }
    public DateTime CreatedAt { get; }

    public IntegrationEvent()
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.UtcNow;
    }

    public IntegrationEvent(string id, DateTime createdAt)
    {
        Id = id;
        CreatedAt = createdAt;
    }
}