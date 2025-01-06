

namespace ZoEazy.Extensions;
public static class ViewQuery
{
    public static readonly BindableProperty IdProperty = BindableProperty.CreateAttached(
        "Id", 
        typeof(string),
        typeof(ViewQuery),
        string.Empty);

    public static string GetId(this BindableObject obj)
    {
        return (string)obj.GetValue(IdProperty);
    }

    public static void InsertId(this BindableObject obj, string propertyName, string uiControl = "") 

    {
        if (!string.IsNullOrEmpty(uiControl)) 
            propertyName = $"{propertyName}-{uiControl}";

        obj.SetValue(IdProperty, propertyName);
    }

    public static T? FindByViewQueryId<T>(this View view, string id) 
        where T : VisualElement
    {
        return view.FindInChildrenHierarchy<T>(x => GetId(x) == id);
    }

    public static IEnumerable<T> FindManyByViewQueryId<T>(this View view, string id)
        where T : View
    {
        return view.FindManyInChildrenHierarchy<T>(x => GetId(x) == id);
    }

    public static T? FindByViewQueryIdInVisualTreeDescendants<T>(this View view, string id)
        where T : VisualElement
    {
        return view.GetVisualTreeDescendants().OfType<T>().FirstOrDefault(x => GetId(x) == id);
    }
}
