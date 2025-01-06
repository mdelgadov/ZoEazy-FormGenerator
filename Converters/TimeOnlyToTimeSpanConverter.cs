using System.Globalization;

namespace ZoEazy.Converters;

public class TimeOnlyToTimeSpanConverter : IValueConverter
{
    // Converts a TimeOnly to a TimeSpan
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            null => null,
            TimeOnly timeOnly => timeOnly.ToTimeSpan(),
            _ => throw new InvalidOperationException("The value must be a TimeOnly")
        };
    }

    // Converts a TimeSpan back to a TimeOnly
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan)
        {
            return TimeOnly.FromTimeSpan(timeSpan);
        }
        throw new InvalidOperationException("The value must be a TimeSpan");
    }
}