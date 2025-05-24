using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int TicketTargetTypeId { get; set; }

    public int TicketTargetId { get; set; }

    public int UserId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public int StatusId { get; set; }

    public int? EventId { get; set; }

    public int? MuseumId { get; set; }

    public virtual Event? Event { get; set; }

    public virtual Museum? Museum { get; set; }

    public virtual Ticketsstatus Status { get; set; } = null!;

    public virtual Tickettargettype TicketTargetType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
