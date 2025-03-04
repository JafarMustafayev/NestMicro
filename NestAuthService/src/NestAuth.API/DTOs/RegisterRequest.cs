﻿namespace NestAuth.API.DTOs;

public record RegisterRequest
{
    public string UserName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public RegisterRequest()
    {
        UserName = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
    }
}