using System;
using System.Globalization;
using System.Windows.Data;

namespace CulturalSiberiaDiplom.Services;

[ValueConversion(typeof(bool), typeof(string))]
public class BooleanToEditSaveConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo cultureInfo)
    {
        return (value is true) ? "Сохранить" : "Редактировать";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo cultureInfo)
    {
        throw new NotImplementedException();
    }
}