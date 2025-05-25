using System;
using System.Collections.Generic;
using System.Linq;
using CulturalSiberiaDiplom.Models;

namespace CulturalSiberiaDiplom.Services;

public static class FilterService
{
    public static IEnumerable<Event> FilterEvents(
        IEnumerable<Event> events,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        HashSet<string> selectedTypes = null)
    {
        var filteredEvents = events.AsQueryable();

        if (minPrice.HasValue && minPrice > 0)
            filteredEvents = filteredEvents.Where(e => e.Price >= minPrice.Value);
        if (maxPrice.HasValue && maxPrice > 0)
            filteredEvents = filteredEvents.Where(e => e.Price <= maxPrice.Value);

        if (startDate.HasValue && startDate >= DateTime.Now)
            filteredEvents = filteredEvents.Where(e => e.StartDate.Date >= startDate.Value);
        if (endDate.HasValue)
            filteredEvents = filteredEvents.Where(e => e.EndDate.Date <= endDate.Value);

        if (selectedTypes != null && selectedTypes.Count > 0)
            filteredEvents = filteredEvents.Where(e => selectedTypes.Contains(e.Type.TypeName));

        return filteredEvents;
    }
    
    public static IEnumerable<Museum> FilterMuseums(
        IEnumerable<Museum> museums,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        TimeOnly? startTime = null,
        TimeOnly? endTime = null)
    {
        var filteredMuseums = museums.AsQueryable();
        
        if (minPrice.HasValue && minPrice > 0)
            filteredMuseums = filteredMuseums.Where(m => m.Price >= minPrice.Value);
        if (maxPrice.HasValue && maxPrice > 0)
            filteredMuseums = filteredMuseums.Where(m => m.Price <= maxPrice.Value);
        
        if (startTime.HasValue)
            filteredMuseums = filteredMuseums.Where(m => m.StartWorkingTime >= startTime.Value);
        if (endTime.HasValue)
            filteredMuseums = filteredMuseums.Where(m => m.EndWorkingTime <= endTime.Value);

        return filteredMuseums;
    }
}