namespace NestAuth.API.Services;

public class CurrentUserDataService : ICurrentUserDataService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserDataService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? GetEmail => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    public string? GetUsername => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}