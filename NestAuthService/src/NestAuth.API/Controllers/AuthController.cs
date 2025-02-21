﻿namespace NestAuth.API.Controllers
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

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string userId, string token, string email)
        {
            var res = await _authService.VerifyEmailAsync(userId, token, email);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginRequest login)
        {
            var res = await _authService.LoginAsync(login);
            return StatusCode(res.StatusCode, res);
        }
    }
}