using ZoEazy.Resources.Fonts;

namespace ZoEazy.Models;

public static class StandardImages
{
    public static Image Required(Boolean isPassword = false, Boolean isField = true)
    {
        var leftMargin = isPassword ? 42 : isField ? 12 : 5;
        var bottomMargin = isField ? 15 : 5;
        var size = 6;

        return new Image
        {
            WidthRequest = size,
            HeightRequest = size,
            MaximumHeightRequest = size,
            MaximumWidthRequest = size,
            Source = new FontImageSource
            {
                FontFamily = FluentUI.FontFamily,
                Glyph = FluentUI.circle_20_regular,
                Color = Colors.Red
            },
            Margin = new Thickness(0, 0, leftMargin, bottomMargin),
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center
        };
    }
}
