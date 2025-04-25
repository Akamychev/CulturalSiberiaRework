using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CulturalSiberiaDiplom.Models;

public partial class CulturalSiberiaContext : DbContext
{
    public CulturalSiberiaContext()
    {
    }

    public CulturalSiberiaContext(DbContextOptions<CulturalSiberiaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditlog> Auditlogs { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Eventstype> Eventstypes { get; set; }

    public virtual DbSet<Exhibitstatus> Exhibitstatuses { get; set; }

    public virtual DbSet<Languagestype> Languagestypes { get; set; }

    public virtual DbSet<Medium> Media { get; set; }

    public virtual DbSet<Museum> Museums { get; set; }

    public virtual DbSet<Museumexhibit> Museumexhibits { get; set; }

    public virtual DbSet<Museumstatus> Museumstatuses { get; set; }

    public virtual DbSet<Museumtype> Museumtypes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Originalitystatus> Originalitystatuses { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Themetype> Themetypes { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Ticketsstatus> Ticketsstatuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Usersetting> Usersettings { get; set; }

    public virtual DbSet<Usersposition> Userspositions { get; set; }

    public virtual DbSet<Usersstatus> Usersstatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Cultural_Siberia;Username=postgres;Password=62......w");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("auditlog_pkey");

            entity.ToTable("auditlog", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(255)
                .HasColumnName("action");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.TargetId).HasColumnName("target_id");
            entity.Property(e => e.TargetTable)
                .HasMaxLength(255)
                .HasColumnName("target_table");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Auditlogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("auditlog_user_id_fkey");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("events_pkey");

            entity.ToTable("events", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_date");
            entity.Property(e => e.ImageMediaId).HasColumnName("image_media_id");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("events_created_by_fkey");

            entity.HasOne(d => d.ImageMedia).WithMany(p => p.Events)
                .HasForeignKey(d => d.ImageMediaId)
                .HasConstraintName("events_image_media_id_fkey");

            entity.HasOne(d => d.Type).WithMany(p => p.Events)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("events_type_id_fkey");

            entity.HasMany(d => d.Museums).WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "Eventsinmuseum",
                    r => r.HasOne<Museum>().WithMany()
                        .HasForeignKey("MuseumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("eventsinmuseum_museum_id_fkey"),
                    l => l.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("eventsinmuseum_event_id_fkey"),
                    j =>
                    {
                        j.HasKey("EventId", "MuseumId").HasName("eventsinmuseum_pkey");
                        j.ToTable("eventsinmuseum", "Part1");
                        j.IndexerProperty<int>("EventId").HasColumnName("event_id");
                        j.IndexerProperty<int>("MuseumId").HasColumnName("museum_id");
                    });
        });

        modelBuilder.Entity<Eventstype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("eventstypes_pkey");

            entity.ToTable("eventstypes", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TypeName)
                .HasMaxLength(255)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<Exhibitstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("exhibitstatus_pkey");

            entity.ToTable("exhibitstatus", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(255)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<Languagestype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("languagestypes_pkey");

            entity.ToTable("languagestypes", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LanguagesName)
                .HasMaxLength(255)
                .HasColumnName("languages_name");
        });

        modelBuilder.Entity<Medium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("media_pkey");

            entity.ToTable("media", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.EntityTable)
                .HasMaxLength(255)
                .HasColumnName("entity_table");
            entity.Property(e => e.FileUrl)
                .HasMaxLength(255)
                .HasColumnName("file_url");
        });

        modelBuilder.Entity<Museum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("museum_pkey");

