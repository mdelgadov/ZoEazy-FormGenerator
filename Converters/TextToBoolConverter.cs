using System.Globalization;

namespace ZoEazy.Converters;

public class TextToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string? param = parameter as string;
        if (param != null && param.ToUpperInvariant() == "NAME")
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }
            return false;
        }

        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return false;
        }
        return true;
    }
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

