using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Auditlog
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public string TargetTable { get; set; } = null!;

    public int TargetId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
