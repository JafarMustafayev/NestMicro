namespace NestAuth.API.Controllers;

[Route("/api/[controller]/[action]")]
[ApiController]
public class SessionsController : ControllerBase
{
    private readonly IUserSessionService _userSessionService;

    public SessionsController(IUserSessionService userSessionService)
    {
        _userSessionService = userSessionService;
    }

    [HttpGet("{userId}")]
    public IActionResult ActiveSessions(string userId)
    {
        var res = _userSessionService.GetActiveSessionsByUser(userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{userId}/{sessionId}")]
    public async Task<IActionResult> GetSession(string userId, string sessionId)
    {
        var res = await _userSessionService.GetUserSessionByIdAsync(userId, sessionId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete("{sessionId}")]
    public async Task<IActionResult> Revoke(string sessionId)
    {
        var res = await _userSessionService.RevokeSessionAsync(sessionId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> RevokeAll(string userId)
    {
        var res = await _userSessionService.RevokeAllSessionsAsync(userId);
        return StatusCode(res.StatusCode, res);
    }
}