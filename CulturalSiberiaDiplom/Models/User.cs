using System;
using System.Collections.Generic;

namespace CulturalSiberiaDiplom.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public int StatusId { get; set; }

    public int PositionId { get; set; }

    public int? AvatarMediaId { get; set; }

    public decimal? Balance { get; set; }

    public DateOnly? LastVisit { get; set; }

    public virtual ICollection<Auditlog> Auditlogs { get; set; } = new List<Auditlog>();

    public virtual Mediafile? AvatarMedia { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Usersposition Position { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Usersstatus Status { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Usersetting? Usersetting { get; set; }
}
