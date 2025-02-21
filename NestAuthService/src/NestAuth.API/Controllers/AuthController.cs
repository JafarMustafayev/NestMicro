namespace NestAuth.API.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult test()
        {
            return Ok(new[] { "value1", "value2" });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterRequest register)
        {
            var res = await _authService.RegisterAsync(register);
            return StatusCode(res.StatusCode, res);
        }
        {
            return Ok(register);
        }
    }
}