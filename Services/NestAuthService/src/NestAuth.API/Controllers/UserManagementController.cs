namespace NestAuth.API.Controllers;

[Route("/api/[controller]/[action]")]
[ApiController]
public class UserManagementController : ControllerBase
{
    private readonly IUserManagementService _userManagementService;

    public UserManagementController(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    [HttpGet]
    public async Task<IActionResult> VerifyEmail(string userId, string token, string email)
    {
        var res = await _userManagementService.VerifyEmailAsync(userId, token, email);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost]
    public async Task<IActionResult> BlockUser([FromBody] string userId)
    {
        var res = await _userManagementService.BlockUserAsync(userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost]
    public async Task<IActionResult> UnblockUser([FromBody] string userId)
    {
        var res = await _userManagementService.UnblockUserAsync(userId);
        return StatusCode(res.StatusCode, res);
    }
}