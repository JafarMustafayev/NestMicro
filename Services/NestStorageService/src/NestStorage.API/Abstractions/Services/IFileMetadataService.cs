namespace NestStorage.API.Abstractions.Services;

public interface IFileMetadataService
{
    public void CreateFileMetadataAsync(FileMetadata metadata);
    public void GetFileMetadataAsync(string fileId);
    public void UpdateFileMetadataAsync(string fileId, FileMetadata metadata);
    public void DeleteFileMetadataAsync(string fileId);

    public void GetFilesByUserIdAsync(string userId, int page, int pageSize);
    //public void SearchFilesAsync(FileSearchCriteria criteria);
}