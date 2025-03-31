namespace NestAuth.API.Abstractions.Servisec;

public interface IPasswordManagementService
{
    Task<ResponseDto> ForgotPasswordAsync(string email);
    Task<ResponseDto> ResetPasswordAsync(ResetPasswordRequest request);
    Task<ResponseDto> ChangePasswordAsync(ChangePasswordRequest request);
}