using CulturalSiberiaDiplom.Views;

namespace CulturalSiberiaDiplom.Services;

public class LogoutUser
{
    public static void OnLogout()
    {
        CurrentUser.SelectedUser = null;
        OpenNewWindowAndCloseCurrent.OpenWindow(new AuthorizationWindow());
    }
}