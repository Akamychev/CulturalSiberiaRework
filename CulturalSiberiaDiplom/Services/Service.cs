using CulturalSiberiaDiplom.Models;

namespace CulturalSiberiaDiplom.Services;

public class Service
{
    private static CulturalSiberiaContext _db;

    public static CulturalSiberiaContext GetDbContext()
    {
        return _db ??= new CulturalSiberiaContext();
    }
}