            entity.ToTable("museum", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Architects)
                .HasMaxLength(255)
                .HasColumnName("architects");
            entity.Property(e => e.DateOfFoundation).HasColumnName("date_of_foundation");
            entity.Property(e => e.EndWorkingTime).HasColumnName("end_working_time");
            entity.Property(e => e.ImageMediaId).HasColumnName("image_media_id");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.StartWorkingTime).HasColumnName("start_working_time");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.ImageMedia).WithMany(p => p.Museums)
                .HasForeignKey(d => d.ImageMediaId)
                .HasConstraintName("museum_image_media_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Museums)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("museum_status_id_fkey");

            entity.HasOne(d => d.Type).WithMany(p => p.Museums)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("museum_type_id_fkey");

            entity.HasMany(d => d.MuseumExhibits).WithMany(p => p.Museums)
                .UsingEntity<Dictionary<string, object>>(
                    "MuseumExhibit1",
                    r => r.HasOne<Museumexhibit>().WithMany()
                        .HasForeignKey("MuseumExhibitId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("museum_exhibits_museum_exhibit_id_fkey"),
                    l => l.HasOne<Museum>().WithMany()
                        .HasForeignKey("MuseumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("museum_exhibits_museum_id_fkey"),
                    j =>
                    {
                        j.HasKey("MuseumId", "MuseumExhibitId").HasName("museum_exhibits_pkey");
                        j.ToTable("museum_exhibits", "Part1");
                        j.IndexerProperty<int>("MuseumId").HasColumnName("museum_id");
                        j.IndexerProperty<int>("MuseumExhibitId").HasColumnName("museum_exhibit_id");
                    });
        });

        modelBuilder.Entity<Museumexhibit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("museumexhibit_pkey");

            entity.ToTable("museumexhibit", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ImageMediaId).HasColumnName("image_media_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.OriginalityStatusId).HasColumnName("originality_status_id");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TimestampOfReceipt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timestamp_of_receipt");

            entity.HasOne(d => d.ImageMedia).WithMany(p => p.Museumexhibits)
                .HasForeignKey(d => d.ImageMediaId)
                .HasConstraintName("museumexhibit_image_media_id_fkey");

            entity.HasOne(d => d.OriginalityStatus).WithMany(p => p.Museumexhibits)
                .HasForeignKey(d => d.OriginalityStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("museumexhibit_originality_status_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Museumexhibits)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("museumexhibit_status_id_fkey");
        });

        modelBuilder.Entity<Museumstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("museumstatus_pkey");

            entity.ToTable("museumstatus", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(255)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<Museumtype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("museumtypes_pkey");

            entity.ToTable("museumtypes", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TypeName)
                .HasMaxLength(255)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notifications_pkey");

            entity.ToTable("notifications", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("is_read");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("notifications_user_id_fkey");
        });

        modelBuilder.Entity<Originalitystatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("originalitystatus_pkey");

            entity.ToTable("originalitystatus", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(255)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reviews_pkey");

            entity.ToTable("reviews", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Event).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reviews_event_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reviews_user_id_fkey");
        });

        modelBuilder.Entity<Themetype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("themetypes_pkey");

            entity.ToTable("themetypes", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TypesName)
                .HasMaxLength(255)
                .HasColumnName("types_name");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tickets_pkey");

            entity.ToTable("tickets", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.PurchaseDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("purchase_date");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Event).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tickets_event_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tickets_status_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tickets_user_id_fkey");
        });

        modelBuilder.Entity<Ticketsstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ticketsstatus_pkey");

            entity.ToTable("ticketsstatus", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(255)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users", "Part1");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Phone, "users_phone_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AvatarMediaId).HasColumnName("avatar_media_id");
            entity.Property(e => e.Balance)
                .HasDefaultValueSql("0")
                .HasColumnType("money")
                .HasColumnName("balance");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.LastVisit).HasColumnName("last_visit");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(255)
                .HasColumnName("middle_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");

            entity.HasOne(d => d.AvatarMedia).WithMany(p => p.Users)
                .HasForeignKey(d => d.AvatarMediaId)
                .HasConstraintName("users_avatar_media_id_fkey");

            entity.HasOne(d => d.Position).WithMany(p => p.Users)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_position_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_status_id_fkey");
        });

        modelBuilder.Entity<Usersetting>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("usersettings_pkey");

            entity.ToTable("usersettings", "Part1");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.ThemeId).HasColumnName("theme_id");

            entity.HasOne(d => d.Language).WithMany(p => p.Usersettings)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usersettings_language_id_fkey");

            entity.HasOne(d => d.Theme).WithMany(p => p.Usersettings)
                .HasForeignKey(d => d.ThemeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usersettings_theme_id_fkey");

            entity.HasOne(d => d.User).WithOne(p => p.Usersetting)
                .HasForeignKey<Usersetting>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usersettings_user_id_fkey");
        });

        modelBuilder.Entity<Usersposition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("userspositions_pkey");

            entity.ToTable("userspositions", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PositionName)
                .HasMaxLength(255)
                .HasColumnName("position_name");
        });

        modelBuilder.Entity<Usersstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usersstatus_pkey");

            entity.ToTable("usersstatus", "Part1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(255)
                .HasColumnName("status_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
