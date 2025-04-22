using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Languagestype
{
    public int Id { get; set; }

    public string LanguagesName { get; set; } = null!;

    public virtual ICollection<Usersetting> Usersettings { get; set; } = new List<Usersetting>();
}
