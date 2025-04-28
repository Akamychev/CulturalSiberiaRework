using System.Windows;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views;

public partial class UserWorkerMainWindow : Window
{
    public UserWorkerMainWindow()
    {
        InitializeComponent();
        DataContext = new UserWorkerMainWindowViewModel();
    }
}