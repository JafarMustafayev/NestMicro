namespace NestAuth.API.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordManagementService _passwordManagementService;

        public PasswordController(IPasswordManagementService passwordManagementService)
        {
            _passwordManagementService = passwordManagementService;
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var res = await _passwordManagementService.ForgotPasswordAsync(email);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPassword)
        {
            var res = await _passwordManagementService.ResetPasswordAsync(resetPassword);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var res = await _passwordManagementService.ChangePasswordAsync(request);
            return StatusCode(res.StatusCode, res);
        }
    }
}