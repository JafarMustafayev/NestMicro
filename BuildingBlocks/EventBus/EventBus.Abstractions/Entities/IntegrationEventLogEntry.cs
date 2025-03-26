namespace EventBus.Abstractions.Entities;

public class IntegrationEventLogEntry:BaseEntityId
{
    public string TransactionId { get; private set; }
    public string EventId { get; private set; }
    public string? EventTypeName { get; private set; }
    public IntegrationEventState State { get; set; }
    public int TimesSent { get; set; }
    public DateTime CreatedAt { get; private set; }
    public string Content { get; private set; }
    public DateTime PublishedAt { get; private set; }

    public IntegrationEventLogEntry(IntegrationEvent @event,string transactionId )
    {
        TransactionId = transactionId;
        EventId = @event.Id;
        CreatedAt = @event.CreatedAt;
        EventTypeName = @event.GetType().FullName;
        Content = JsonSerializer.Serialize(@event, @event.GetType());
        State = IntegrationEventState.NotPublished;
        TimesSent = 0;
    }
    
    public IntegrationEvent? DeserializeJsonContent(Type type)
    {
        return JsonSerializer.Deserialize(Content, type) as IntegrationEvent;
    }
}

