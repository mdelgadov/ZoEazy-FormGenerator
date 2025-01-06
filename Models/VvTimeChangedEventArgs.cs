namespace ZoEazy.Models;

public class VvTimeChangedEventArgs(string elementName, TimeSpan newTime) : EventArgs
{
    public TimeSpan NewTime { get; private set; } = newTime;

    public string ElementName { get; init; } = elementName;
}