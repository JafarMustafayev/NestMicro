namespace NestNotification.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScheduledEmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ScheduledEmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Schedule([FromForm] SendScheduledEmailDto sendScheduledEmailDto)
        {
            var res = await _emailService.ScheduleEmailAsync(sendScheduledEmailDto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleTemplated([FromForm] SendScheduledTemplateEmailDto sendScheduledTemplateEmailDto)
        {
            var res = await _emailService.ScheduleTemplatedEmailAsync(sendScheduledTemplateEmailDto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> SendPriority([FromForm] SendEmailDto emailDto)
        {
            var res = await _emailService.SendPriorityEmailAsync(emailDto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> SendPriorityTemplated([FromForm] SendTemplatedEmailDto emailDto)
        {
            var res = await _emailService.SendPriorityTemplatedEmailAsync(emailDto);
            return StatusCode(res.StatusCode, res);
        }
    }
}