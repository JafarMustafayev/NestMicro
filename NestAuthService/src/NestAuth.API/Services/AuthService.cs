namespace NestAuth.API.Services;

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

    public async Task<ResponseDto> RegisterAsync(RegisterRequest request)
    {
        AppUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null)
        {
            throw new DuplicateEntityException("User with this email already exists");
        }

        user = await _userManager.FindByNameAsync(request.UserName);
        if (user != null)
        {
            throw new DuplicateEntityException("User with this username already exists");
        }

        //todo: add mapping to AutoMapper if needed
        user = new()
        {
            Email = request.Email,
            UserName = request.UserName,
            UserStatus = UserStatus.PendingVerification,
            CreatedAt = DateTime.Now
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Empty;
            foreach (var item in result.Errors.Select(e => e.Description).ToList())
            {
                errors += item + Environment.NewLine;
            }
            throw new OperationFailedException(errors);
        }

        //await _userManager.AddToRoleAsync(user, Roles.Customer);

        var confirmedEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        string confirmeUrl = Configuration.GetConfiguratinValue<string>("ClientUrl");
        confirmeUrl = string.Concat(confirmeUrl, $"/auth/verifyemail?token={confirmedEmailToken.Encode()}&userId={user.Id.Encode()}&email={user.Email?.Encode()}");

        // send email

        return new()
        {
            Errors = null,
            IsSuccess = true,
            Message = "User registered successfully",
            StatusCode = 200,
            Data = new
            {
                emailVerifyUrl = confirmeUrl,
            }
        };
    }
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> LoginAsync(LoginRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> VerifyEmailAsync(string userId, string token, string email)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> ForgotPasswordAsync(ForgotPasswordRequest request)
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

    public Task<ResponseDto> AssignRoleAsync(AssignRoleRequest request)
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