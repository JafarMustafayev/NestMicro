namespace NestAuth.API.DTOs.AuthDtos;

public class ChangePasswordRequest
{
    public string UserId { get; set; }

    public string CurrentPassword { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public ChangePasswordRequest()
    {
        UserId = string.Empty;
        CurrentPassword = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
    }
}