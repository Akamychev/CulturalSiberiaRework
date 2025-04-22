using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Ticketsstatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
