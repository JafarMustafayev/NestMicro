namespace NestNotification.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailManagementController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailManagementController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("{page}")]
        public async Task<IActionResult> GetEmails(int page)
        {
            var res = await Task.Run(() => _emailService.GetEmailLogs(page, 20));
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("{emailId}")]
        public async Task<IActionResult> GetEmailStatus(string emailId)
        {
            var res = await _emailService.GetEmailStatusAsync(emailId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost("{emailId}")]
        public async Task<IActionResult> ResendEmail(string emailId)
        {
            var res = await _emailService.ResendFailedEmailAsync(emailId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpDelete("{emailId}")]
        public async Task<IActionResult> CancelEmail(string emailId)
        {
            var res = await _emailService.CancelPendingEmailAsync(emailId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpDelete]
        public async Task<IActionResult> CancelAllPendingEmails([FromQuery] string recipientEmail)
        {
            var res = await _emailService.CancelAllPendingEmailsAsync(recipientEmail);
            return StatusCode(res.StatusCode, res);
        }
    }
}