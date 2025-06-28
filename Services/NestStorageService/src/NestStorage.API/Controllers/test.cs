using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NestStorage.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class test : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}