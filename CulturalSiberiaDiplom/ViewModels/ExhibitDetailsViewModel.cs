using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;

namespace CulturalSiberiaDiplom.ViewModels;

public class ExhibitDetailsViewModel : NotifyProperty
{
    private readonly CulturalSiberiaContext _context;
    private readonly Exhibit _exhibit;
    private readonly Window _window;

    public event EventHandler? ExhibitDeleted;
    public event EventHandler? ExhibitUpdated;
    
    public string Title { get; set; }
    public string Price { get; set; }
    public string Description { get; set; }
    public bool Originality { get; set; }
    public byte[] ImageBytes { get; set; }
    public BitmapSource PreviewImage { get; set; }
    
    private bool _isInEditMode;
    public bool IsInEditMode
    {
        get => _isInEditMode;
        set
        {
            _isInEditMode = value;
            OnPropertyChanged(nameof(IsInEditMode));
            OnPropertyChanged(nameof(IsReadOnly));
        }
    }

    public bool IsReadOnly => !IsInEditMode;

    private User _currentUser;
    public User CurrentUser => _currentUser;
    
    public ICommand ToggleEditModeOrSaveChangesCommand { get; }
    public ICommand SetImageCommand { get; }
    public ICommand OnChoseImageCommand { get; }
    public ICommand DeleteCommand { get; }
    
    public ExhibitDetailsViewModel(Exhibit exhibit, CulturalSiberiaContext context, User user, Window window)
    {
        _context = context;
        _currentUser = user;
        _exhibit = exhibit;
        _window = window;

        Title = exhibit.Name;
        Price = exhibit.Price?.ToString() ?? "Не указана";
        Description = exhibit.Description ?? "Отсутствует";
        Originality = exhibit.OriginalityStatus.StatusName == "Original";

        PreviewImage = exhibit.ImageMediaId.HasValue ?
            ImageService.GetImageById(exhibit.ImageMediaId.Value, _context) : ImageService.GetImageById(-1, _context);

        ToggleEditModeOrSaveChangesCommand = new RelayCommand(OnToggleEditModeOrSaveChanges);
        SetImageCommand = new RelayCommand(() => SetImage(ImageBytes));
        OnChoseImageCommand = new RelayCommand(OnChoseImage);
        DeleteCommand = new RelayCommand(DeleteExhibit);
    }
    
    private async void DeleteExhibit()
    {
        try
        {
            _exhibit.StatusId = 2;

            _context.Exhibits.Update(_exhibit);
            await _context.SaveChangesAsync();
            
            MessageService.ShowSuccess("Экспонат удален");

            ExhibitDeleted?.Invoke(this, EventArgs.Empty);
            
            _window.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при удалении экспоната: " + ex.Message);
            
            MessageService.ShowError("Ошибка при удалении экспоната");
        }
    }
    
    private void OnToggleEditModeOrSaveChanges()
    {
        if (IsInEditMode)
        {
            SaveChangesAsync();
        }
        else
            IsInEditMode = true;
    }
    
    private void OnChoseImage()
    {
        var imageBytes = ImageService.ChooseImage();

        ImageBytes = imageBytes ?? null;

        if (imageBytes != null) 
            SetImage(imageBytes);
    }
    
    private async void SaveChangesAsync()
    {
        try
        {
            if (!InputValidator.ValidateNewExhibit(Title, decimal.Parse(Price)))
                return;
            
            _exhibit.Name = Title;
            _exhibit.Description = Description;
            _exhibit.Price = decimal.Parse(Price);
            _exhibit.UpdatedBy = CurrentUser.Id;
            _exhibit.UpdatedAt = DateTime.Now;
    
            if (ImageBytes?.Length > 0)
            {
                var mediaFileId = await MediaFileService.SaveMediaFileAsync(
                    "Exhibit",
                    _exhibit.Id,
                    $"exhibit_{_exhibit.Id}.png",
                    "image/png",
                    ImageBytes,
                    DateTime.Now,
                    _context);
    
                if (mediaFileId.HasValue)
                    _exhibit.ImageMediaId = mediaFileId.Value;
            }
    
            _context.Exhibits.Update(_exhibit);
            await _context.SaveChangesAsync();
            
            MessageService.ShowSuccess("Изменения сохранены");
            ExhibitUpdated?.Invoke(this, EventArgs.Empty);
            
            IsInEditMode = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка в сохранении отредактированных данных экспозиции: " + ex.Message);
            
            MessageService.ShowError("Ошибка сохранения изменений");
        }
    }
    
    private void SetImage(byte[]? imageBytes)
    {
        if (imageBytes == null) return;

        ImageBytes = imageBytes;
        PreviewImage = ImageService.SetImage(imageBytes);
        OnPropertyChanged(nameof(PreviewImage));
    }
}