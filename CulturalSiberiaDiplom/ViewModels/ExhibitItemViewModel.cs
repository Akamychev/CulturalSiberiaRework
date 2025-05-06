using System.Windows.Media.Imaging;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;

namespace CulturalSiberiaDiplom.ViewModels;

public class ExhibitItemViewModel
{
    public string Name { get; set; }
    public BitmapSource PreviewImage { get; set; }

    public ExhibitItemViewModel(Exhibit exhibit, CulturalSiberiaContext context)
    {
        Name = exhibit.Name;

        if (exhibit.ImageMediaId.HasValue)
            PreviewImage = ImageService.GetImageById(exhibit.ImageMediaId.Value, context);
        else
            PreviewImage = ImageService.GetImageById(-1, context);
    }
}