using System.Windows;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views.DetailsWindows;

public partial class ExhibitDetailsWindow : Window
{
    public ExhibitDetailsWindow(Exhibit exhibit, CulturalSiberiaContext context, User user)
    {
        InitializeComponent();
        DataContext = new ExhibitDetailsViewModel(exhibit, context, user); 
    }
}