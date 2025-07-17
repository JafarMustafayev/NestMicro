namespace NestStorage.API.Abstractions.Services;

public interface ISyncMonitoringService
{
    public void MonitorSyncStatusAsync(string fileId);
    public void GetSyncStatisticsAsync();
    public void GetFailedSyncsAsync();
    public void RetrySyncAsync(string fileId);
}