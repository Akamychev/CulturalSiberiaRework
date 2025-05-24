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
        DataContext = new UserWorkerMainWindowViewModel(Service.GetDbContext(), CurrentUser.SelectedUser);
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

    private void UserProfile_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is UserWorkerMainWindowViewModel vm)
            vm.UserMenuVisibility = vm.UserMenuVisibility == Visibility.Visible ? Visibility.Collapsed
                : Visibility.Visible;
    }
}