using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Exhibitstatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Exhibit> Exhibits { get; set; } = new List<Exhibit>();
}
