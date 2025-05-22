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
    private Event _event;
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
    public ICommand OnBuyTicketCommand { get; }
    

    public EventDetailsViewModel(Event @event, CulturalSiberiaContext context, User user, Window window)
    {
        _currentUser = user;
        _context = context;
        _event = @event;
        _window = window;

        LoadData();

        ToggleEditModeOrSaveChangesCommand = new RelayCommand(OnToggleEditModeOrSaveChanges);
        SetImageCommand = new RelayCommand(() => SetImage(ImageBytes));
        OnChoseImageCommand = new RelayCommand(OnChoseImage);
        DeleteCommand = new RelayCommand(DeleteEvent);
        OnBuyTicketCommand = new RelayCommand(BuyTicket);
    }

    private async void DeleteEvent()
    {
        var result = MessageBox.Show($"Вы точно хотите удалить мероприятие \"{_event.Title}\"?",
            "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
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
        else
            MessageService.ShowInfo("Удаление отменено");
    }

    private async void BuyTicket()
    {
        if (!InputValidator.ValidateAvailableSeats(AvailableSeats))
            return;

        try
        {
            var ticket = new Ticket
            {
                TicketTargetTypeId = 1,
                TicketTargetId = _event.Id,
                StatusId = 1,
                UserId = _currentUser.Id,
                PurchaseDate = DateTime.Now
            };

            _event.Capacity -= 1;

            _context.Tickets.Add(ticket);
            _context.Events.Update(_event);
            await _context.SaveChangesAsync();
            
            LoadData();
            
            MessageService.ShowSuccess("Билет успешно приобретен");
            EventUpdated?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при покупке билета на мероприятие: " + ex.Message);
            
            MessageService.ShowError("Ошибка при покупке билета, повторите операцию позже");
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
            if (!InputValidator.ValidateNewEvent(Title, Location, decimal.Parse(Price),
                    int.Parse(AvailableSeats), EditStartDate, EditEndDate))
                return;
            
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
            
            LoadData();
            
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
    
    private void LoadData()
    {
        EditStartDate = new DateTime(_event.StartDate.Year, _event.StartDate.Month, _event.StartDate.Day,
            _event.StartDate.Hour, _event.StartDate.Minute, 0);

        EditEndDate = new DateTime(_event.EndDate.Year, _event.EndDate.Month, _event.EndDate.Day,
            _event.EndDate.Hour, _event.EndDate.Minute, 0);
        
        Title = _event.Title;
        Location = _event.Location ?? "Локация неизвестна";
        Description = _event.Description ?? "Описание отсутствует";
        StartDate = _event.StartDate.ToString("dd.MM.yyyy HH:mm");
        EndDate = _event.EndDate.ToString("dd.MM.yyyy HH:mm");
        AvailableSeats = _event.Capacity?.ToString() ?? "Неизвестно";
        Price = _event.Price?.ToString() ?? "Не указана";
        PreviewImage = _event.ImageMediaId.HasValue ?
            ImageService.GetImageById(_event.ImageMediaId.Value, _context) : ImageService.GetImageById(-1, _context);
    }
}