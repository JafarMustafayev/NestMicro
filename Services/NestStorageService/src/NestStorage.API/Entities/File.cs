namespace NestStorage.API.Entities;

public class File : BaseEntity
{
    public string FileName { get; set; } // file esil adi 
    public string StorageName { get; set; } // faylin serverdeki adi 
    public string RelativePath { get; set; }
    public long Size { get; set; }
    public string MimeType { get; set; }
    public string StorageBucketId { get; set; } // saxlanildigi folder adi 
    public FileCategory FileCategory { get; set; }
    public string RelatedEntityId { get; set; }
    public string RelatedEntityType { get; set; }
    public string FullPath => Path.Combine(StorageBucket.BasePath, StorageBucket.Name, RelativePath); // bas folder daxil olmaqla tam yolu 
    public virtual required StorageBucket StorageBucket { get; set; }

    public virtual ICollection<FileMetaData>? MetaData { get; set; }

    public File()
    {
        FileName = string.Empty;
        StorageName = string.Empty;
        RelativePath = string.Empty;
        MimeType = string.Empty;
        Size = long.MinValue;
        StorageBucketId = string.Empty;
        FileCategory = FileCategory.None;
        RelatedEntityId = string.Empty;
        RelatedEntityType = string.Empty;
    }
}