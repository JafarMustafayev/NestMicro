namespace NestStorage.API.Entities;

public class File : BaseEntity
{
    public string MimeTypeId { get; set; }
    public string FileName { get; set; } // file esil adi 
    public string StorageName { get; set; } // faylin serverdeki adi 
    public string RelativePath { get; set; }
    public long Size { get; set; }
    public bool IsPublic { get; set; }
    public string StorageBucketId { get; set; } // saxlanildigi folder adi 
    public FileCategory FileCategory { get; set; }
    public string RelatedEntityId { get; set; }
    public string RelatedEntityType { get; set; }
    public string FromIp { get; set; }
    public string FullPath => Path.Combine(StorageBucket.BasePath, StorageBucket.Name, RelativePath); // bas folder daxil olmaqla tam yolu 
    public StorageBucket StorageBucket { get; set; }

    public ICollection<FileMetadata>? MetaData { get; set; }
    public MimeType MimeType { get; set; }

    public File()
    {
        FromIp = string.Empty;
        FileName = string.Empty;
        StorageName = string.Empty;
        RelativePath = string.Empty;
        Size = long.MinValue;
        StorageBucketId = string.Empty;
        FileCategory = FileCategory.None;
        RelatedEntityId = string.Empty;
        RelatedEntityType = string.Empty;
        IsPublic = true;
    }
}