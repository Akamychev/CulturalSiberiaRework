using System.Windows;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views.PersonalCabinetOperations;

public partial class UserActiveTicketsWindow : Window
{
    public UserActiveTicketsWindow(CulturalSiberiaContext context, User user)
    {
        InitializeComponent();
        DataContext = new ActiveTicketsViewModel(context, user);
    }
}