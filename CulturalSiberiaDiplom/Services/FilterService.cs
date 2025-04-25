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
        string museumType = null,
        string architectName = null,
        DateTime? foundationDateFrom = null,
        DateTime? foundationDateTo = null)
    {
        var filteredMuseums = museums.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(museumType))
            filteredMuseums = filteredMuseums.Where(m => m.Type.TypeName == museumType);
        
        if (!string.IsNullOrWhiteSpace(architectName))
            filteredMuseums = filteredMuseums.Where(m => m.Architects.Contains(architectName, StringComparison.OrdinalIgnoreCase));
        
        if (foundationDateFrom.HasValue)
            filteredMuseums = filteredMuseums.Where(m => m.DateOfFoundation >= DateOnly.FromDateTime(foundationDateFrom.Value.Date));
        if (foundationDateTo.HasValue)
            filteredMuseums = filteredMuseums.Where(m => m.DateOfFoundation <= DateOnly.FromDateTime(foundationDateTo.Value.Date));

        return filteredMuseums;
    }
}