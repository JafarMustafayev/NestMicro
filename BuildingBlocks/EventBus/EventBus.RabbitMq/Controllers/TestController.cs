using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public IActionResult SendEvent()
    {
        _eventBus.PublishAsync(new TestEvent { Message = "Hello World" });
        return Ok();
    }
}