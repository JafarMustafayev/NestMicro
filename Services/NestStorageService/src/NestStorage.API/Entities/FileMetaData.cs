namespace NestStorage.API.Entities;

public class FileMetaData : BaseEntityId
{
    public string FileId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }

    public virtual File File { get; set; }

    public FileMetaData()
    {
        FileId = string.Empty;
        Key = string.Empty;
        Value = string.Empty;
    }
}