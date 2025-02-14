namespace Nest.Shared.Events;

public record ProductCreatedEvent : IEvent
{
    public string? ProductId { get; set; }

    public string? CreatedUserId { get; set; }

    public int StockCount { get; set; }

    //public string? MainImage { get; set; }

    // public ICollection<IFormFile>? ProductFiles { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CriticalStockCount { get; set; }
}