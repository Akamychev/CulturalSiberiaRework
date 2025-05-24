using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;

namespace CulturalSiberiaDiplom.ViewModels;

public class EditUserDataViewModel : NotifyProperty
{
    private CulturalSiberiaContext _context;
    private User _currentUser;
    private Window _window;

    public event EventHandler? UpdatedUserData;
    
    public string? PhoneNumberProperty { get; set; }
    public string EmailProperty { get; set; }
    public string FirstNameProperty { get; set; }
    public string LastNameProperty { get; set; }
    public string? MiddleNameProperty { get; set; }

    private bool _isInEditMode;
    public bool IsInEditMode
    {
        get => _isInEditMode;
        set
        {
            _isInEditMode = value;
            OnPropertyChanged(nameof(IsInEditMode));
        }
    }
    
    public ICommand EditModeUserDataCommand { get; }

    public EditUserDataViewModel(CulturalSiberiaContext context, User user, Window window)
    {
        _context = context;
        _currentUser = user;
        _window = window;

        LoadData();
        
        EditModeUserDataCommand = new RelayCommand(OnToggleEditModeOrUpdateChanges);
    }

    private async void UpdateUserDataAsync()
    {
        try
        {
            if (!InputValidator.ValidateEmail(EmailProperty)
                || !InputValidator.ValidateMiddleName(MiddleNameProperty)
                || !InputValidator.ValidateFirstNameAndLastName(FirstNameProperty, LastNameProperty)
                || !InputValidator.ValidatePhoneNumbers(PhoneNumberProperty))
                return;
            
            _currentUser.Phone = PhoneNumberProperty;
            _currentUser.Email = EmailProperty;
            _currentUser.FirstName = FirstNameProperty;
            _currentUser.LastName = LastNameProperty;
            _currentUser.MiddleName = MiddleNameProperty;

            _context.Users.Update(_currentUser);
            await _context.SaveChangesAsync();
            
            UpdatedUserData?.Invoke(this, EventArgs.Empty);
            MessageService.ShowSuccess("Данные успешно обновлены");

            IsInEditMode = false;
            _window.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка обновления данных пользователя: " + ex.Message);
            
            MessageService.ShowError("Ошибка при обновлении данных, повторите попытку позднее");
        }
    }
    
    private void OnToggleEditModeOrUpdateChanges()
    {
        if (IsInEditMode)
        {
            UpdateUserDataAsync();
        }
        else
            IsInEditMode = true;
    }

    private void LoadData()
    {
        PhoneNumberProperty = _currentUser.Phone;
        EmailProperty = _currentUser.Email;
        FirstNameProperty = _currentUser.FirstName;
        LastNameProperty = _currentUser.LastName;
        MiddleNameProperty = _currentUser.MiddleName;
    }
}