using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class Mediafile
{
    public int Id { get; set; }

    public string EntityTable { get; set; } = null!;

    public int EntityId { get; set; }

    public string FileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public byte[] FileData { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Exhibit> Exhibits { get; set; } = new List<Exhibit>();

    public virtual ICollection<Museum> Museums { get; set; } = new List<Museum>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
