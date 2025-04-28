using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Pipelines;
using System.Linq;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;

namespace CulturalSiberiaDiplom.ViewModels;

public class UserWorkerMainWindowViewModel : NotifyProperty
{
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
    
    public UserWorkerMainWindowViewModel()
    {
        LoadData();
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
            SelectedMuseumType, /////////fix
            ArchitectName,
            FoundationDateFrom,
            FoundationDateTo);
        Museums = new ObservableCollection<Museum>(filteredMuseums);
    }
}