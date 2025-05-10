using System;
using System.Collections.ObjectModel;
using System.Linq;
using CulturalSiberiaDiplom.Models;

namespace CulturalSiberiaDiplom.Services;

public class LoadDataFromDb : NotifyProperty
{
    public static ObservableCollection<Museum> LoadMuseums()
    {
        var query = Service.GetDbContext().Museums.Where(m => m.StatusId == 1).ToList();
        return new ObservableCollection<Museum>(query);
    }

    public static ObservableCollection<Event> LoadEvents()
    {
        var query = Service.GetDbContext().Events.Where(e => e.StatusId == 1 || e.StatusId == 2).ToList();
        return new ObservableCollection<Event>(query);
    }

    public static ObservableCollection<Event> LoadUpcomingEvents()
    {
        DateTime today = DateTime.Now.Date;
        DateTime nextWeek = today.AddDays(7);

        var query = Service.GetDbContext().Events
            .Where(e => e.StartDate >= today && e.StartDate <= nextWeek && e.StatusId == 1 || e.StatusId == 2).ToList();

        return new ObservableCollection<Event>(query);
    }

    public static ObservableCollection<Eventstype> LoadEventsTypes()
    {
        var query = Service.GetDbContext().Eventstypes.ToList();
        return new ObservableCollection<Eventstype>(query);
    }

    public static ObservableCollection<Exhibit> LoadExhibits()
    {
        var query = Service.GetDbContext().Exhibits.ToList();
        return new ObservableCollection<Exhibit>(query);
    }

}