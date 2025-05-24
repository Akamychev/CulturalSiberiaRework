using System;
using CulturalSiberiaDiplom.ViewModels;
using CulturalSiberiaDiplom.Views;

namespace CulturalSiberiaDiplom;

public partial class App
{
    public App()
    {
        var view = new AuthorizationWindow()
        {
            DataContext = Activator.CreateInstance<AuthorizationViewModel>()
        };

        view.Show();
    }
}