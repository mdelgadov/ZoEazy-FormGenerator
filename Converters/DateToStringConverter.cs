using System.Globalization;

namespace ZoEazy.Converters;

public class DateToStringConverter : IValueConverter
{
    public string DateFormat { get; set; } = "MM/dd/yyyy";

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            DateTime dateTime => dateTime.ToString(DateFormat, culture),
            DateOnly date => date.ToString(DateFormat, culture),
            _ => string.Empty
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string dateString)
        {
            if ((targetType == typeof(DateTime) || targetType == typeof(DateTime?)) 
                && DateTime.TryParseExact(dateString, DateFormat, culture, DateTimeStyles.None, out var dateTime))
            {
                return dateTime;
            }
            if ((targetType == typeof(DateOnly) || targetType == typeof(DateOnly?)) 
                && DateOnly.TryParseExact(dateString, DateFormat, culture, DateTimeStyles.None, out var dateOnly))
            {
                return dateOnly;
            }
        }
        return null;
    }
   
}