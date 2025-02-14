namespace Nest.Shared.Entities;

public class BaseEntity
{
    public string Id { get; set; }

    public string WhoCreated { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public EntityStatus IsActive { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
        WhoCreated = string.Empty;
        DeletedBy = string.Empty;
        LastModifiedBy = string.Empty;
        IsActive = EntityStatus.Active;
        LastModifiedAt = null;
        DeletedAt = null;
    }
}