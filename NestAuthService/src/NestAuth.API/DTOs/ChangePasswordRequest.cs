﻿namespace NestAuth.API.DTOs;

public class ChangePasswordRequest
{
    public string UserId { get; set; }

    public string CurrentPassword { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }
}