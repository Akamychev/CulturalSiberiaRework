using System;

namespace CulturalSiberiaDiplom.ViewModels;

public class TicketHistoryItemDto
{
    public string TargetName { get; set; } = null!;
    public decimal? Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime? EventStartDate { get; set; }
    public string Type { get; set; } = null!;
}