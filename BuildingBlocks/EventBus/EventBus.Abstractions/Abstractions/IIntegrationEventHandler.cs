using EventBus.Abstractions.Events;

namespace EventBus.Abstractions.Abstractions;

public interface IIntegrationEventHandler<TEvent> where TEvent : IntegrationEvent
{
    Task Handle(TEvent @event);
}