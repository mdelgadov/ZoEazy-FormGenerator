using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using CommunityToolkit.Maui.Converters;
using ZoEazy.Models;

namespace ZoEazy.Converters;

public class DateSelectedArgsConverter(string viewName) : BaseConverterOneWay<DateChangedEventArgs, object?>
{
    public override object? DefaultConvertReturnValue { get; set; } = null;

    [return: NotNullIfNotNull(nameof(value))]
    public override EventArgs? ConvertFrom(DateChangedEventArgs value, CultureInfo? culture)
    {
        return new VvDateSelectedEventArgs(viewName, value.OldDate, value.NewDate);
    }
}