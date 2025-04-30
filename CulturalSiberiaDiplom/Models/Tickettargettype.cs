using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Tickettargettype
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
