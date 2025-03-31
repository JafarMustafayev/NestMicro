namespace NestAuth.API.Services;

public class PasswordManagementService : IPasswordManagementService
{
    public async Task<ResponseDto> ForgotPasswordAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDto> ResetPasswordAsync(ResetPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDto> ChangePasswordAsync(ChangePasswordRequest request)
    {
        throw new NotImplementedException();
    }
}