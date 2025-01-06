using System.Globalization;

namespace ZoEazy.Converters;

public class NumericToDoubleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            int intValue => (double)intValue,
            float floatValue => (double)floatValue,
            decimal decimalValue => (double)decimalValue,
            _ => throw new InvalidOperationException("Unsupported type")
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double doubleValue)
            throw new InvalidOperationException("Unsupported type");

        return targetType switch
        {
            Type t when t == typeof(int) => (int)doubleValue,
            Type t when t == typeof(float) => (float)doubleValue,
            Type t when t == typeof(decimal) => (decimal)doubleValue,
            _ => throw new InvalidOperationException("Unsupported type")
        };
    }
}