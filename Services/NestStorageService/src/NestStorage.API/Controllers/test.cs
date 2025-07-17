using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using EventBus.Abstractions.Events;

namespace NestStorage.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class test : ControllerBase
{
    private readonly IFileValidationService _fileValidationService;
    private readonly IEventBus _eventBus;

    public test(
        IFileValidationService fileValidationService,
        IEventBus eventBus)
    {
        _fileValidationService = fileValidationService;
        _eventBus = eventBus;
    }

    [HttpPost]
    public async Task<IActionResult> FileTest(IFormFile file)
    {
        var requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var res = await _fileValidationService.ValidateFileAsync(file);

        var options = new ImageProcessingOptions
        {
            OutputFileName = "test123.jpg"
        };

        var evet = new FileUploadRequestedIntegrationEvent
        {
            BucketName = "test",
            FileName = "guid",
            FileType = "image",
            OutputFileName = "test123.jpg",
            OptinosOfProcessing = options
        };

        await _eventBus.PublishAsync(evet);

        var responseTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        return Ok(new
        {
            requestTime = requestTime,
            res = res,
            responseTime = responseTime
        });
    }

    [HttpGet]
    public IActionResult admin()
    {
        var requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        var color = Color.Aquamarine;
        return Ok(new
        {
            ozu = color,
            color = color.Name,
            color.IsKnownColor,
            requestTime = requestTime
        });
    }
}