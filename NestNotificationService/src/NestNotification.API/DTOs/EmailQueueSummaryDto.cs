namespace NestNotification.API.DTOs;

public class EmailQueueSummaryDto
{
    public int PendingCount { get; set; }
    public int FailedCount { get; set; }
    public int SuccessCount { get; set; }
    public int HighPriorityPendingCount { get; set; }
    public int ScheduledCount { get; set; }
    public DateTime LastProcessedTime { get; set; }
}