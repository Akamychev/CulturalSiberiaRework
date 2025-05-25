using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.Views;
using CulturalSiberiaDiplom.Views.DetailsWindows;
using CulturalSiberiaDiplom.Views.UserMenuViews;
using CulturalSiberiaDiplom.Views.WorkerOperationsWithEvents;
using CulturalSiberiaDiplom.Views.WorkerOperationsWithMuseums;
using Microsoft.EntityFrameworkCore;

namespace CulturalSiberiaDiplom.ViewModels;

public class UserWorkerMainWindowViewModel : NotifyProperty
{
    private readonly CulturalSiberiaContext _context;
    
    private ObservableCollection<Museum> _allMuseums = new();
    private ObservableCollection<Event> _allEvents = new();
    private ObservableCollection<Event> _allUpcomingEvents = new();
    
    private ObservableCollection<Museum> _museums = new();
    public ObservableCollection<Museum> Museums
    {
        get => _museums;
        set
        {
            _museums = value;
            OnPropertyChanged(nameof(Museums));
        }
    }

    private ObservableCollection<Event> _events = new();
    public ObservableCollection<Event> Events
    {
        get => _events;
        set
        {
            _events = value;
            OnPropertyChanged(nameof(Events));
        }
    }

    private ObservableCollection<Event> _upcomingEvents;
    public ObservableCollection<Event> UpcomingEvents
    {
        get => _upcomingEvents;
        set
        {
            _upcomingEvents = value;
            OnPropertyChanged(nameof(UpcomingEvents));
        }
    }
    
    private decimal? _minPrice;
    public decimal? MinPrice
    {
        get => _minPrice;
        set
        {
            _minPrice = value;
            OnPropertyChanged(nameof(MinPrice));
            ApplyFilters();
        }
    }

    private decimal? _maxPrice;
    public decimal? MaxPrice
    {
        get => _maxPrice;
        set
        {
            _maxPrice = value;
            OnPropertyChanged(nameof(MaxPrice));
            ApplyFilters();
        }
    }

