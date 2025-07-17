namespace NestAuth.API.Services;

public class PasswordManagementService : IPasswordManagementService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEventBus _eventBus;

    public PasswordManagementService(
        UserManager<AppUser> userManager,
        IEventBus eventBus)
    {
        _userManager = userManager;
        _eventBus = eventBus;
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

        var clientUrl = Configurations.GetConfiguration<ExternalServices>().ClientUrl;
        var resetUrl = string.Concat(clientUrl,
            $"auth/resetpassword?token={token.Encode()}&email={user?.Email?.Encode()}");

        UserPasswordResetRequestedIntegrationEvent @event = new()
        {
            Email = user?.Email ?? string.Empty,
            ResetUrl = resetUrl,
            UserName = user?.UserName ?? string.Empty
        };
        await _eventBus.PublishAsync(@event);

        return new()
        {
            IsSuccess = true,
            Message = "Password reset link sent to your email",
            StatusCode = StatusCodes.Status200OK,
            Errors = null,
            Data = null
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

            throw new AuthenticationException(errors);
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
}