using System.Windows.Media.Imaging;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;

namespace CulturalSiberiaDiplom.ViewModels;

public class ExhibitItemViewModel
{
    public Exhibit Exhibit { get; set; }
    public string Name { get; set; }
    public BitmapSource PreviewImage { get; set; }

    public ExhibitItemViewModel(Exhibit exhibit, CulturalSiberiaContext context)
    {
        Exhibit = exhibit;
        Name = exhibit.Name;
        PreviewImage = exhibit.ImageMediaId.HasValue ?
            ImageService.GetImageById(exhibit.ImageMediaId.Value, context) : ImageService.GetImageById(-1, context);
    }
}