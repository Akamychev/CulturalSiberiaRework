using System.Windows;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views;

public partial class RegistrationWindow : Window
{
    public RegistrationWindow()
    {
        InitializeComponent();
        DataContext = new RegistrationViewModel();
    }
}