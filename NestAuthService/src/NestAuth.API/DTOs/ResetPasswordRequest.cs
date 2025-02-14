namespace NestAuth.API.DTOs;

public class ResetPasswordRequest
{
    public string Email { get; set; }

    public string ResetToken { get; set; }

    public string NewPassword { get; set; }

    public string ConfirmPassword { get; set; }
}