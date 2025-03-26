namespace EventBus.Abstractions.Abstractions;

public interface IEventLogService
{
    Task<IEnumerable<IntegrationEvent>> RetrieveEventLogsPendingToPublishAsync();
    
    Task SaveEventAsync(IntegrationEvent @event, string? transactionId = null);
    
    Task MarkEventAsPublishedAsync(string eventId);
    
    Task MarkEventAsProcessedAsync(string eventId);
    
    Task MarkEventAsFailedAsync(string eventId);
}