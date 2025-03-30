namespace NestAuth.API.DTOs;

public class UserSessionDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string? DeviceInfo { get; set; }
    public string? LocationInfo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }

    public UserSessionDto()
    {
        Id = string.Empty;
        UserId = string.Empty;
        DeviceInfo = string.Empty;
        CreatedAt = DateTime.MinValue;
        ExpiresAt = DateTime.MinValue;
    }
}