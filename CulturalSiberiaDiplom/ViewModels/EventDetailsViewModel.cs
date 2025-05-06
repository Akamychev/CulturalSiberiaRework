using System.Windows.Media.Imaging;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;

namespace CulturalSiberiaDiplom.ViewModels;

public class EventDetailsViewModel : NotifyProperty
{
    private readonly CulturalSiberiaContext _context;
    public string Title { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string? AvailableSeats { get; set; }
    public string? Price { get; set; }
    public BitmapSource PreviewImage { get; set; }
    
    private readonly User _currentUser;
    public User CurrentUser => _currentUser;

    public EventDetailsViewModel(Event @event, CulturalSiberiaContext context, User user)
    {
        _currentUser = user;
        _context = context;
        Title = @event.Title;
        Location = @event.Location ?? "Неизвестно";
        Description = @event.Description ?? "Отсутствует";
        StartDate = @event.StartDate.ToString("dd.MM.yyyy HH:mm");
        EndDate = @event.EndDate.ToString("dd.MM.yyyy HH:mm");
        AvailableSeats = @event.Capacity?.ToString() ?? "Неизвестно";
        Price = @event.Price?.ToString() ?? "Не указана";
        PreviewImage = @event.ImageMediaId.HasValue ?
            ImageService.GetImageById(@event.ImageMediaId.Value, _context) : ImageService.GetImageById(-1, _context);
    }
}