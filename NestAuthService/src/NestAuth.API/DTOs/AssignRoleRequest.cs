namespace NestAuth.API.DTOs;

public class AssignRoleRequest
{
    public string UserId { get; set; }
    public string RoleId { get; set; }
}