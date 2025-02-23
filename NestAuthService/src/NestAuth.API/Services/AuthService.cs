namespace NestAuth.API.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenHandler;

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<AppRole> roleManager,
        ITokenService tokenHandler)

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
            CreatedAt = DateTime.UtcNow
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

        // TODO: send email

        return new()
        {
            Errors = null,
            IsSuccess = true,
            Message = "User registered successfully",
            StatusCode = StatusCodes.Status200OK,
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
            StatusCode = StatusCodes.Status200OK,
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
            StatusCode = StatusCodes.Status200OK,
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

    public async Task<ResponseDto> ForgotPasswordAsync(string email)
    {
        var isValid = Regex.IsMatch(email, "[^@ \\t\\r\\n]+@[^@ \\t\\r\\n]+\\.[^@ \\t\\r\\n]+");

        if (!isValid)
        {
            throw new Nest.Shared.Exceptions.ValidationException("Invalid Email address");
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new AuthenticationException("User not found");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var resetUrl = Configuration.GetConfiguratinValue<string>("ClientUrl");
        resetUrl = string.Concat(resetUrl, $"/auth/resetpassword?token={token.Encode()}&email={user.Email.Encode()}");

        // TODO:send email

        return new()
        {
            IsSuccess = true,
            Message = "Password reset link sent to your email",
            Errors = null,
            StatusCode = StatusCodes.Status200OK,
            Data = new
            {
                resetUrl = resetUrl,
            }
        };
    }

    public async Task<ResponseDto> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var isValid = Regex.IsMatch(request.Email.Decode(), "[^@ \\t\\r\\n]+@[^@ \\t\\r\\n]+\\.[^@ \\t\\r\\n]+");
        if (!isValid)
        {
            throw new Nest.Shared.Exceptions.ValidationException("Invalid Email address");
        }

        var user = await _userManager.FindByEmailAsync(request.Email.Decode());
        if (user == null)
        {
            throw new AuthenticationException("Invalid Email address");
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token.Decode(), request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Empty;
            foreach (var item in result.Errors.Select(e => e.Description).ToList())
            {
                errors += item + Environment.NewLine;
            }
            throw new OperationFailedException(errors);
        }

        await _userManager.UpdateSecurityStampAsync(user);

        return new()
        {
            IsSuccess = true,
            Message = "Password reset successfully",
            Errors = null,
            Data = null,
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<ResponseDto> RefreshTokenAsync(RefreshTokenRequest request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken) || string.IsNullOrEmpty(request.UserId))
        {
            throw new AuthenticationException("Invalid refresh token or userId");
        }

        var isValid = await _tokenHandler.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
        if (!isValid)
        {
            throw new AuthenticationException("Invalid refresh token or userId");
        }

        await _tokenHandler.MarkRefreshTokenAsUsedAsync(request.UserId, request.RefreshToken);

        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new AuthenticationException("Invalid refresh token or userId");
        }

        var tokenResponse = await _tokenHandler.GenerateAccessTokenAsync(user);

        return new()
        {
            Errors = null,
            IsSuccess = true,
            Message = "Token refreshed successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = new
            {
                TokenInfo = tokenResponse,
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

    public async Task<ResponseDto> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new AuthenticationException("User not found");
        }
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Empty;
            foreach (var item in result.Errors.Select(e => e.Description).ToList())
            {
                errors += item + Environment.NewLine;
            }
            throw new OperationFailedException(errors);
        }
        await _userManager.UpdateSecurityStampAsync(user);

        return new()
        {
            Errors = null,
            IsSuccess = true,
            Message = "Password changed successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = null
        };
    }

    public async Task<ResponseDto> BlockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new AuthenticationException("User not found");
        }
        user.UserStatus = UserStatus.Banned;
        await _userManager.UpdateAsync(user);
        return new()
        {
            Errors = null,
            IsSuccess = true,
            Message = "User blocked successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = null
        };
    }

    public async Task<ResponseDto> UnblockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new AuthenticationException("User not found");
        }
        user.UserStatus = UserStatus.Active;
        await _userManager.UpdateAsync(user);
        return new()
        {
            Errors = null,
            IsSuccess = true,
            Message = "User unblocked successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = null
        };
    }

    public async Task<ResponseDto> LogoutAsync(LogOutRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new AuthenticationException("User not found");
        }
        await _signInManager.SignOutAsync();

        await _tokenHandler.RevokeRefreshTokenAsync(request.UserId, request.RefreshToken);

        return new()
        {
            Errors = null,
            IsSuccess = true,
            Message = "User logged out successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = null
        };
    }
}