using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Medium
{
    public int Id { get; set; }

    public string EntityTable { get; set; } = null!;

    public int EntityId { get; set; }

    public string FileUrl { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Museumexhibit> Museumexhibits { get; set; } = new List<Museumexhibit>();

    public virtual ICollection<Museum> Museums { get; set; } = new List<Museum>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
