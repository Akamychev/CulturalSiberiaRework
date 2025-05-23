﻿using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Usersstatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
