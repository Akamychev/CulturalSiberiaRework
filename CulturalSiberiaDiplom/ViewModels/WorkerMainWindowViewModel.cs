using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.Views.WorkerOperationsWithEvents;
using CulturalSiberiaDiplom.Views.WorkerOperationsWithMuseumExhibits;
using CulturalSiberiaDiplom.Views.WorkerOperationsWithMuseums;

namespace CulturalSiberiaDiplom.ViewModels;

public class WorkerMainWindowViewModel : NotifyProperty
{
    private ObservableCollection<Museum> _allMuseums = new();
    private ObservableCollection<Event> _allEvents = new();
    
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

    public ICommand NavigateCommand { get; }
    
    public WorkerMainWindowViewModel()
    {
        LoadData();
        NavigateCommand = new NavigateCommand(OnNavigate);
    }

    private void LoadData()
    {
        _allMuseums = LoadDataFromDb.LoadMuseums();
        _allEvents = LoadDataFromDb.LoadEvents();
        
        Museums = new ObservableCollection<Museum>(_allMuseums);
        Events = new ObservableCollection<Event>(_allEvents);
    }

    private void PerformSearch()
    {
        var filteredEvents = SearchService.SearchEvents(_allEvents, SearchQuery);
        Events = new ObservableCollection<Event>(filteredEvents);

        var filteredMuseums = SearchService.SearchMuseums(_allMuseums, SearchQuery);
        Museums = new ObservableCollection<Museum>(filteredMuseums);
    }

    private void OnNavigate(string actionType, string entityType, object data)
    {
        switch (entityType)
        {
            case "Event":
                HandleEventNavigation(actionType, data);
                break;
            case "Museum":
                HandleMuseumNavigation(actionType, data);
                break;
            case "Exhibit":
                HandleExhibitNavigation(actionType, data);
                break;
            default:
                throw new ArgumentException("Не поддерживаемый тип сущности");
        }
    }

    private void HandleEventNavigation(string actionType, object data)
    {
        switch (actionType)
        {
            case "Add":
                if (data is Event selectedEvent)
                    NavigateTo(new AddNewEventWindow());
                break;
            case "Edit":
                NavigateTo(new EditEventWindow());
                break;
            case "Delete":
                NavigateTo(new DeleteEventWindow());
                break;
            default:
                throw new ArgumentException("Не поддерживаемый тип действия");
        }
    }
    
    private void HandleMuseumNavigation(string actionType, object data)
    {
        switch (actionType)
        {
            case "Add":
                NavigateTo(new AddNewMuseumWindow());
                break;
            case "Edit":
                NavigateTo(new EditMuseumWindow());
                break;
            case "Delete":
                NavigateTo(new DeleteMuseumWindow());
                break;
            default:
                throw new ArgumentException("Не поддерживаемый тип действия");
        }
    }
    
    private void HandleExhibitNavigation(string actionType, object data)
    {
        switch (actionType)
        {
            case "Add":
                NavigateTo(new AddNewMuseumExhibitWindow());
                break;
            case "Edit":
                NavigateTo(new EditMuseumExhibitWindow());
                break;
            case "Delete":
                NavigateTo(new DeleteMuseumExhibitWindow());
                break;
            default:
                throw new ArgumentException("Не поддерживаемый тип действия");
        }
    }

    private void NavigateTo(Window window)
    {
        window.ShowDialog();
    }
        
}