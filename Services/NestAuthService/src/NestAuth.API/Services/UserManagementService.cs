namespace NestAuth.API.Services;

public class UserManagementService : IUserManagementService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEventBus _eventBus;

    public UserManagementService(
        UserManager<AppUser> userManager,
        IEventBus eventBus)
    {
        _userManager = userManager;
        _eventBus = eventBus;
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
        await _userManager.UpdateAsync(user);

        UserEmailConfirmedIntegrationEvent @enent = new()
        {
            UserId = user.Id,
            UserName = user?.UserName ?? string.Empty,
            Email = user?.Email ?? string.Empty,
            ClientUrl = Configurations.GetConfiguratinValue<string>("ClientUrl"),
        };

        await _eventBus.PublishAsync(@enent);

        return new()
        {
            Errors = null,
            IsSuccess = true,
            Message = "Email verified successfully",
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
}