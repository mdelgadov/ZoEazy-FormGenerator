namespace ZoEazy.Services;

public static class StyleResource
{
    public static Color GetColor(string key, Color? fallBack)
    {
        if (Application.Current?.Resources.TryGetValue(key, out object value) == true)
        {
            return (Color)value;
        }
        return fallBack ?? Colors.Transparent;
    }

    public static Color GetColor(string lightKey, string darkKey, Color? fallBack)
    {
        if (Application.Current is null)
        {
            return fallBack ?? Colors.Transparent;
        }

        var key = Application.Current.RequestedTheme == AppTheme.Light ? lightKey : darkKey;

        if (Application.Current.Resources.TryGetValue(key, out object value))
        {
            return (Color)value;
        }
        return fallBack ?? Colors.Transparent;
    }
    public static Brush GetBrush(string key, Brush? fallBack)
    {
        if (Application.Current is null)
            return fallBack ?? (Brush)Colors.Transparent;

        if (Application.Current.Resources.TryGetValue(key, out object value) && value is Brush brushValue)
            return brushValue;

        return fallBack ?? (Brush)Colors.Transparent;
    }


    public static Style? GetStyle(string style)
    {
        if (Application.Current is null) return null;
        return Application.Current.Resources.TryGetValue(style, out object value) ? (Style)value : null;
    }

    public static ImageSource GetImageSource(string key, ImageSource? fallBack = null)
    {
        if (Application.Current?.Resources.TryGetValue(key, out object value) == true)
        {
            return (ImageSource)value;
        }
        return fallBack ?? ImageSource.FromFile("icon.png");
    }
}

// if (LabelStyle is not null) return;
//Application.Current!.Resources.TryGetValue("DoubleLabel", out var value);
//LabelStyle = (Style) value!;