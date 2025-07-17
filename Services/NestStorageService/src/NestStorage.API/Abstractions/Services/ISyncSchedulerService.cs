namespace NestStorage.API.Abstractions.Services;

public interface ISyncSchedulerService
{
    public void ScheduleSyncAsync(string fileId, DateTime scheduledTime);
    public void GetScheduledSyncsAsync();
    public void CancelScheduledSyncAsync(string fileId);
    public void ProcessScheduledSyncsAsync();
}