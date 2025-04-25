using System.Windows;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views;

public partial class UserMainWindow : Window
{
    public UserMainWindow()
    {
        InitializeComponent();
        DataContext = new UserMainWindowViewModel();
    }
}