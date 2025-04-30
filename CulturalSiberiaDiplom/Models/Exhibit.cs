using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Exhibit
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly? CreatedAt { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public int StatusId { get; set; }

    public int OriginalityStatusId { get; set; }

    public int? ImageMediaId { get; set; }

    public DateTime TimestampOfReceipt { get; set; }

    public virtual Mediafile? ImageMedia { get; set; }

    public virtual Originalitystatus OriginalityStatus { get; set; } = null!;

    public virtual Exhibitstatus Status { get; set; } = null!;

    public virtual ICollection<Museum> Museums { get; set; } = new List<Museum>();
}
