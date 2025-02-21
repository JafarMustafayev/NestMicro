namespace NestAuth.API.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenHandler _tokenHandler;

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<AppRole> roleManager,
        ITokenHandler tokenHandler)

    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _tokenHandler = tokenHandler;
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

    public async Task<ResponseDto> VerifyEmailAsync(string userId, string token, string email)
    {
        userId = userId.Decode();
        email = email.Decode();
        token = token.Decode();

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null || user.Id != userId)
        {
            throw new AuthenticationException();
        }

        var res = await _userManager.ConfirmEmailAsync(user, token);

        if (!res.Succeeded)
        {
            throw new AuthenticationException("Email verification failed");
        }

        await _userManager.UpdateSecurityStampAsync(user);

        user.UserStatus = UserStatus.Active;

        return new()
        {
            Errors = null,
            IsSuccess = true,
            Message = "Email verified successfully",
            StatusCode = 200,
            Data = null
        };
    }

    public async Task<ResponseDto> LoginAsync(LoginRequest request)
    {
        AppUser? user = await _userManager.FindByEmailAsync(request.EmailOrUsername);
        if (user == null)
        {
            user = await _userManager.FindByNameAsync(request.EmailOrUsername);
            if (user == null)
            {
                throw new AuthenticationException("UserName or Password is invalid");
            }
        }

        if (user.UserStatus == UserStatus.Banned)
        {
            throw new AuthenticationException("User is blocked");
        }

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

        if (!result.Succeeded)
        {
            throw new AuthenticationException("UserName or Password is invalid");
        }

        var res = await _tokenHandler.GenerateAccessTokenAsync(user);

        return new()
        {
            Errors = null,
            IsSuccess = true,
            Message = "User logged in successfully",
            StatusCode = 200,
            Data = new
            {
                TokenInfo = res,
                User = new
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                }
            }
        };
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