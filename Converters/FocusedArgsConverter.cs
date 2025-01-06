using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace ZoEazy.Converters;

public class FocusedArgsConverter : BaseConverterOneWay<FocusEventArgs, object?>
{
    public override object? DefaultConvertReturnValue { get; set; } = null;

    [return: NotNullIfNotNull(nameof(value))]
    public override EventArgs? ConvertFrom(FocusEventArgs value, CultureInfo? culture = null)
    {
        return new FocusEventArgs(value.VisualElement, value.IsFocused);
    }
}