namespace EventBus.RabbitMq.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IEventBus _eventBus;

    public TestController(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    [HttpGet("{message}/{serialNumber}/{version}")]
    public async Task<IActionResult> SendTestEvent(string message, string serialNumber, int version)
    {
        var @event = new TestEvent()
        {
            SerialNumber = serialNumber,
            Version = version,
            Message = message,
        };
        await _eventBus.PublishAsync(@event);
        return Ok("Event send");
    }
}