using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Eventstype
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
