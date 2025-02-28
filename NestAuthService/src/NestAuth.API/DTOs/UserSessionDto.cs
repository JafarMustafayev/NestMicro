namespace NestAuth.API.DTOs;

public class UserSessionDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Device { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
}