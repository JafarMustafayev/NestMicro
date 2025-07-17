namespace Nest.Shared.Entities;

public class BaseEntityId
{
    public string Id { get; set; }

    public BaseEntityId()
    {
        Id = Guid.NewGuid().ToString();
    }
}

public class BaseEntity : BaseEntityId
{
    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public bool IsActive { get; set; }

    public BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
        CreatedBy = string.Empty;
        DeletedBy = string.Empty;
        LastModifiedBy = string.Empty;
        IsActive = true;
        LastModifiedAt = null;
        DeletedAt = null;
    }
}