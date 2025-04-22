using System.Windows;

namespace CulturalSiberiaDiplom.Services;

public class OpenNewWindowAndCloseCurrent
{
    public static void OpenWindow(Window window)
    {
        window.Show();
        Application.Current.MainWindow?.Close();
        Application.Current.MainWindow = window;
    }
}