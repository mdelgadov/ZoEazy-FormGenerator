using System.ComponentModel.DataAnnotations.Schema;
using ZoEazy.Model.Enums;

namespace ZoEazy.Models;

[ComplexType]
public record Money
{
    public Money()
    {
        Amount = 0;
        Currency = Monies.Default;
    }

    public Money(decimal amount)
    {
        Amount = amount;
        Currency = Monies.Default;
    }

    public decimal? Amount { get; set; }

    public Currencies? Currency { get; set; }
    public int ChargeInCents
    {
        get
        {
            return  (Amount is not null) ? (int)Math.Floor((decimal)Amount * 100) : 0;
        }
    }

    public Tuple<decimal, Currency> GetMoney(decimal amount)
    {
        return new Tuple<decimal, Currency>(amount, new Currency());
    }

    /// <exception cref="Exception">Invalid currency</exception>
    public Tuple<decimal, Currency> GetMoney(decimal amount, int currencyId)
    {
        var currency = new Currency(currencyId);
        if (currencyId != currency.Id) throw new Exception("Invalid currency");

        return new Tuple<decimal, Currency>(amount, currency);
    }

    public Tuple<decimal, Currency> GetMoney(decimal amount, string name)
    {
        var currency = new Currency(name);
        if (name != currency?.Name && name != currency?.Code && name != currency?.Short && name != currency?.Short &&
            name != currency?.Symbol)
            throw new Exception("Invalid currency");

        return new Tuple<decimal, Currency>(amount, new Currency());
    }
}