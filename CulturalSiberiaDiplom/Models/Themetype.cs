using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Themetype
{
    public int Id { get; set; }

    public string TypesName { get; set; } = null!;

    public virtual ICollection<Usersetting> Usersettings { get; set; } = new List<Usersetting>();
}
