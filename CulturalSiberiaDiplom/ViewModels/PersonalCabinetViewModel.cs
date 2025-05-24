using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.Views;
using CulturalSiberiaDiplom.Views.PersonalCabinetOperations;
using Microsoft.EntityFrameworkCore;

namespace CulturalSiberiaDiplom.ViewModels;

public class PersonalCabinetViewModel : NotifyProperty
{
    private CulturalSiberiaContext _context;
    private User _currentUser;

    public event EventHandler? AvatarUpdated;
    
    public byte[]? ImageBytes { get; set; }
    public BitmapSource AvatarImage { get; set; }
    
    public string UserInitials { get; set; }
    
    public ICommand BuyHistoryCommand { get; }
    public ICommand EditUserDataCommand { get; }
    public ICommand ActiveTicketsCommand { get; }
    public ICommand BackToMenuCommand { get; }
    public ICommand SetImageCommand { get; }
    public ICommand ChoseImageCommand { get; }
    
    public PersonalCabinetViewModel(CulturalSiberiaContext context, User user)
    {
        _context = context;
        _currentUser = user;

        LoadData();
        
        BuyHistoryCommand = new RelayCommand(() =>
        {
            var buyHistoryWithTicketsUser = 
                _context.Users.Include(u => u.Tickets).FirstOrDefault(u => u.Id == _currentUser.Id);

            if (buyHistoryWithTicketsUser != null)
            {
                var buyHistoryWindow = new BuyHistoryWindow(_context, buyHistoryWithTicketsUser);
                WindowShowDialog(buyHistoryWindow);
            }
        });
        EditUserDataCommand = new RelayCommand(() =>
        {
            var editUserDataWindow = new EditUserDataWindow(_context, _currentUser);

            ((EditUserDataViewModel)editUserDataWindow.DataContext).UpdatedUserData += (s, e) =>
            {
                RefreshUserData();
            };
            
            WindowShowDialog(editUserDataWindow);
        });
        ActiveTicketsCommand = new RelayCommand(() =>
        {
            var activeTicketsWithTicketsUser = 
                _context.Users.Include(u => u.Tickets).FirstOrDefault(u => u.Id == _currentUser.Id);

            if (activeTicketsWithTicketsUser != null)
            {
                var activeTicketsWindow = new UserActiveTicketsWindow(_context, activeTicketsWithTicketsUser);
                WindowShowDialog(activeTicketsWindow);
            }  
        });
        BackToMenuCommand = new RelayCommand(() => OpenNewWindowAndCloseCurrent.OpenWindow(new UserWorkerMainWindow()));
        SetImageCommand = new RelayCommand(() => SetAndSaveImage(ImageBytes));
        ChoseImageCommand = new RelayCommand(OnChoseImage);
    }

    private void WindowShowDialog(Window window)
    {
        window.ShowDialog();
    }
    
    private void OnChoseImage()
    {
        var imageBytes = ImageService.ChooseImage();

        ImageBytes = imageBytes ?? null;

        if (imageBytes != null)
        {
            SetAndSaveImage(imageBytes);
            AvatarUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
    
    private void SetAndSaveImage(byte[]? imageBytes)
    {
        if (imageBytes == null) return;

        ImageBytes = imageBytes;
        AvatarImage = ImageService.SetImage(imageBytes);
        OnPropertyChanged(nameof(AvatarImage));
        SaveAvatarAsync();
    }

    private async void SaveAvatarAsync()
    {
        try
        {
            if (ImageBytes?.Length > 0)
            {
                var mediaFileId = await MediaFileService.SaveMediaFileAsync(
                    "Users",
                    _currentUser.Id,
                    $"user_{_currentUser.Id}.png",
                    "image/png",
                    ImageBytes,
                    DateTime.Now,
                    _context);

                if (mediaFileId.HasValue)
                    _currentUser.AvatarMediaId = mediaFileId.Value;

                _context.Users.Update(_currentUser);
                await _context.SaveChangesAsync();

                MessageService.ShowSuccess("Аватар обновлен");
                AvatarUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при обновлении аватара пользователя: " + ex.Message);
            
            MessageService.ShowError("Ошибка при обновлении аватара, повторите попытку позднее");
        }
    }

    private void LoadData()
    {
        AvatarImage = _currentUser.AvatarMediaId.HasValue ?
            ImageService.GetImageById(_currentUser.AvatarMediaId.Value, _context) : ImageService.GetImageById(-1, _context);
        UserInitials = $"Добро пожаловать {_currentUser.LastName} {_currentUser.FirstName} {_currentUser.MiddleName ?? " "}";
    }

    private void RefreshUserData()
    {
        var freshUser = _context.Users.Find(_currentUser.Id);
        
        if (freshUser == null) return;

        _currentUser = freshUser;
        LoadData();
        OnPropertyChanged(nameof(UserInitials));
    }
}