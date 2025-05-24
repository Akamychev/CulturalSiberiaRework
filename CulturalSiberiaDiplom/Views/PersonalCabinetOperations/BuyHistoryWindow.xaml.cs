using System.Windows;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views.PersonalCabinetOperations;

public partial class BuyHistoryWindow : Window
{
    public BuyHistoryWindow(CulturalSiberiaContext context, User user)
    {
        InitializeComponent();
        DataContext = new BuyHistoryViewModel(context, user);
    }
}