namespace NestAuth.API.Abstractions.Servisec;

public interface IAuthenticationService
{
    Task<ResponseDto> RegisterAsync(RegisterRequest request);

    Task<ResponseDto> LoginAsync(LoginRequest request);

    Task<ResponseDto> RefreshTokenAsync(RefreshTokenRequest request);

    Task<ResponseDto> LogoutAsync(LogOutRequest request);

    // 2FA metodları
    Task<ResponseDto> EnableEmail2FAAsync(string userId);

    Task<ResponseDto> DisableEmail2FAAsync(string userId);

    Task<ResponseDto> Regenerate2FACodeAsync(string userId);

    Task<ResponseDto> Verify2FACodeAsync(Verify2FACodeRequest request);

    // Task<ResponseDto> GenerateQRCodeForAuthenticatorAppAsync(string userId);

    // Task<ResponseDto> Setup2FAWithAuthenticatorAppAsync(string userId, string verificationCode);

    //Task<ResponseDto> Generate2FARecoveryCodesAsync(string userId);
}