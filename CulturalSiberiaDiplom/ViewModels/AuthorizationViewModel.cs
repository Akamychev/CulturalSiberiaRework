using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.Views;
using Microsoft.EntityFrameworkCore;
using static CulturalSiberiaDiplom.Services.Hash;

namespace CulturalSiberiaDiplom.ViewModels;

public class AuthorizationViewModel : NotifyProperty
{
    private string? _loginProperty;
    public string? LoginProperty
    {
        get => _loginProperty;
        set
        {
            if (_loginProperty == value) return;
            
            _loginProperty = value;
            OnPropertyChanged(nameof(LoginProperty));
        }
    }

    private string? _passwordProperty;
    public string? PasswordProperty
    {
        get => _passwordProperty;
        set
        {
            if (_passwordProperty == value) return;

            _passwordProperty = value;
            Debug.WriteLine($"AuthorizationViewModel: PasswordProperty updated to {value}");
            OnPropertyChanged(nameof(PasswordProperty));
        }
    }

    public ICommand AuthorizationCommand { get; set; }
    public ICommand GoToRegistrationCommand { get; set; }
    
    public AuthorizationViewModel()
    {
        AuthorizationCommand = new RelayCommand(Authorization);
        GoToRegistrationCommand = new RelayCommand(GoToRegistration);
    }

    private async void Authorization()
    {
        if (!ValidateInput())
            return;

        Debug.WriteLine($"Login: {LoginProperty}, Password: {PasswordProperty}");
        
        var user = await FindUserAsync();
        if (user == null)
        {
            MessageBox.Show("Пользователь не был найден, повторите попытку входа",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        OpenMainWindow(user);

        PasswordProperty = string.Empty;
    }

    private bool ValidateInput()
    {
        return InputValidator.ValidateAuthorization(LoginProperty, PasswordProperty);
    }

    private async Task<User?> FindUserAsync()
    {
        try
        {
            return await Service.GetDbContext().Users
                .Include(user => user.Position)
                .FirstOrDefaultAsync(u => u.Username == LoginProperty && u.PasswordHash == HashPassword(PasswordProperty));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при поиске пользователя: {ex.Message}");
            
            return null;
        }
    }

    private void OpenMainWindow(User user)
    {
        CurrentUser.SelectedUser = user;

        switch (user.Position.PositionName)
        {
            case "Admin":
                NavigateTo(new AdminMainWindow());
                break;
            case "User":
            case "Worker":
                NavigateTo(new UserWorkerMainWindow());
                break;
            
            default:
                MessageBox.Show("Доступ запрещен", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                break;
        }
    }

    private void GoToRegistration()
    {
        NavigateTo(new RegistrationWindow());
    }

    private void NavigateTo(Window window)
    {
        OpenNewWindowAndCloseCurrent.OpenWindow(window);
    }
}