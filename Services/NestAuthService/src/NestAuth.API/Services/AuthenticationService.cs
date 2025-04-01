using AuthenticationException = Nest.Shared.Exceptions.AuthenticationException;

namespace NestAuth.API.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IUserSessionService _userSessionService;
    private readonly IEventBus _eventBus;
    private readonly ICacheService _cacheService;

    private readonly string _otpPrefix = "otp_";

    public AuthenticationService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IUserSessionService userSessionService,
        IEventBus eventBus, ICacheService cacheService)

    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _userSessionService = userSessionService;
        _eventBus = eventBus;
        _cacheService = cacheService;
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

        await _userManager.AddToRoleAsync(user, Roles.Customer);

        var confirmedEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        string confirmedUrl = Configurations.GetConfiguratinValue<string>("ClientUrl");
        confirmedUrl = string.Concat(confirmedUrl,
            $"/auth/verifyemail?token={confirmedEmailToken.Encode()}&userId={user.Id.Encode()}&email={user.Email?.Encode()}");

        UserRegisteredIntegrationEvent @event = new()
        {
            Email = user?.Email ?? string.Empty,
            ConfirmedUrl = confirmedUrl,
            UserName = user?.UserName ?? string.Empty,
        };
        await _eventBus.PublishAsync(@event);

        return new()
        {
            IsSuccess = true,
            Message = "User registered successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = null,
            Errors = null
        };
    }

    public async Task<ResponseDto> LoginAsync(LoginRequest request)
    {
        AppUser? user = Regex.IsMatch(request.EmailOrUsername, @"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+")
            ? await _userManager.FindByEmailAsync(request.EmailOrUsername)
            : await _userManager.FindByNameAsync(request.EmailOrUsername);

        if (user == null)
        {
            throw new AuthenticationException("UserName or Password is invalid");
        }

        if (user.UserStatus == UserStatus.Banned)
        {
            throw new AuthenticationException("User is blocked");
        }

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);

        if (result.IsLockedOut)
        {
            throw new AuthenticationException("Your account is locked out. Kindly wait for 15 minutes and try again");
        }

        if (!result.Succeeded && !result.RequiresTwoFactor)
        {
            throw new AuthenticationException("UserName or Password is invalid");
        }

        if (await _userManager.GetTwoFactorEnabledAsync(user))
        {
            var otp = _tokenService.GenerateOtpToken();
            var temporaryToken = _tokenService.GenerateTemporaryToken();
            var expiresAt = TimeSpan.FromMinutes(1);
            var key = $"{_otpPrefix}{temporaryToken}";

            var cacheData = new TwoFaCache()
            {
                UserId = user.Id,
                Otp = otp,
                TemporaryToken = temporaryToken,
                Expiration = expiresAt
            };
            await _cacheService.SetAsync(key,
                cacheData,
                expiresAt);

            await _eventBus.PublishAsync(new User2FaOtpSentIntegrationEvent()
            {
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                Otp = otp
            });

            return new()
            {
                IsSuccess = true,
                Message = "OTP sent to your email.",
                StatusCode = StatusCodes.Status202Accepted,
                Data = new
                {
                    UserId = user.Id,
                    ExpiresAt = expiresAt,
                    TemporaryToken = temporaryToken,
                    Email = EmailMasking.Mask(user.Email ?? string.Empty),
                },
            };
        }

        var (sessionId, @event) = await _userSessionService.CreateSessionAsync(user);
        var accessToken = await _tokenService.GenerateAccessTokenAsync(user, sessionId);
        await _eventBus.PublishAsync(@event);

        return new()
        {
            IsSuccess = true,
            Message = "User logged in successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = new
            {
                TokenInfo = accessToken,
                User = new
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                }
            }
        };
    }

    public async Task<ResponseDto> RefreshTokenAsync(RefreshTokenRequest request)
    {
        //todo: userID back ile qebul olunacaq 
        if (string.IsNullOrEmpty(request.RefreshToken) || string.IsNullOrEmpty(request.UserId))
        {
            throw new AuthenticationException("Invalid refresh token or userId");
        }

        var isValid = await _tokenService.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
        if (!isValid)
        {
            throw new AuthenticationException("Invalid refresh token or userId");
        }

        await _tokenService.MarkRefreshTokenAsUsedAsync(request.UserId, request.RefreshToken);

        var sessionId = await _tokenService.GetSessionId(request.UserId, request.RefreshToken);

        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new AuthenticationException("Invalid refresh token or userId");
        }

        var tokenResponse = await _tokenService.GenerateAccessTokenAsync(user, sessionId);

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

    public async Task<ResponseDto> LogoutAsync(LogOutRequest request)
    {
        //todo: userID back ile qebul olunacaq 
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new AuthenticationException("User not found");
        }

        await _signInManager.SignOutAsync();

        var sessionId = await _tokenService.GetSessionId(request.UserId, request.RefreshToken);

        await _userSessionService.RevokeSessionAsync(sessionId);

        await _tokenService.RevokeRefreshTokenAsync(request.UserId, request.RefreshToken);

        return new()
        {
            Errors = null,
            IsSuccess = true,
            Message = "User logged out successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = null
        };
    }

    public async Task<ResponseDto> EnableEmail2FAAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new AuthenticationException("User not found");
        }

        if (user.UserStatus == UserStatus.Banned || user.UserStatus == UserStatus.LockedOut || await _userManager.IsLockedOutAsync(user))
        {
            throw new AuthenticationException("User is blocked");
        }

        if (await _userManager.GetTwoFactorEnabledAsync(user))
        {
            throw new InvalidOperationException("Function is already enabled");
        }

        var res = await _userManager.SetTwoFactorEnabledAsync(user, true);
        if (!res.Succeeded)
        {
            var errors = string.Join(", ", res.Errors.Select(e => e.Description));
            throw new OperationFailedException(errors);
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "2FA enabled successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<ResponseDto> DisableEmail2FAAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new AuthenticationException("User not found");
        }

        if (user.UserStatus == UserStatus.Banned || user.UserStatus == UserStatus.LockedOut || await _userManager.IsLockedOutAsync(user))
        {
            throw new AuthenticationException("User is blocked");
        }

        if (await _userManager.GetTwoFactorEnabledAsync(user))
        {
            throw new InvalidOperationException("The function is already passive");
        }

        var res = await _userManager.SetTwoFactorEnabledAsync(user, false);
        if (!res.Succeeded)
        {
            var errors = string.Join(", ", res.Errors.Select(e => e.Description));
            throw new OperationFailedException(errors);
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "2FA enabled successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<ResponseDto> Regenerate2FACodeAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new AuthenticationException("UserName or Password is invalid");
        }

        if (user.UserStatus == UserStatus.Banned)
        {
            throw new AuthenticationException("User is blocked");
        }

        if (user.UserStatus == UserStatus.LockedOut || await _userManager.IsLockedOutAsync(user))
        {
            throw new AuthenticationException("Your account is locked out. Kindly wait for 15 minutes and try again");
        }

        var otp = _tokenService.GenerateOtpToken();
        var temporaryToken = _tokenService.GenerateTemporaryToken();
        var expiresAt = TimeSpan.FromMinutes(1);
        var key = $"{_otpPrefix}{temporaryToken}";

        var cacheData = new TwoFaCache()
        {
            UserId = user.Id,
            Otp = otp,
            TemporaryToken = temporaryToken,
            Expiration = expiresAt
        };
        await _cacheService.SetAsync(key,
            cacheData,
            expiresAt);

        await _eventBus.PublishAsync(new User2FaOtpSentIntegrationEvent()
        {
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            Otp = otp
        });

        return new()
        {
            IsSuccess = true,
            Message = "OTP sent to your email.",
            StatusCode = StatusCodes.Status202Accepted,
            Data = new
            {
                UserId = user.Id,
                ExpiresAt = expiresAt,
                TemporaryToken = temporaryToken,
                Email = EmailMasking.Mask(user.Email ?? string.Empty),
            },
        };
    }

    public async Task<ResponseDto> Verify2FACodeAsync(Verify2FACodeRequest request)
    {
        var cacheKey = $"{_otpPrefix}{request.TemporaryToken}";
        var cachedData = await _cacheService.GetAsync<TwoFaCache>(cacheKey);

        if (cachedData == null)
        {
            throw new AuthenticationException("Invalid OTP code or expired session");
        }

        var user = await _userManager.FindByIdAsync(cachedData.UserId);
        if (user == null)
        {
            throw new AuthenticationException("User not found");
        }

        if (user.UserStatus == UserStatus.LockedOut || await _userManager.IsLockedOutAsync(user))
        {
            throw new AuthenticationException("Your account is locked out. Kindly wait for 15 minutes and try again");
        }

        // OTP kodunu yoxlayırıq
        if (cachedData.Otp.GetHashCode() != request.Code.GetHashCode())
        {
            // Yanlış cəhdləri izləyirik
            string failedAttemptsKey = $"FAILED_ATTEMPTS_{user.Id}";
            int failedAttempts = await _cacheService.GetAsync<int>(failedAttemptsKey);
            failedAttempts++;

            // Cache-i yeniləyirik
            await _cacheService.SetAsync(failedAttemptsKey, failedAttempts, TimeSpan.FromMinutes(30));

            // 5 və ya daha çox yanlış cəhd olubsa, hesabı kilidləyirik
            if (failedAttempts >= 5)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(15));
                await _cacheService.RemoveAsync(failedAttemptsKey);
                throw new AuthenticationException("Too many invalid attempts. Your account has been locked for 15 minutes");
            }

            throw new AuthenticationException($"Invalid OTP code. You have {5 - failedAttempts} attempts remaining");
        }

        // Uğurlu olduqda kilidlənmə cəhdlərini sıfırlayırıq
        await _cacheService.RemoveAsync($"FAILED_ATTEMPTS_{user.Id}");
        await _cacheService.RemoveAsync(cacheKey);

        // Sessiya yaradıb token qaytarırıq
        var (sessionId, @event) = await _userSessionService.CreateSessionAsync(user);
        var accessToken = await _tokenService.GenerateAccessTokenAsync(user, sessionId);
        await _eventBus.PublishAsync(@event);

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "User logged in successfully",
            StatusCode = StatusCodes.Status200OK,
            Data = new
            {
                TokenInfo = accessToken,
                User = new
                {
                    UserId = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = await _userManager.GetRolesAsync(user)
                }
            }
        };
    }
}