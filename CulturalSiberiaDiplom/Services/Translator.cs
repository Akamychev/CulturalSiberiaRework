namespace CulturalSiberiaDiplom.Services;

public static class Translator
{
    public static string TranslateEventType(string typeName)
    {
        return typeName switch
        {
            "Concert" => "Концерт",
            "Exhibition" => "Выставка",
            "Stand-up" => "Стендап",
            "Cinema" => "Киносеанс",
            _ => typeName
        };
    }

    public static string TranslateMuseumTypes(string typeName)
    {
        return typeName switch
        {
            "Archaeological" => "Археологический",
            "Childrens" => "Детский",
            "Departmental" => "Областной",
            "Virtual" => "Виртуальный",
            "Interactive" => "Интерактивный",
            "Historical" => "Исторический",
            "Science" => "Научный",
            _ => typeName
        };
    }
}