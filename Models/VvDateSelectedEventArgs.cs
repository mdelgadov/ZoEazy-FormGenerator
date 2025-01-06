namespace ZoEazy.Models;

public class VvDateSelectedEventArgs(string elementName, DateTime oldDate, DateTime newDate) : EventArgs
{
    public DateTime  OldDate { get; init; } = oldDate;
    public DateTime NewDate { get; init; } = newDate;

    public string ElementName { get; init; } = elementName;
}