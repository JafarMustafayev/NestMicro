namespace NestStorage.API.Entities;

public class MimeType : BaseEntity
{
    public string MimeCategoryId { get; set; }
    public string Name { get; set; }
    public string Extension { get; set; }
    public long MaxUploadSizeInBytes { get; set; }

    public MimeCategory MimeCategory { get; set; }

    public ICollection<File>? Files { get; set; }

    public MimeType()
    {
        Extension = string.Empty;
        MimeCategoryId = string.Empty;
        Name = string.Empty;
        MaxUploadSizeInBytes = long.MinValue;
    }
}