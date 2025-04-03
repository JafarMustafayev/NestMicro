namespace NestAuth.API.Controllers;

[Route("/api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

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

    [HttpPost]
    public async Task<IActionResult> LoginWith2Fa([FromBody] Verify2FACodeRequest request)
    {
        var res = await _authenticationService.LoginWithEmailAuthenticator(request);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet]
    public async Task<IActionResult> Regenerate2FaCode(string userId)
    {
        var res = await _authenticationService.Regenerate2FACodeAsync(userId);
        return StatusCode(res.StatusCode, res);
    }

    #region Authenticator With App

    [HttpPost]
    public async Task<IActionResult> SetupAuthenticator2Fa(string userId)
    {
        var res = await _authenticationService.SetupAuthenticator2FAAsync(userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost]
    public async Task<IActionResult> VerifyAuthenticator2Fa(string userId, string code)
    {
        var res = await _authenticationService.VerifyAuthenticator2FAAsync(userId, code);
        return StatusCode(res.StatusCode, res);
    }

    #endregion

    #region Authenticator With Email

    [HttpPost]
    public async Task<IActionResult> EnableEmailAuthenticator(string userId)
    {
        var res = await _authenticationService.EnableEmail2FAAsync(userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost]
    public async Task<IActionResult> DisableEmailAuthenticator(string userId)
    {
        var res = await _authenticationService.DisableEmail2FAAsync(userId);
        return StatusCode(res.StatusCode, res);
    }

    #endregion
}