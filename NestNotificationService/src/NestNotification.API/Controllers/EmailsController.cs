namespace NestNotification.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailsController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromForm] SendEmailDto emailDto)
        {
            var res = await _emailService.SendEmailAsync(emailDto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> SendTemplatedEmail([FromForm] SendTemplatedEmailDto emailDto)
        {
            var res = await _emailService.SendTemplatedEmailAsync(emailDto);
            return StatusCode(res.StatusCode, res);
        }
    }
}