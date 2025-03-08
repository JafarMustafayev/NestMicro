namespace NestNotification.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TemplatesController : ControllerBase
    {
        private readonly IEmailTemplateService _emailTemplateService;

        public TemplatesController(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }

        [HttpGet]
        public IActionResult GetEmailTemplates()
        {
            var res = _emailTemplateService.GetEmailTemplates();
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmailTemplateByNameAsync(string templateName)
        {
            var res = await _emailTemplateService.GetEmailTemplateByNameAsync(templateName);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("{templateId}")]
        public async Task<IActionResult> GetEmailTemplateByIdAsync(string templateId)
        {
            var res = await _emailTemplateService.GetEmailTemplateByIdAsync(templateId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmailTemplateAsync([FromBody] CreateEmailTemplateDto templateDto)
        {
            var res = await _emailTemplateService.AddEmailTemplateAsync(templateDto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmailTemplateAsync([FromBody] UpdateEmailTemplateDto templateDto)
        {
            var res = await _emailTemplateService.UpdateEmailTemplateAsync(templateDto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmailTemplateAsync([FromBody] DeleteEmailTemplateDto templateDto)
        {
            var res = await _emailTemplateService.DeleteEmailTemplateAsync(templateDto);
            return StatusCode(res.StatusCode, res);
        }
    }
}