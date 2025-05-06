using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using Microsoft.EntityFrameworkCore;

namespace CulturalSiberiaDiplom.ViewModels;

public class MuseumDetailsViewModel : NotifyProperty
{
    private readonly CulturalSiberiaContext _context;
    public string Title { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string? Type { get; set; }
    public string? Price { get; set; }
    public BitmapSource PreviewImage { get; set; }
    
    private readonly User _currentUser;
    public User CurrentUser => _currentUser;

    public ObservableCollection<ExhibitItemViewModel> Exhibits { get; set; }
    
    private Exhibit? _selectedExhibit;
    public Exhibit? SelectedExhibit
    {
        get => _selectedExhibit;
        set
        {
            _selectedExhibit = value;
            OnPropertyChanged(nameof(SelectedExhibit));
        }
    }

    public MuseumDetailsViewModel(Museum museum, CulturalSiberiaContext context, User user)
    {
        _currentUser = user;
        _context = context;
        
        Title = museum.Name;
        Location = museum.Location;
        Description = museum.Description ?? "Отсутствует";
        StartDate = museum.StartWorkingTime.ToString("t");
        EndDate = museum.EndWorkingTime.ToString("t");
        Type = museum.Type.TypeName;
        Price = museum.Price?.ToString() ?? "Не указана";
        PreviewImage = museum.ImageMediaId.HasValue ?
            ImageService.GetImageById(museum.ImageMediaId.Value, _context) : ImageService.GetImageById(-1, _context);

        Exhibits = new ObservableCollection<ExhibitItemViewModel>(LoadExhibitsData(museum));
    }

    private IEnumerable<ExhibitItemViewModel> LoadExhibitsData(Museum museum)
    {
        return museum.MuseumExhibits.Select(exhibit => new ExhibitItemViewModel(exhibit, _context));
    }
}