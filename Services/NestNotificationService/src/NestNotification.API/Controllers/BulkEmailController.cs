namespace NestNotification.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BulkEmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public BulkEmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> SendBulkEmail([FromBody] SendBulkEmailDto emailDto)
    {
        var res = await _emailService.SendBulkEmailAsync(emailDto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost]
    public async Task<IActionResult>  SendBulkTemplatedEmail([FromBody] SendBulkTemplatedEmailDto emailDto)
    {
        var res = await _emailService.SendBulkTemplatedEmailAsync(emailDto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost]
    public async Task<IActionResult> ScheduleBulkEmail([FromBody] SendScheduleBulkEmailDto sendScheduleBulkEmailDto)
    {
        var res = await _emailService.ScheduleBulkEmailAsync(sendScheduleBulkEmailDto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost]
    public async Task<IActionResult> ScheduleBulkTemplatedEmail ([FromBody] SendScheduleBulkTemplatedEmailDto sendScheduleBulkTemplatedEmailDto)
    {
        var res = await _emailService.ScheduleBulkTemplatedEmailAsync(sendScheduleBulkTemplatedEmailDto);
        return StatusCode(res.StatusCode, res);
    }
}