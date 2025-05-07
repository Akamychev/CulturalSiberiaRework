using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views;

public partial class MuseumDetailsWindow : Window
{
    public MuseumDetailsWindow(Museum museum)
    {
        InitializeComponent();

        var currentUser = CurrentUser.SelectedUser;
        var context = Service.GetDbContext();
        
        DataContext = new MuseumDetailsViewModel(museum, context, currentUser);
    }

    private void Exhibit_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is MuseumDetailsViewModel vm)
            vm.OpenExhibitDetailsCommand.Execute(vm.SelectedExhibit);
    }
}