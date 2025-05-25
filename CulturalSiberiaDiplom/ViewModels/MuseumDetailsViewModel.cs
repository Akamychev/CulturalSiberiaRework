using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.Views.DetailsWindows;
using CulturalSiberiaDiplom.Views.WorkerOperationsWithMuseumExhibits;
using Microsoft.EntityFrameworkCore;

namespace CulturalSiberiaDiplom.ViewModels;

public class MuseumDetailsViewModel : NotifyProperty
{
    private readonly CulturalSiberiaContext _context;
    private readonly Museum _museum;
    private readonly Window _window;

    public event EventHandler? MuseumDeleted;
    public event EventHandler? MuseumUpdated;
    
    public string Title { get; set; }
    public string Location { get; set; }
    public string? Description { get; set; }
    public string StartTime { get; set; }
    public TimeOnly EditStartTime { get; set; }
    public string EndTime { get; set; }
    public TimeOnly EditEndTime { get; set; }
    public string? Type { get; set; }
    public string? Price { get; set; }
    public byte[] ImageBytes { get; set; }
    public BitmapSource PreviewImage { get; set; }
    
    private readonly User _currentUser;
    public User CurrentUser => _currentUser;

    public ObservableCollection<ExhibitItemViewModel> Exhibits { get; set; }
    
    private ExhibitItemViewModel _selectedExhibit;
    public ExhibitItemViewModel SelectedExhibit
    {
        get => _selectedExhibit;
        set
        {
            _selectedExhibit = value;
            OnPropertyChanged(nameof(SelectedExhibit));
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
    
    public ICommand OpenExhibitDetailsCommand { get; }
    public ICommand AddNewExhibitCommand { get; }
    public ICommand ToggleEditModeOrSaveChangesCommand { get; }
    public ICommand SetImageCommand { get; }
    public ICommand OnChoseImageCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand OnBuyTicketCommand { get; }
    

    public MuseumDetailsViewModel(Museum museum, CulturalSiberiaContext context, User user, Window window)
    {
        _currentUser = user;
        _context = context;
        _museum = museum;
        _window = window;
        
        LoadData();
        LoadExhibitsData(museum);

        OpenExhibitDetailsCommand = new RelayCommand(() => OnOpenExhibitDetails(SelectedExhibit));
        AddNewExhibitCommand = new RelayCommand(GoToAddNewExhibit);
        ToggleEditModeOrSaveChangesCommand = new RelayCommand(OnToggleEditModeOrSaveChanges);
        SetImageCommand = new RelayCommand(() => SetImage(ImageBytes));
        OnChoseImageCommand = new RelayCommand(OnChoseImage);
        DeleteCommand = new RelayCommand(DeleteMuseum);
        OnBuyTicketCommand = new RelayCommand(BuyTicket);
    }
    
    private void OnOpenExhibitDetails(object? param)
    {
        if (param is ExhibitItemViewModel selectedExhibit)
        {
            var exhibitWithOriginalityStatus =
                _context.Exhibits.Include(e => e.OriginalityStatus)
                    .FirstOrDefault(e => e.Id == selectedExhibit.Exhibit.Id);

            if (exhibitWithOriginalityStatus != null)
            {
                var window = new ExhibitDetailsWindow(exhibitWithOriginalityStatus, _context, CurrentUser);

                ((ExhibitDetailsViewModel)window.DataContext).ExhibitDeleted += (s, e) =>
                {
                    LoadExhibitsData(_museum);
                };

                ((ExhibitDetailsViewModel)window.DataContext).ExhibitUpdated += (s, e) =>
                {
                    LoadExhibitsData(_museum);
                };
                
                window.ShowDialog();
            }
            else
                MessageService.ShowError("Не удалось загрузить информацию об экспонате");
        }
        else
            MessageService.ShowError("Не удалось открыть экспонат");
    }

    private void GoToAddNewExhibit()
    {
        var addNewExhibitWindow = new AddNewMuseumExhibitWindow(_context, _museum);

        ((AddNewExhibitViewModel)addNewExhibitWindow.DataContext).ExhibitAdded += (s, e) => LoadExhibitsData(_museum);
        
        addNewExhibitWindow.ShowDialog();
    }

    private void BuyTicket()
    {
        try
        {
            UserBuyTicket.BuyTicket(_currentUser, null, _museum);
            
            MessageService.ShowSuccess("Билет успешно приобретен");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при покупке билета в музей: {_museum.Id} " + "Сообщение ошибки: " + ex.Message);
            
            MessageService.ShowError("Ошибка при покупке билета, повторите попытку позже");
        }
    }
    
    private async void DeleteMuseum()
    {
        try
        {
            _museum.StatusId = 2;

            _context.Museums.Update(_museum);
            await _context.SaveChangesAsync();
            
            MessageService.ShowSuccess("Музей удален");

            MuseumDeleted?.Invoke(this, EventArgs.Empty);
            
            _window.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при удалении музея: " + ex.Message);
            
            MessageService.ShowError("Ошибка при удалении музея");
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
            if (!InputValidator.ValidateNewMuseum(Title, Location, decimal.Parse(Price),
                    null, EditStartTime, EditEndTime))
                return;
            
            _museum.Name = Title;
            _museum.Location = Location;
            _museum.Description = Description;
            _museum.Price = decimal.Parse(Price);
            _museum.StartWorkingTime = EditStartTime;
            _museum.EndWorkingTime = EditEndTime;
            _museum.UpdatedBy = CurrentUser.Id;
            _museum.UpdatedAt = DateTime.Now;
    
            if (ImageBytes?.Length > 0)
            {
                var mediaFileId = await MediaFileService.SaveMediaFileAsync(
                    "Museum",
                    _museum.Id,
                    $"museum_{_museum.Id}.png",
                    "image/png",
                    ImageBytes,
                    DateTime.Now,
                    _context);
    
                if (mediaFileId.HasValue)
                    _museum.ImageMediaId = mediaFileId.Value;
            }
    
            _context.Museums.Update(_museum);
            await _context.SaveChangesAsync();
            
            MessageService.ShowSuccess("Изменения сохранены");
            MuseumUpdated?.Invoke(this, EventArgs.Empty);
            
            LoadData();
            
            IsInEditMode = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка в сохранении отредактированных данных музея: " + ex.Message);
            
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
        Title = _museum.Name;
        Location = _museum.Location;
        Description = _museum.Description ?? "Отсутствует";
        StartTime = _museum.StartWorkingTime.ToString("t");
        EndTime = _museum.EndWorkingTime.ToString("t");
        Type = Translator.TranslateMuseumTypes(_museum.Type.TypeName);
        Price = _museum.Price?.ToString() ?? "Не указана";
        PreviewImage = _museum.ImageMediaId.HasValue ?
            ImageService.GetImageById(_museum.ImageMediaId.Value, _context) : ImageService.GetImageById(-1, _context);
    }

    private void LoadExhibitsData(Museum museum)
    {
        var exhibitList = museum.MuseumExhibits
            .Select(exhibit => new ExhibitItemViewModel(exhibit, _context))
            .ToList();

        if (Exhibits == null)
        {
            Exhibits = new ObservableCollection<ExhibitItemViewModel>(exhibitList);
        }
        else
        {
            Exhibits.Clear();
            foreach (var item in exhibitList)
            {
                Exhibits.Add(item);
            }
        }

        OnPropertyChanged(nameof(Exhibits));
    }
}