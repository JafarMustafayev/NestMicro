namespace NestNotification.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IEmailService _mailService;

        public HealthController(IEmailService mailService)
        {
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpPost]
        public async Task<IActionResult> SendEmailAsync(SendScheduledEmailDto emailDto)
        {
            await _mailService.ScheduleEmailAsync(emailDto);
            return Ok(new
            {
                EmailId = "emailId",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}