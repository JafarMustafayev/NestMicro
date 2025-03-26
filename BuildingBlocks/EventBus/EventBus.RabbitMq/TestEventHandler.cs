namespace EventBus.RabbitMq;

public class TestEventHandler : IIntegrationEventHandler<TestEvent>
{
    public Task Handle(TestEvent @event)
    {
        Console.WriteLine($"Received: {@event.Message}");
        return Task.CompletedTask;
    }
}