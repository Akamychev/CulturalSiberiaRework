using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.Views;
using Microsoft.EntityFrameworkCore;
using static CulturalSiberiaDiplom.Services.Hash;

namespace CulturalSiberiaDiplom.ViewModels;

public class RegistrationViewModel : NotifyProperty
{
    private Window _window;
    
    private string? _fNameProperty;
    public string? FNameProperty
    {
        get => _fNameProperty;
        set
        {
            if (_fNameProperty == value) return;

            _fNameProperty = value;
            OnPropertyChanged(nameof(FNameProperty));
        }
    }

    private string? _lNameProperty;
    public string? LNameProperty
    {
        get => _lNameProperty;
        set 
        {
            if (_lNameProperty == value) return;

            _lNameProperty = value;
            OnPropertyChanged(nameof(LNameProperty));
        }
    }

    private string? _mNameProperty;
    public string? MNameProperty
    {
        get => _mNameProperty;
        set 
        {
            if (_mNameProperty == value) return;

            _mNameProperty = value;
            OnPropertyChanged(nameof(MNameProperty));
        }
    }
    
    private string? _emailProperty;
    public string? EmailProperty
    {
        get => _emailProperty;
        set 
        {
            if (_emailProperty == value) return;

            _emailProperty = value;
            OnPropertyChanged(nameof(EmailProperty));
        }
    }

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
            OnPropertyChanged(nameof(PasswordProperty));
        }
    }
    
    public ICommand RegistrationCommandButton { get; set; }

    public RegistrationViewModel(Window window)
    {
        _window = window;
        
        RegistrationCommandButton = new RelayCommand(RegisterUser);
    }

    private void RegisterUser()
    {
        if (!ValidateInput())
            return;
        
        AddNewUser();
        _window.Close();
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(FNameProperty)
            || string.IsNullOrWhiteSpace(LNameProperty)
            || string.IsNullOrWhiteSpace(EmailProperty)
            || string.IsNullOrWhiteSpace(LoginProperty)
            || string.IsNullOrWhiteSpace(PasswordProperty))
        {
            MessageBox.Show("Все обязательнные поля должны быть заполнены!",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return InputValidator.ValidateFirstNameAndLastName(FNameProperty, LNameProperty) 
               && InputValidator.ValidateMiddleName(MNameProperty)
               && InputValidator.ValidateEmail(EmailProperty)
               && InputValidator.ValidateUsernameFormat(LoginProperty)
               && InputValidator.ValidateUsernameUniqueness(LoginProperty)
               && InputValidator.ValidatePasswordFormat(PasswordProperty)
               && InputValidator.ValidatePasswordLength(PasswordProperty);
    }

    private async void AddNewUser()
    {
        try
        {
            var activeStatus = await Service.GetDbContext().Usersstatuses
                .FirstOrDefaultAsync(s => s.StatusName == "Active");

            var userPosition = await Service.GetDbContext().Userspositions
                .FirstOrDefaultAsync(p => p.PositionName == "User");
            
            if (activeStatus == null || userPosition == null)
            {
                MessageBox.Show("Не удалось найти статус или роль в базе данных.", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            var user = new User
            {
                Username = LoginProperty,
                PasswordHash = HashPassword(PasswordProperty),
                CreatedAt = DateTime.Now,
                FirstName = FNameProperty,
                LastName = LNameProperty,
                MiddleName = MNameProperty,
                Email = EmailProperty,
                StatusId = activeStatus.Id,
                PositionId = userPosition.Id,
                LastVisit = DateOnly.FromDateTime(DateTime.Now.Date)
            };
            
            Service.GetDbContext().Users.Add(user);
            await Service.GetDbContext().SaveChangesAsync();

            MessageBox.Show("Регистрация прошла успешно",
                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            
            FNameProperty = null;
            LNameProperty = null;
            MNameProperty = null;
            EmailProperty = null;
            LoginProperty = null;
            PasswordProperty = null;
            
            OpenNewWindowAndCloseCurrent.OpenWindow(new AuthorizationWindow());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка добавления нового пользователя: {ex.Message}");

            MessageBox.Show("Ошибка регистрации. Попробуйте позже",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}