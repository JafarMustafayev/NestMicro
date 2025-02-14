namespace NestAuth.API.DTOs;

public record LoginRequest
{
    public string EmailOrUsername { get; set; }

    public string Password { get; set; }
}