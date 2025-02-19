namespace NestAuth.API.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new[] { "value1", "value2" });
        }

        [HttpPost]
        public IActionResult test([FromForm] RegisterRequest register)
        {
            return Ok(register);
        }
    }
}