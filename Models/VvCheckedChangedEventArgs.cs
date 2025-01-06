namespace ZoEazy.Models;

public class VvCheckedChangedEventArgs(string elementName, bool isChecked) : EventArgs
{
    public bool IsChecked { get; init; } = isChecked;

    public string ElementName { get; init; } = elementName;
}