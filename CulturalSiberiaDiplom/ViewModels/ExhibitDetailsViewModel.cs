using System.Windows.Media.Imaging;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;

namespace CulturalSiberiaDiplom.ViewModels;

public class ExhibitDetailsViewModel : NotifyProperty
{
    private readonly CulturalSiberiaContext _context;
    
    public string Title { get; set; }
    public string Price { get; set; }
    public string Description { get; set; }
    public bool Originality { get; set; }
    public BitmapSource PreviewImage { get; set; }

    private User _currentUser;
    public User CurrentUser => _currentUser;
    
    public ExhibitDetailsViewModel(Exhibit exhibit, CulturalSiberiaContext context, User user)
    {
        _context = context;
        _currentUser = user;

        Title = exhibit.Name;
        Price = exhibit.Price?.ToString() ?? "Не указана";
        Description = exhibit.Description ?? "Отсутствует";
        Originality = exhibit.OriginalityStatus.StatusName == "Original";

        PreviewImage = exhibit.ImageMediaId.HasValue ?
            ImageService.GetImageById(exhibit.ImageMediaId.Value, _context) : ImageService.GetImageById(-1, _context);
        
    }
}