    private DateTime? _startDate;
    public DateTime? StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;
            OnPropertyChanged(nameof(StartDate));
            ApplyFilters();
        }
    }

    private DateTime? _endDate;
    public DateTime? EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            OnPropertyChanged(nameof(EndDate));
            ApplyFilters();
        }
    }
    
    private TimeOnly? _startTime;
    public TimeOnly? StartTime
    {
        get => _startTime;
        set
        {
            _startTime = value;
            OnPropertyChanged(nameof(StartTime));
            ApplyFilters();
        }
    }

    private TimeOnly? _endTime;
    public TimeOnly? EndTime
    {
        get => _endTime;
        set
        {
            _endTime = value;
            OnPropertyChanged(nameof(EndTime));
            ApplyFilters();
        }
    }

    /// <summary>
    /// CheckBox prior 3 later
    /// </summary>
    // private ObservableCollection<CheckBoxController> _eventTypes = new();
    // public ObservableCollection<CheckBoxController> EventTypes
    // {
    //     get => _eventTypes;
    //     set
    //     {
    //         _eventTypes = value;
    //         OnPropertyChanged(nameof(EventTypes));
    //     }
    // }
    
    private string _selectedMuseumType;
    public string SelectedMuseumType
    {
        get => _selectedMuseumType;
        set
        {
            _selectedMuseumType = value;
            OnPropertyChanged(nameof(SelectedMuseumType));
            ApplyFilters();
        }
    }

    private string _architectName;
    public string ArchitectName
    {
        get => _architectName;
        set
        {
            _architectName = value;
            OnPropertyChanged(nameof(ArchitectName));
            ApplyFilters();
        }
    }

    private DateTime? _foundationDateFrom;
    public DateTime? FoundationDateFrom
    {
        get => _foundationDateFrom;
        set
        {
            _foundationDateFrom = value;
            OnPropertyChanged(nameof(FoundationDateFrom));
            ApplyFilters();
        }
    }

    private DateTime? _foundationDateTo;
    public DateTime? FoundationDateTo
    {
        get => _foundationDateTo;
        set
        {
            _foundationDateTo = value;
            OnPropertyChanged(nameof(FoundationDateTo));
            ApplyFilters();
        }
    }

    private string _searchQuery;
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            OnPropertyChanged(nameof(SearchQuery));
            PerformSearch();
        }
    }
    
    private Event? _selectedEvent;
    public Event? SelectedEvent
    {
        get => _selectedEvent;
        set
        {
            _selectedEvent = value;
            OnPropertyChanged(nameof(SelectedEvent));
        }
    }

    private Museum? _selectedMuseum;
    public Museum? SelectedMuseum
    {
        get => _selectedMuseum;
        set
        {
            _selectedMuseum = value;
            OnPropertyChanged(nameof(SelectedMuseum));
        }
    }
    
    private Visibility _userMenuVisibility = Visibility.Collapsed;
    public Visibility UserMenuVisibility
    {
        get => _userMenuVisibility;
        set
        {
            _userMenuVisibility = value;
            OnPropertyChanged(nameof(UserMenuVisibility));
        }
    }

    private readonly User _currentUser;
    public User CurrentUser => _currentUser;

    public string Username => _currentUser.Username;

    public string UserBalance => "Баланс: " + _currentUser.Balance;
    public BitmapSource AvatarImage => ImageService.GetImageById(_currentUser.AvatarMediaId ?? -1, _context);
    
    public bool IsInUserMenu { get; set; }
    
    public ICommand AddNewEventCommand { get; set; }
    public ICommand AddNewMuseumCommand { get; set; }
    
    public ICommand AllEventsReportPDF { get; set; }
    public ICommand AllMuseumsReportPDF { get; set; }
    
    public ICommand OpenEventDetailsCommand { get; }
    public ICommand OpenMuseumDetailsCommand { get; }
    
    public ICommand BalanceCommand { get; }
    public ICommand ProfileCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand GoToAuthCommand { get; }

    public UserWorkerMainWindowViewModel(CulturalSiberiaContext context, User user)
    {
        _currentUser = user;
        _context = context;
        LoadData();
        
        AddNewEventCommand = new RelayCommand(() =>
        {
            var addNewEventWindow = new AddNewEventWindow();

            ((AddNewEventViewModel)addNewEventWindow.DataContext).AddEvent += (s, e) =>
            {
                LoadData();
            };
            
            addNewEventWindow.ShowDialog();
        });
        AddNewMuseumCommand = new RelayCommand(() =>
        {
            var addNewMuseumWindow = new AddNewMuseumWindow();

            ((AddNewMuseumViewModel)addNewMuseumWindow.DataContext).AddMuseum += (s, e) =>
            {
                LoadData();
            };
            
            addNewMuseumWindow.ShowDialog();
        });
        
        AllEventsReportPDF = new RelayCommand(async () => await AllEventsReportAsync());
        AllMuseumsReportPDF = new RelayCommand(async () => await AllMuseumsReportAsync());

        OpenEventDetailsCommand = new RelayCommand(() => OnOpenEventDetails(SelectedEvent));
        OpenMuseumDetailsCommand = new RelayCommand(() => OnOpenMuseumDetails(SelectedMuseum));
        
        BalanceCommand = new RelayCommand(OnBalance);
        ProfileCommand = new RelayCommand(OnProfile);
        LogoutCommand = new RelayCommand(LogoutUser.OnLogout);
        GoToAuthCommand = new RelayCommand(() => OpenNewWindowAndCloseCurrent.OpenWindow(new AuthorizationWindow()));
    }

    private void PerformSearch()
    {
        var filteredUpcomingEvents = SearchService.SearchEvents(_allUpcomingEvents, SearchQuery);
        UpcomingEvents = new ObservableCollection<Event>(filteredUpcomingEvents);

        var filteredEvents = SearchService.SearchEvents(_allEvents, SearchQuery);
        Events = new ObservableCollection<Event>(filteredEvents);

        var filteredMuseums = SearchService.SearchMuseums(_allMuseums, SearchQuery);
        Museums = new ObservableCollection<Museum>(filteredMuseums);
    }
    
    private void ApplyFilters()
    {
        var filteredUpcomingEvents = FilterService.FilterEvents(
            _allUpcomingEvents,
            MinPrice > 0 ? MinPrice : null,
            MaxPrice > 0 ? MaxPrice : null,
            StartDate.HasValue ? StartDate : null,
            EndDate.HasValue ? EndDate : null);
        UpcomingEvents = new ObservableCollection<Event>(filteredUpcomingEvents);
        
        var filteredEvents = FilterService.FilterEvents(
            _allEvents,
            MinPrice > 0 ? MinPrice : null,
            MaxPrice > 0 ? MaxPrice : null,
            StartDate.HasValue ? StartDate : null,
            EndDate.HasValue ? EndDate : null);
        Events = new ObservableCollection<Event>(filteredEvents);
        
        var filteredMuseums = FilterService.FilterMuseums(
            _allMuseums,
            MinPrice > 0 ? MinPrice : null,
            MaxPrice > 0 ? MaxPrice : null,
            StartTime.HasValue ? StartTime : null,
            EndTime.HasValue ? EndTime : null);
        Museums = new ObservableCollection<Museum>(filteredMuseums);
    }
    
    private void OnOpenEventDetails(object? param)
    {
        if (param is Event selectedEvent)
        {
            var window = new EventDetailsWindow(selectedEvent);

            ((EventDetailsViewModel)window.DataContext).EventDeleted += (s, e) =>
            {
                LoadData();
            };

            ((EventDetailsViewModel)window.DataContext).EventUpdated += (s, e) =>
            {
                LoadData();
            };
            
            window.ShowDialog();
        }
    }

    private void OnOpenMuseumDetails(object? param)
    {
        if (param is Museum selectedMuseum)
        {
            var museumWithTypeInfoAndExhibits =
                _context.Museums.Include(m => m.Type)
                    .Include(m => m.MuseumExhibits)
                    .ThenInclude(e => e.ImageMedia)
                    .FirstOrDefault(m => m.Id == selectedMuseum.Id);

            if (museumWithTypeInfoAndExhibits != null)
            {
                var window = new MuseumDetailsWindow(museumWithTypeInfoAndExhibits);

                ((MuseumDetailsViewModel)window.DataContext).MuseumDeleted += (s, e) =>
                {
                    LoadData();
                };

                ((MuseumDetailsViewModel)window.DataContext).MuseumUpdated += (s, e) =>
                {
                    LoadData();
                };

                window.ShowDialog();
            }
            else
                MessageService.ShowError("Не удалось загрузить информацию о музее");
        }
    }
    
    private void LoadData()
    {
        _allMuseums = LoadDataFromDb.LoadMuseums();
        _allEvents = LoadDataFromDb.LoadEvents();
        _allUpcomingEvents = LoadDataFromDb.LoadUpcomingEvents();
        
        Museums = new ObservableCollection<Museum>(_allMuseums);
        Events = new ObservableCollection<Event>(_allEvents);
        UpcomingEvents = new ObservableCollection<Event>(_allUpcomingEvents);
    }

    private void OnBalance()
    {
        UserMenuVisibility = Visibility.Collapsed;
        Console.WriteLine("Функция баланса");
    }

    private void OnProfile()
    {
        UserMenuVisibility = Visibility.Collapsed;
        OpenNewWindowAndCloseCurrent.OpenWindow(new PersonalCabinet(_context, _currentUser));
    }

    private async Task AllEventsReportAsync()
    {
        var savePath = Reports.GetSaveFilePath(".pdf", "PDF файл (*.pdf)|*.pdf");
        
        if (string.IsNullOrWhiteSpace(savePath))
            return;
        
        var events = Service.GetDbContext().Events
            .Include(e => e.Type).ToList();
        await Reports.GenerateAllEventsPdfReportAsync(events, savePath);
    }

    private async Task AllMuseumsReportAsync()
    {
        var savePath = Reports.GetSaveFilePath(".pdf", "PDF файл (*.pdf)|*.pdf");
        
        if (string.IsNullOrWhiteSpace(savePath))
            return;
        
        var museums = Service.GetDbContext().Museums
            .Include(m => m.Type).ToList();
        
        await Reports.GenerateAllMuseumsPdfReportAsync(museums, savePath);
    }
}