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
        public async Task<IActionResult> GetTemplateByName(string templateName)
        {
            var res = await _emailTemplateService.GetEmailTemplateByNameAsync(templateName);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("{templateId}")]
        public async Task<IActionResult> GetTemplateById(string templateId)
        {
            var res = await _emailTemplateService.GetEmailTemplateByIdAsync(templateId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplate([FromBody] CreateEmailTemplateDto templateDto)
        {
            var res = await _emailTemplateService.CreateEmailTemplateAsync(templateDto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTemplate([FromBody] UpdateEmailTemplateDto templateDto)
        {
            var res = await _emailTemplateService.UpdateEmailTemplateAsync(templateDto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTemplate([FromBody] DeleteEmailTemplateDto templateDto)
        {
            var res = await _emailTemplateService.DeleteEmailTemplateAsync(templateDto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("{templateId}")]
        public async Task<IActionResult> GetTemplateAttributes(string templateId)
        {
            var res = await _emailTemplateService.GetTemplateAttributeByIdAsync(templateId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("{attributeId}")]
        public async Task<IActionResult> GetTemplateAttribute(string attributeId)
        {
            var res = await _emailTemplateService.GetTemplateAttributeByIdAsync(attributeId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplateAttribute([FromBody] CreateTemplateAttributeDto attributeDto)
        {
            var res = await _emailTemplateService.CreateTemplateAttributeAsync(attributeDto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTemplateAttribute([FromBody] UpdateTemplateAttributeDto attributeDto)
        {
            var res = await _emailTemplateService.UpdateTemplateAttributeAsync(attributeDto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTemplateAttribute([FromBody] DeleteTemplateAttributeDto attributeDto)
        {
            var res = await _emailTemplateService.DeleteTemplateAttributeAsync(attributeDto);
            return StatusCode(res.StatusCode, res);
        }
    }
}