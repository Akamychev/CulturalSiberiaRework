using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Museumtype
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Museum> Museums { get; set; } = new List<Museum>();
}
