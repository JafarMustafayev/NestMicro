namespace NestAuth.API.Abstractions;

public interface IAuthService
{
    Task<ResponseDto> RegisterAsync(RegisterRequest request);

    Task<ResponseDto> LoginAsync(LoginRequest request);

    Task<ResponseDto> VerifyEmailAsync(string userId, string token, string email);

    Task<ResponseDto> ForgotPasswordAsync(ForgotPasswordRequest request);

    Task<ResponseDto> ResetPasswordAsync(ResetPasswordRequest request);

    Task<ResponseDto> RefreshTokenAsync(RefreshTokenRequest request);

    Task<ResponseDto> AssignRoleAsync(AssignRoleRequest request);

    Task<ResponseDto> BlockUserAsync(string userId);

    Task<ResponseDto> UnblockUserAsync(string userId);

    Task<ResponseDto> LogoutAsync(string userId);
}