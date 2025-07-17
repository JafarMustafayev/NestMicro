namespace NestStorage.API.Entities;

public class FileMetadata : BaseEntityId
{
    public string FileId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }

    public File File { get; set; }

    public FileMetadata()
    {
        FileId = string.Empty;
        Key = string.Empty;
        Value = string.Empty;
    }
}