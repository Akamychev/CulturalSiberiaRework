using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int TypeId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? Location { get; set; }

    public decimal? Price { get; set; }

    public int? Capacity { get; set; }

    public int CreatedBy { get; set; }

    public int? ImageMediaId { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Mediafile? ImageMedia { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Eventstype Type { get; set; } = null!;

    public virtual ICollection<Museum> Museums { get; set; } = new List<Museum>();
}
