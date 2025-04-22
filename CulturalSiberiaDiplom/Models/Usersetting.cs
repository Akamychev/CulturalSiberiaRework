using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Usersetting
{
    public int UserId { get; set; }

    public int ThemeId { get; set; }

    public int LanguageId { get; set; }

    public virtual Languagestype Language { get; set; } = null!;

    public virtual Themetype Theme { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
