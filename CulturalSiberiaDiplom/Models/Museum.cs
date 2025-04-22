using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Museum
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateOnly? DateOfFoundation { get; set; }

    public string? Architects { get; set; }

    public TimeOnly StartWorkingTime { get; set; }

    public TimeOnly EndWorkingTime { get; set; }

    public int StatusId { get; set; }

    public int? ImageMediaId { get; set; }

    public virtual Medium? ImageMedia { get; set; }

    public virtual Museumstatus Status { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Museumexhibit> MuseumExhibits { get; set; } = new List<Museumexhibit>();
}
