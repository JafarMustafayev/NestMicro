namespace NestStorage.API.Entities;

public class StorageBucket : BaseEntity
{
    public string Name { get; set; }
    public StorageProvider Provider { get; set; }
    public string BasePath { get; set; }
    public bool IsPublic { get; set; }

    public virtual ICollection<File>? Files { get; set; }

    public StorageBucket()
    {
        Name = string.Empty;
        Provider = StorageProvider.Local;
        IsPublic = true;
        BasePath = string.Empty;
    }
}