namespace EventBus.RabbitMq;

public class TestEventHandler : IIntegrationEventHandler<TestEvent>
{
    public Task Handle(TestEvent @event)
    {
        Console.WriteLine("-----------------------------------");
        Console.WriteLine($"Received ID: {@event.Id}");
        Console.WriteLine($"Received Version: {@event.Version}");
        Console.WriteLine($"Received Serial Number: {@event.SerialNumber}");
        Console.WriteLine($"Received Message: {@event.Message}");
        Console.WriteLine("-----------------------------------");

        return Task.CompletedTask;
    }
}