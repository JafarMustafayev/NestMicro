namespace NestAuth.API.Abstractions;

public interface IAuthService
{
    Task<ResponseDto> RegisterAsync(RegisterRequest request);

    Task<ResponseDto> LoginAsync(LoginRequest request);

    Task<ResponseDto> VerifyEmailAsync(string userId, string token);

    Task<ResponseDto> ForgotPasswordAsync(string email);

    Task<ResponseDto> ResetPasswordAsync(ResetPasswordRequest request);

    //Task<ResponseDto> ExternalLoginAsync(ExternalLoginRequest request);

    Task<ResponseDto> RefreshTokenAsync(string refreshToken);

    Task<ResponseDto> AssignRoleAsync(string userId, string role);

    Task<ResponseDto> BlockUserAsync(string userId);

    Task<ResponseDto> UnblockUserAsync(string userId);

    Task<ResponseDto> LogoutAsync(string userId);
}