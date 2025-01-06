
using System.Resources;

namespace ZoEazy.Models;

public static class Localize
{
    //private static readonly ResourceManager RM;
    private static readonly ResourceManager RM = ResourceForm.ResourceManager;

    public static string? Get(string name)
    {
        return RM.GetString(name) ?? null;
    }
}