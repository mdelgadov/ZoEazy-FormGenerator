using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using CommunityToolkit.Maui.Converters;
using ZoEazy.Models;

namespace ZoEazy.Converters;

public class CheckboxArgsConverter(string viewName) : BaseConverterOneWay<CheckedChangedEventArgs, object?>
{
    public override object? DefaultConvertReturnValue { get; set; } = null;

    [return: NotNullIfNotNull(nameof(value))]
    public override EventArgs? ConvertFrom(CheckedChangedEventArgs value, CultureInfo? culture)
    {
        return new VvCheckedChangedEventArgs(viewName, value.Value);
    }
}