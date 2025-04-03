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

    private const string OTP_PREFIX = "otp_";
    private const string FAILED_ATTEMPTS_PREFIX = "FAILED_ATTEMPTS_";
    private const int MAX_FAILED_ATTEMPTS = 5;
    private const int OTP_EXPIRATION_MINUTES = 1;
    private const int LOCKOUT_MINUTES = 15;

    public AuthenticationService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IUserSessionService userSessionService,
        IEventBus eventBus,
        ICacheService cacheService)
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
        // Check if user already exists by email or username
        var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingUserByEmail != null)
        {
            throw new DuplicateEntityException("User with this email already exists");
        }

        var existingUserByUsername = await _userManager.FindByNameAsync(request.UserName);
        if (existingUserByUsername != null)
        {
            throw new DuplicateEntityException("User with this username already exists");
        }

        // Create new user
        var user = new AppUser
        {
            Email = request.Email,
            UserName = request.UserName,
            UserStatus = UserStatus.PendingVerification,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(Environment.NewLine,
                result.Errors.Select(e => e.Description));
            throw new OperationFailedException(errors);
        }

        await _userManager.AddToRoleAsync(user, Roles.Customer);

        // Generate email confirmation token and URL
        var confirmedEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var clientUrl = Configurations.GetConfiguratinValue<string>("ClientUrl");
        var confirmedUrl = $"{clientUrl}/auth/verifyemail?token={confirmedEmailToken.Encode()}&userId={user.Id.Encode()}&email={user.Email?.Encode()}";

        // Publish user registration event
        await _eventBus.PublishAsync(new UserRegisteredIntegrationEvent
        {
            Email = user.Email ?? string.Empty,
            ConfirmedUrl = confirmedUrl,
            UserName = user.UserName ?? string.Empty,
        });

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "User registered successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<ResponseDto> LoginAsync(LoginRequest request)
    {
        // Find user by email or username
        var user = Regex.IsMatch(request.EmailOrUsername, @"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+")
            ? await _userManager.FindByEmailAsync(request.EmailOrUsername)
            : await _userManager.FindByNameAsync(request.EmailOrUsername);

        if (user == null)
        {
            throw new AuthenticationException("UserName or Password is invalid");
        }

        await ValidateUserStatus(user);

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);

        if (result.IsLockedOut)
        {
            throw new AuthenticationException("Your account is locked out. Kindly wait for 15 minutes and try again");
        }

        if (!result.Succeeded && !result.RequiresTwoFactor)
        {
            throw new AuthenticationException("UserName or Password is invalid");
        }

        // Handle 2FA if enabled
        if (await _userManager.GetTwoFactorEnabledAsync(user))
        {
            return await Handle2FALogin(user);
        }

        // Standard login flow
        return await CompleteLoginProcess(user);
    }

    private async Task<ResponseDto> Handle2FALogin(AppUser user)
    {
        var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
        var temporaryToken = _tokenService.GenerateTemporaryToken();
        var expiresAt = TimeSpan.FromMinutes(OTP_EXPIRATION_MINUTES);
        var key = $"{OTP_PREFIX}{temporaryToken}";

        var cacheData = new TwoFaCache
        {
            UserId = user.Id,
            TemporaryToken = temporaryToken,
            Expiration = expiresAt
        };

        if (providers.Contains("Authenticator"))
        {
            await _cacheService.SetAsync(key, cacheData, expiresAt);

            return new ResponseDto
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status202Accepted,
                Message = "Enter your authenticator code",
                Data = new
                {
                    UserId = user.Id,
                    ExpiresAt = expiresAt,
                    RequiresAuthenticator = true,
                    TemporaryToken = temporaryToken,
                }
            };
        }
        else if (providers.Contains("Email"))
        {
            var otp = _tokenService.GenerateOtpToken();
            cacheData.Otp = otp;

            await _cacheService.SetAsync(key, cacheData, expiresAt);

            await _eventBus.PublishAsync(new User2FaOtpSentIntegrationEvent
            {
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                Otp = otp
            });

            return new ResponseDto
            {
                IsSuccess = true,
                Message = "OTP sent to your email.",
                StatusCode = StatusCodes.Status202Accepted,
                Data = new
                {
                    UserId = user.Id,
                    ExpiresAt = expiresAt,
                    TemporaryToken = temporaryToken,
                    RequiresEmailAuthenticator = true,
                    Email = EmailMasking.Mask(user.Email ?? string.Empty),
                }
            };
        }

        // Fallback if no supported 2FA method
        return await CompleteLoginProcess(user);
    }

    private async Task<ResponseDto> CompleteLoginProcess(AppUser user)
    {
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
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                }
            }
        };
    }

    public async Task<ResponseDto> RefreshTokenAsync(RefreshTokenRequest request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken) || string.IsNullOrEmpty(request.UserId))
        {
            throw new AuthenticationException("Invalid refresh token or userId");
        }

        // Validate refresh token
        var isValid = await _tokenService.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
        if (!isValid)
        {
            throw new AuthenticationException("Invalid refresh token or userId");
        }

        // Mark token as used and get session ID
        await _tokenService.MarkRefreshTokenAsUsedAsync(request.UserId, request.RefreshToken);
        var sessionId = await _tokenService.GetSessionId(request.UserId, request.RefreshToken);

        // Get user and generate new tokens
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new AuthenticationException("Invalid refresh token or userId");
        }

        var tokenResponse = await _tokenService.GenerateAccessTokenAsync(user, sessionId);

        return new ResponseDto
        {
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
        if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.RefreshToken))
        {
            throw new AuthenticationException("Invalid user ID or refresh token");
        }

        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new AuthenticationException("User not found");
        }

        await _signInManager.SignOutAsync();

        var sessionId = await _tokenService.GetSessionId(request.UserId, request.RefreshToken);
        await _userSessionService.RevokeSessionAsync(sessionId);
        await _tokenService.RevokeRefreshTokenAsync(request.UserId, request.RefreshToken);

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "User logged out successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<ResponseDto> EnableEmail2FAAsync(string userId)
    {
        var user = await GetAndValidateUser(userId);

        if (await _userManager.GetTwoFactorEnabledAsync(user))
        {
            throw new InvalidOperationException("Function is already enabled");
        }

        var result = await _userManager.SetTwoFactorEnabledAsync(user, true);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
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
        var user = await GetAndValidateUser(userId);

        if (!await _userManager.GetTwoFactorEnabledAsync(user))
        {
            throw new InvalidOperationException("The function is already passive");
        }

        var result = await _userManager.SetTwoFactorEnabledAsync(user, false);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new OperationFailedException(errors);
        }

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "2FA disabled successfully",
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<ResponseDto> Regenerate2FACodeAsync(string userId)
    {
        var user = await GetAndValidateUser(userId);

        var otp = _tokenService.GenerateOtpToken();
        var temporaryToken = _tokenService.GenerateTemporaryToken();
        var expiresAt = TimeSpan.FromMinutes(OTP_EXPIRATION_MINUTES);
        var key = $"{OTP_PREFIX}{temporaryToken}";

        var cacheData = new TwoFaCache
        {
            UserId = user.Id,
            Otp = otp,
            TemporaryToken = temporaryToken,
            Expiration = expiresAt
        };

        await _cacheService.SetAsync(key, cacheData, expiresAt);

        await _eventBus.PublishAsync(new User2FaOtpSentIntegrationEvent
        {
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            Otp = otp
        });

        return new ResponseDto
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
            }
        };
    }

    public async Task<ResponseDto> LoginWithEmailAuthenticator(Verify2FACodeRequest request)
    {
        var cacheKey = $"{OTP_PREFIX}{request.TemporaryToken}";
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

        await ValidateUserStatus(user);

        var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
        bool isValid;

        if (providers.Contains("Authenticator"))
        {
            isValid = await _userManager.VerifyTwoFactorTokenAsync(
                user,
                _userManager.Options.Tokens.AuthenticatorTokenProvider,
                request.Code
            );
        }
        else
        {
            isValid = cachedData.Otp?.GetHashCode() == request.Code.GetHashCode();
        }

        if (!isValid)
        {
            await HandleFailedAuthenticationAttempt(user.Id);
            throw new AuthenticationException($"Invalid OTP code");
        }

        // Clear failed attempts and cache on successful login
        await _cacheService.RemoveAsync($"{FAILED_ATTEMPTS_PREFIX}{user.Id}");
        await _cacheService.RemoveAsync(cacheKey);

        // Complete login process
        return await CompleteLoginProcess(user);
    }

    public async Task<ResponseDto> VerifyAuthenticator2FAAsync(string userId, string code)
    {
        var user = await GetAndValidateUser(userId);

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(
            user,
            _userManager.Options.Tokens.AuthenticatorTokenProvider,
            code
        );

        if (!isValid)
        {
            throw new AuthenticationException("Invalid code");
        }

        // Enable 2FA
        await _userManager.SetTwoFactorEnabledAsync(user, true);

        // Generate recovery codes
        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        user.RecoveryCodes = recoveryCodes;
        await _userManager.UpdateAsync(user);

        return new ResponseDto
        {
            IsSuccess = true,
            Data = new
            {
                RecoveryCodes = recoveryCodes
            }
        };
    }

    public async Task<ResponseDto> SetupAuthenticator2FAAsync(string userId)
    {
        var user = await GetAndValidateUser(userId);

        // Generate secret key if not exists
        var secretKey = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(secretKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            secretKey = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        // Create QR code URI
        var issuer = "NestAuth"; // Get from configuration
        var qrCodeUri = GenerateQrCodeUri(user.Email, secretKey, issuer);

        return new ResponseDto
        {
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            Message = "Authenticator QR Code",
            Data = new
            {
                SecretKey = secretKey,
                QrCodeUri = qrCodeUri
            }
        };
    }

    #region Helper Methods

    private async Task<AppUser> GetAndValidateUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new AuthenticationException("User not found");
        }

        await ValidateUserStatus(user);
        return user;
    }

    private async Task ValidateUserStatus(AppUser user)
    {
        if (user.UserStatus == UserStatus.Banned)
        {
            throw new AuthenticationException("User is blocked");
        }

        if (user.UserStatus == UserStatus.LockedOut || await _userManager.IsLockedOutAsync(user))
        {
            throw new AuthenticationException("Your account is locked out. Kindly wait for 15 minutes and try again");
        }
    }

    private async Task HandleFailedAuthenticationAttempt(string userId)
    {
        string failedAttemptsKey = $"{FAILED_ATTEMPTS_PREFIX}{userId}";
        int failedAttempts = await _cacheService.GetAsync<int>(failedAttemptsKey);
        failedAttempts++;

        // Update cache
        await _cacheService.SetAsync(failedAttemptsKey, failedAttempts, TimeSpan.FromMinutes(30));

        // Lock account if max attempts reached
        if (failedAttempts >= MAX_FAILED_ATTEMPTS)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(LOCKOUT_MINUTES));
                await _cacheService.RemoveAsync(failedAttemptsKey);
                throw new AuthenticationException($"Too many invalid attempts. Your account has been locked for {LOCKOUT_MINUTES} minutes");
            }
        }

        throw new AuthenticationException($"Invalid code. You have {MAX_FAILED_ATTEMPTS - failedAttempts} attempts remaining");
    }

    private string GenerateQrCodeUri(string email, string secretKey, string issuer)
    {
        return $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(email)}?secret={secretKey}&issuer={Uri.EscapeDataString(issuer)}";
    }

    #endregion Helper Methods
}