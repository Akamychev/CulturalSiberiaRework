using System.Windows;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views.PersonalCabinetOperations;

public partial class EditUserDataWindow : Window
{
    public EditUserDataWindow(CulturalSiberiaContext context, User user)
    {
        InitializeComponent();
        DataContext = new EditUserDataViewModel(context, user, this);
    }
}