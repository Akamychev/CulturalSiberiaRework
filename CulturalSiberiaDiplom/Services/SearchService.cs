using System.Collections.Generic;
using System.Linq;
using CulturalSiberiaDiplom.Models;

namespace CulturalSiberiaDiplom.Services;

public static class SearchService
{
    public static IEnumerable<Event> SearchEvents(IEnumerable<Event> events, string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return events;

        query = query.ToLowerInvariant();

        return events.Where(e => e.Title.ToLowerInvariant().Contains(query) ||
                                 e.Location.ToLowerInvariant().Contains(query));
    }

    public static IEnumerable<Museum> SearchMuseums(IEnumerable<Museum> museums, string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return museums;

        query = query.ToLowerInvariant();

        return museums.Where(m => m.Name.ToLowerInvariant().Contains(query) ||
                                  m.Location.ToLowerInvariant().Contains(query));
    }
}