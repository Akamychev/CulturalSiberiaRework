using System.Windows;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views;

public partial class MuseumDetailsWindow : Window
{
    public MuseumDetailsWindow(Museum museum)
    {
        InitializeComponent();
        DataContext = new MuseumDetailsViewModel(museum, Service.GetDbContext(), CurrentUser.SelectedUser);
    }
}