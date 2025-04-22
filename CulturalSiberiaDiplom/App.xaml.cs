using System;
using CulturalSiberiaDiplom.ViewModels;
using CulturalSiberiaDiplom.Views;

namespace CulturalSiberiaDiplom;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    /// <summary>
    /// Application Entry for CulturalSiberiaDiplom
    /// </summary>
    public App()
    {
        var view = new AuthorizationWindow
        {
            DataContext = Activator.CreateInstance<AuthorizationViewModel>()
        };

        view.Show();
    }
}