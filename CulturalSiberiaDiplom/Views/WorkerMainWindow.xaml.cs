using System.Windows;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views;

public partial class WorkerMainWindow : Window
{
    public WorkerMainWindow()
    {
        InitializeComponent();
        DataContext = new WorkerMainWindowViewModel();
    }
}