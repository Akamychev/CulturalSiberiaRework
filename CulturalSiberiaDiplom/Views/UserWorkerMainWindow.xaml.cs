using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views;

public partial class UserWorkerMainWindow : Window
{
    public UserWorkerMainWindow()
    {
        InitializeComponent();
        DataContext = new UserWorkerMainWindowViewModel(CurrentUser.SelectedUser, Service.GetDbContext());
    }

    private void Events_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListBox listBox && DataContext is UserWorkerMainWindowViewModel vm)
            vm.OpenEventDetailsCommand.Execute(listBox.SelectedItem);
    }

    private void Museums_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListBox listBox && DataContext is UserWorkerMainWindowViewModel vm)
            vm.OpenMuseumDetailsCommand.Execute(listBox.SelectedItem);
    }
}