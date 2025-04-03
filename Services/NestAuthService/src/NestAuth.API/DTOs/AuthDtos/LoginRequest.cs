namespace NestAuth.API.DTOs.AuthDtos;

public class LoginRequest
{
    public string EmailOrUsername { get; set; }

    public string Password { get; set; }

    public LoginRequest()
    {
        EmailOrUsername = string.Empty;
        Password = string.Empty;
    }
}