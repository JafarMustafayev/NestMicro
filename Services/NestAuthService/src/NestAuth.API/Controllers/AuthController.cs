namespace NestAuth.API.Controllers;

[Route("/api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserSessionRepository _userSessionService;
    private readonly ICacheService _cacheService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequest register)
    {
        var res = await _authenticationService.RegisterAsync(register);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        var res = await _authenticationService.LoginAsync(login);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var res = await _authenticationService.RefreshTokenAsync(request);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost]
    public async Task<IActionResult> Logout([FromBody] LogOutRequest request)
    {
        var res = await _authenticationService.LogoutAsync(request);
        return StatusCode(res.StatusCode, res);
    }
}