using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int UserId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public int StatusId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Ticketsstatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
