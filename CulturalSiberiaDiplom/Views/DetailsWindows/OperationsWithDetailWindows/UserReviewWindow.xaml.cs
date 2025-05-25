using System.Windows;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views.DetailsWindows.OperationsWithDetailWindows;

public partial class UserReviewWindow : Window
{
    public UserReviewWindow(User user, Event @event, CulturalSiberiaContext context)
    {
        InitializeComponent();
        DataContext = new UserReviewViewModel(user, @event, context, this);
    }
}