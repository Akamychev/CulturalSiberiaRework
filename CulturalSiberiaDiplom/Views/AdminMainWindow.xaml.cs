using System.Windows;
using CulturalSiberiaDiplom.Services;

namespace CulturalSiberiaDiplom.Views;

public partial class AdminMainWindow : Window
{
    public AdminMainWindow()
    {
        InitializeComponent();
        MessageBox.Show($"Авторизация под пользователем {CurrentUser.SelectedUser?.Phone}", 
            "Тест", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}