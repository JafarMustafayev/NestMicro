﻿namespace NestAuth.API.DTOs.AuthDtos;

public class LogOutRequest
{
    public string UserId { get; set; }
    public string RefreshToken { get; set; }

    public LogOutRequest()
    {
        UserId = string.Empty;
        RefreshToken = string.Empty;
    }
}