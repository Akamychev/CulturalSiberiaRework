using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;

namespace CulturalSiberiaDiplom.ViewModels;

public class EventDetailsViewModel : NotifyProperty
{
    private readonly CulturalSiberiaContext _context;
    private readonly Event _event;
    private readonly Window _window;

    public event EventHandler? EventDeleted;
    public event EventHandler? EventUpdated; 
    
    public string Title { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string? AvailableSeats { get; set; }
    public string? Price { get; set; }
    public byte[]? ImageBytes { get; set; }
    public BitmapSource PreviewImage { get; set; }
    
    private readonly User _currentUser;
    public User CurrentUser => _currentUser;
    
    private DateTime _editStartDate;
    public DateTime EditStartDate
    {
        get => _editStartDate;
        set
        {
            var roundedValue = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0);
            if (SetField(ref _editStartDate, roundedValue))
                OnPropertyChanged(nameof(EditStartDate));
        }
    }

    private DateTime _editEndDate;
    public DateTime EditEndDate
    {
        get => _editEndDate;
        set
        {
            var roundedValue = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0);
            if (SetField(ref _editEndDate, roundedValue))
                OnPropertyChanged(nameof(EditEndDate));
        }
    }

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
    
    public ICommand ToggleEditModeOrSaveChangesCommand { get; }
    public ICommand SetImageCommand { get; }
    public ICommand OnChoseImageCommand { get; }
    public ICommand DeleteCommand { get; }
    

    public EventDetailsViewModel(Event @event, CulturalSiberiaContext context, User user, Window window)
    {
        _currentUser = user;
        _context = context;
        _event = @event;
        _window = window;
        
        EditStartDate = new DateTime(@event.StartDate.Year, @event.StartDate.Month, @event.StartDate.Day,
            @event.StartDate.Hour, @event.StartDate.Minute, 0);

        EditEndDate = new DateTime(@event.EndDate.Year, @event.EndDate.Month, @event.EndDate.Day,
            @event.EndDate.Hour, @event.EndDate.Minute, 0);
        
        Title = @event.Title;
        Location = @event.Location ?? "Неизвестно";
        Description = @event.Description ?? "Отсутствует";
        StartDate = @event.StartDate.ToString("dd.MM.yyyy HH:mm");
        EndDate = @event.EndDate.ToString("dd.MM.yyyy HH:mm");
        AvailableSeats = @event.Capacity?.ToString() ?? "Неизвестно";
        Price = @event.Price?.ToString() ?? "Не указана";
        PreviewImage = @event.ImageMediaId.HasValue ?
            ImageService.GetImageById(@event.ImageMediaId.Value, _context) : ImageService.GetImageById(-1, _context);

        ToggleEditModeOrSaveChangesCommand = new RelayCommand(OnToggleEditModeOrSaveChanges);
        SetImageCommand = new RelayCommand(() => SetImage(ImageBytes));
        OnChoseImageCommand = new RelayCommand(OnChoseImage);
        DeleteCommand = new RelayCommand(DeleteEvent);
    }

    private async void DeleteEvent()
    {
        try
        {
            _event.StatusId = 4;

            _context.Events.Update(_event);
            await _context.SaveChangesAsync();
            
            MessageService.ShowSuccess("Мероприятие удалено");

            EventDeleted?.Invoke(this, EventArgs.Empty);
            
            _window.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при удалении мероприятия: " + ex.Message);
            
            MessageService.ShowError("Ошибка при удалении мероприятия");
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
            if (!InputValidator.ValidateNewEvent(Title, Location, decimal.Parse(Price), int.Parse(AvailableSeats)))
                return;

            if (EditStartDate == default || EditEndDate == default || EditStartDate > EditEndDate)
            {
                MessageService.ShowError("Некорректный ввод даты");
                return;
            }
            
            _event.Title = Title;
            _event.Location = Location;
            _event.Description = Description;
            _event.Price = decimal.Parse(Price);
            _event.Capacity = int.Parse(AvailableSeats);
            _event.StartDate = EditStartDate;
            _event.EndDate = EditEndDate;
            _event.UpdatedBy = CurrentUser.Id;
            _event.UpdatedAt = DateTime.Now;
            _event.StatusId = 2;

            if (ImageBytes?.Length > 0)
            {
                var mediaFileId = await MediaFileService.SaveMediaFileAsync(
                    "Events",
                    _event.Id,
                    $"event_{_event.Id}.png",
                    "image/png",
                    ImageBytes,
                    DateTime.Now,
                    _context);

                if (mediaFileId.HasValue)
                    _event.ImageMediaId = mediaFileId.Value;
            }

            _context.Events.Update(_event);
            await _context.SaveChangesAsync();
            
            MessageService.ShowSuccess("Изменения сохранены");
            EventUpdated?.Invoke(this, EventArgs.Empty);
            
            IsInEditMode = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка в сохранении отредактированных данных мероприятия: " + ex.Message);
            
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