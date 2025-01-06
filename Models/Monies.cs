using ZoEazy.Model;
using ZoEazy.Model.Enums;

namespace ZoEazy.Models;

public static class Monies
{
    private static Dictionary<Currencies, Currency?> moniesDictionary = new Dictionary<Currencies, Currency?>();

    static Monies()
    {
        moniesDictionary.Add(Currencies.usd, new Currency((int)Currencies.usd, "USD", "United States Dollar", "Dollar", "$"));
        moniesDictionary.Add(Currencies.cad, new Currency((int)Currencies.cad, "CAD", "Canadian Dollar", "Dollar","$"));
        moniesDictionary.Add(Currencies.mxn, new Currency((int)Currencies.mxn, "MXN", "Mexican Peso", "Peso", "$"));

    }

    public static Currency GetCurrency(int id = (int)Currencies.usd)
    {
        var currency = (Currencies)id;
        return (moniesDictionary[currency] ?? moniesDictionary[Currencies.usd]) ?? new Currency();
    }

    public static Currency? GetCurrency(string name)
    {
        return moniesDictionary.FirstOrDefault(c => c.Value?.Name == name).Value;
    }

    public static Currencies Default => Currencies.usd;
}