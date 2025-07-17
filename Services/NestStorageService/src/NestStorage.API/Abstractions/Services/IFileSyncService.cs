namespace NestStorage.API.Abstractions.Services;

public interface IFileSyncService
{
    public void SyncFileToCloudAsync(string fileId);
    public void SyncFileFromCloudAsync(string fileId);
    public void SyncAllPendingFilesAsync();
    public void GetPendingSyncFilesAsync();
    public void MarkSyncCompleteAsync(string fileId);
    public void HandleSyncFailureAsync(string fileId, string error);
}