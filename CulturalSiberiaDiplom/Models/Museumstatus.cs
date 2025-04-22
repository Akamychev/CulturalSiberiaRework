using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Museumstatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Museum> Museums { get; set; } = new List<Museum>();
}
