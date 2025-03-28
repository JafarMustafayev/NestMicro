namespace EventBus.RabbitMq.Subscriptions;

public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
{
    private readonly Dictionary<string, List<Type>> _handlers;
    private readonly List<Type> _eventTypes;

    public event EventHandler<string> OnEventRemoved;

    public InMemoryEventBusSubscriptionsManager()
    {
        _handlers = new Dictionary<string, List<Type>>();
        _eventTypes = new List<Type>();
    }

    public bool IsEmpty => !_handlers.Keys.Any();

    public void AddSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();

        AddSubscription(typeof(TH), eventName);

        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }
    }

    private void AddSubscription(Type handlerType, string eventName)
    {
        if (!HasSubscriptionsForEvent(eventName))
        {
            _handlers.Add(eventName, new List<Type>());
        }

        // if (_handlers[eventName].Any(s => s == handlerType))
        // {
        //     throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'",
        //         nameof(handlerType));
        // }

        _handlers[eventName].Add(handlerType);
    }

    public void RemoveSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var handlerToRemove = typeof(TH);
        var eventName = GetEventKey<T>();
        RemoveHandler(eventName, handlerToRemove);
    }

    private void RemoveHandler(string eventName, Type handlerType)
    {
        if (!HasSubscriptionsForEvent(eventName))
        {
            return;
        }

        _handlers[eventName].Remove(handlerType);
        if (!_handlers[eventName].Any())
        {
            _handlers.Remove(eventName);
            var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
            if (eventType != null)
            {
                _eventTypes.Remove(eventType);
            }

            RaiseOnEventRemoved(eventName);
        }
    }

    public IEnumerable<Type> GetHandlersForEvent<T>() where T : IntegrationEvent
    {
        var key = GetEventKey<T>();
        return GetHandlersForEvent(key);
    }

    public IEnumerable<Type> GetHandlersForEvent(string eventName)
    {
        return HasSubscriptionsForEvent(eventName) ? _handlers[eventName] : Enumerable.Empty<Type>();
    }

    public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
    {
        var key = GetEventKey<T>();
        return HasSubscriptionsForEvent(key);
    }

    public bool HasSubscriptionsForEvent(string eventName)
    {
        return _handlers.ContainsKey(eventName);
    }

    public Type GetEventTypeByName(string eventName)
    {
        return _eventTypes.SingleOrDefault(t => t.Name == eventName);
    }

    public string GetEventKey<T>() where T : IntegrationEvent
    {
        return typeof(T).Name;
    }

    public void Clear()
    {
        _handlers.Clear();
    }

    private void RaiseOnEventRemoved(string eventName)
    {
        OnEventRemoved?.Invoke(this, eventName);
    }
}