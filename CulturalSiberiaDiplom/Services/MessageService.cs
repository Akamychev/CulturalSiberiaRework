using System.Windows;

namespace CulturalSiberiaDiplom.Services;

public static class MessageService
{
    public static void ShowError(string message)
    {
        MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public static void ShowSuccess(string message)
    {
        MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}