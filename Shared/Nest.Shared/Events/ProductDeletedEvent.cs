namespace Nest.Shared.Events;

public class ProductDeletedEvent : IEvent
{
    public string? ProductId { get; set; }

    public string? DeletedUserId { get; set; }

    public DateTime DeletedDate { get; set; }
}