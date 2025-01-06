using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using CommunityToolkit.Maui.Converters;
using ZoEazy.Models;

namespace ZoEazy.Converters;

public class TimeSelectedArgsConverter
    (string viewName) : BaseConverterOneWay<TimeChangedEventArgs, object?>
{
    public override object? DefaultConvertReturnValue { get; set; } = null;

    [return: NotNullIfNotNull(nameof(value))]
    public override EventArgs? ConvertFrom(TimeChangedEventArgs value, CultureInfo? culture = null)
    {
        return new VvTimeChangedEventArgs(viewName, value.NewTime);
    }
}