namespace NestAuth.API.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public Task<ResponseDto> RegisterAsync(RegisterRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> LoginAsync(LoginRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> VerifyEmailAsync(string userId, string token)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> ForgotPasswordAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> ResetPasswordAsync(ResetPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> AssignRoleAsync(string userId, string role)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> BlockUserAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> UnblockUserAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> LogoutAsync(string userId)
    {
        throw new NotImplementedException();
    }
}