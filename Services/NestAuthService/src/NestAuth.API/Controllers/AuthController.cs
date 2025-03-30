﻿namespace NestAuth.API.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserSessionRepository _userSessionService;
        private readonly ICacheService _cacheService;

        public AuthController(IAuthService authService, IUserSessionRepository userSessionService, ICacheService cacheService)
        {
            _authService = authService;
            _userSessionService = userSessionService;
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var datas = await _cacheService.GetAllDatasAsync<UserSession>();
            if (!datas.Any())
            {
                datas = _userSessionService.GetAll(true);
                var isSuccesfulAsync = await _cacheService.SetAsync("", datas, TimeSpan.FromMinutes(1));
                return isSuccesfulAsync ? Ok(datas) : BadRequest();
            }

            return Ok(datas);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
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
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var res = await _authService.LoginAsync(login);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var res = await _authService.ForgotPasswordAsync(email);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPassword)
        {
            var res = await _authService.ResetPasswordAsync(resetPassword);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var res = await _authService.RefreshTokenAsync(request);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var res = await _authService.ChangePasswordAsync(request);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser([FromBody] string userId)
        {
            var res = await _authService.BlockUserAsync(userId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser([FromBody] string userId)
        {
            var res = await _authService.UnblockUserAsync(userId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> Logout([FromBody] LogOutRequest request)
        {
            var res = await _authService.LogoutAsync(request);
            return StatusCode(res.StatusCode, res);
        }
    }
}