namespace ZoEazy.Models;

public class Currency : Entity
{
    public Currency()
    {
        var currency = Monies.GetCurrency();
        Id = currency.Id;
        Code = currency.Code;
        Name = currency.Name;
        Short = currency.Short;
        Symbol = currency.Symbol;
    }
    public Currency(int id)
    {
        var currency = Monies.GetCurrency(id);
        Id = currency.Id;
        Code = currency.Code;
        Name = currency.Name;
        Short = currency.Short;
        Symbol = currency.Symbol;
    }

    public Currency(string name)
    {
        var currency = Monies.GetCurrency(name);
        Id = currency!.Id;
        Code = currency.Code;
        Name = currency.Name;
        Short = currency.Short;
        Symbol = currency.Symbol;
    }
    public Currency(long id, string code, string name, string @short, string symbol)
    {
        Id = id;
        Code = code;
        Name = name;
        Short = @short;
        Symbol = symbol;
    }


    public string Code { get; set; }

    public string Name { get; set; }

    public string Short { get; set; }  

    public string Symbol { get; set; }
}