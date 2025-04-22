using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Usersposition
{
    public int Id { get; set; }

    public string PositionName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
