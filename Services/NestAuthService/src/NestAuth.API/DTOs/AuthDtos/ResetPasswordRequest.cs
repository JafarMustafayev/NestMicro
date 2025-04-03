namespace NestAuth.API.DTOs;

public class ResetPasswordRequest
{
    public string Email { get; set; }

    public string Token { get; set; }

    public string NewPassword { get; set; }

    public string ConfirmPassword { get; set; }

    public ResetPasswordRequest()
    {
        Email = string.Empty;
        Token = string.Empty;
        NewPassword = string.Empty;
        ConfirmPassword = string.Empty;
    }
}