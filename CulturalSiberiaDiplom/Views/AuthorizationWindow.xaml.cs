using System.Windows;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views;

public partial class AuthorizationWindow : Window
{
    public AuthorizationWindow()
    {
        InitializeComponent();
        DataContext = new AuthorizationViewModel();
    }
}