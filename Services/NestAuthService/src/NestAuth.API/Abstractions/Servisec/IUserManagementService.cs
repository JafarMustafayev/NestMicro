namespace NestAuth.API.Abstractions.Servisec;

public interface IUserManagementService
{
    Task<ResponseDto> VerifyEmailAsync(string userId, string token, string email);
    Task<ResponseDto> BlockUserAsync(string userId);
    Task<ResponseDto> UnblockUserAsync(string userId);
}