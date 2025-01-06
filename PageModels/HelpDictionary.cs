namespace ZoEazy.PageModels;

public class HelpDictionary
{
    protected Dictionary<string, string> helpMessages = new Dictionary<string, string>();
    public Dictionary<string, string> HelpMessages()
    {
        return helpMessages;
    }
}