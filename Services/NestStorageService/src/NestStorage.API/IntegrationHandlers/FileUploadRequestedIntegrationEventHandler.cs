namespace NestStorage.API.IntegrationHandlers;

public class FileUploadRequestedIntegrationEventHandler : IIntegrationEventHandler<FileUploadRequestedIntegrationEvent>
{
    public async Task Handle(FileUploadRequestedIntegrationEvent @event)
    {
        await Task.Run(() => Console.WriteLine(@event.OutputFileName));
    }
}