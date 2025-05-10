using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Eventsstatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
