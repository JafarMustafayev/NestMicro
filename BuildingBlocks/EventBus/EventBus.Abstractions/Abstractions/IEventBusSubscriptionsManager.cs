namespace EventBus.Abstractions.Abstractions;

public interface IEventBusSubscriptionsManager
{
    bool IsEmpty { get; }
    
    event EventHandler<string> OnEventRemoved;
    
    void AddSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
        
    void RemoveSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
        
    bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;
    
    bool HasSubscriptionsForEvent(string eventName);
    
    Type GetEventTypeByName(string eventName);
    
    void Clear();
    
    IEnumerable<Type> GetHandlersForEvent<T>() where T : IntegrationEvent;
    
    IEnumerable<Type> GetHandlersForEvent(string eventName);
    
    string GetEventKey<T>() where T : IntegrationEvent;
}