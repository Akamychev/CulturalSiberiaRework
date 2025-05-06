using System.Windows;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views.DetailsWindows;

public partial class EventDetailsWindow : Window
{
    public EventDetailsWindow(Event @event)
    {
        InitializeComponent();
        DataContext = new EventDetailsViewModel(@event, Service.GetDbContext(), CurrentUser.SelectedUser);
    }
}