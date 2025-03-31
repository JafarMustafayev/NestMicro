namespace NestAuth.API.Abstractions.Servisec;

public interface IAuthenticationService
{
    Task<ResponseDto> RegisterAsync(RegisterRequest request);

    Task<ResponseDto> LoginAsync(LoginRequest request);

    Task<ResponseDto> RefreshTokenAsync(RefreshTokenRequest request);

    Task<ResponseDto> LogoutAsync(LogOutRequest request);
}