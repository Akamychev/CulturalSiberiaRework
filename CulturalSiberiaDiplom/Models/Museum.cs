using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Museum
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? DateOfFoundation { get; set; }

    public decimal? Price { get; set; }

    public int TypeId { get; set; }

    public string? Architects { get; set; }

    public TimeOnly StartWorkingTime { get; set; }

    public TimeOnly EndWorkingTime { get; set; }

    public int StatusId { get; set; }

    public int? ImageMediaId { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Mediafile? ImageMedia { get; set; }

    public virtual Museumstatus Status { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Museumtype Type { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Exhibit> MuseumExhibits { get; set; } = new List<Exhibit>();
}
