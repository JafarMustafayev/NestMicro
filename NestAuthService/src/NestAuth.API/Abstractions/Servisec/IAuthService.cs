namespace NestAuth.API.Abstractions.Servisec;

public interface IAuthService
{
    Task<ResponseDto> RegisterAsync(RegisterRequest request);

    Task<ResponseDto> LoginAsync(LoginRequest request);

    Task<ResponseDto> VerifyEmailAsync(string userId, string token, string email);

    Task<ResponseDto> ForgotPasswordAsync(string email);

    Task<ResponseDto> ResetPasswordAsync(ResetPasswordRequest request);

    Task<ResponseDto> RefreshTokenAsync(RefreshTokenRequest request);

    Task<ResponseDto> ChangePasswordAsync(ChangePasswordRequest request);

    Task<ResponseDto> BlockUserAsync(string userId);

    Task<ResponseDto> UnblockUserAsync(string userId);

    Task<ResponseDto> LogoutAsync(LogOutRequest request);
}