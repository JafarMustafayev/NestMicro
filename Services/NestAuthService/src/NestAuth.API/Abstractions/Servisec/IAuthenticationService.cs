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

    Task<ResponseDto> LoginWithEmailAuthenticator(Verify2FACodeRequest request);

    Task<ResponseDto> VerifyAuthenticator2FAAsync(string userId, string code);

    Task<ResponseDto> SetupAuthenticator2FAAsync(string userId);

    //Task<ResponseDto> Generate2FARecoveryCodesAsync(string userId);
}