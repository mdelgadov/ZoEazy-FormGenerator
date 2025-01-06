using System.ComponentModel.DataAnnotations;

namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class NumericAttribute : ValidationAttribute
{
    public NumericAttribute(
        CustomFormat customFormat = CustomFormat.None,
        string? minimum = null,
        string? maximum = null,
        string? specialFormat = null,
        bool isEditable = true,
        string placeHolder = "",
        char symbol = '$',
        string cultureInfo = "",
        string percentageMode = "",
        string? maxDecimal = null
    )
    {
        CustomFormat = customFormat;
        Minimum = minimum;
        Maximum = maximum;
        SpecialFormat = specialFormat;
        IsEditable = isEditable;
        Placeholder = placeHolder;
        Symbol = symbol;
        CultureInfo = cultureInfo;
        PercentageMode = percentageMode;
        MaxDecimal = maxDecimal;
    }

    public CustomFormat CustomFormat { get; }
    public string? Minimum { get; }
    public string? Maximum { get; }
    public string? SpecialFormat { get; }
    public bool IsEditable { get; }
    public string Placeholder { get; }
    public char Symbol { get; }
    public string CultureInfo { get; }
    public string PercentageMode { get; }
    public string? MaxDecimal { get; }

    public static readonly Dictionary<CustomFormat, string?> Metadata = new()
    {
        { CustomFormat.N0, "999,999" },
        { CustomFormat.C0, "999,999" },
        { CustomFormat.P0, "999,999%" },
        { CustomFormat.N1, "999,999.9" },
        { CustomFormat.C1, "999,999.9" },
        { CustomFormat.P1, "999,999.9%" },
        { CustomFormat.N2, "999,999.99" },
        { CustomFormat.C2, "999,999.99" },
    };

    public string Format
    {
        get
        {
            var format = Metadata[CustomFormat] ?? Placeholder;
            var symbol = (CustomFormat == CustomFormat.C0
                          || CustomFormat == CustomFormat.C1
                          || CustomFormat == CustomFormat.C2) ? Symbol : ' ';

            return symbol + format;
        }
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        float amount = 0;
        if ((value is null && Minimum is null) || (value is not int && value is not double && value is not decimal && value is not float && value is not Money))
            return ValidationResult.Success!;

        if (value is Money money)
        {
            if (money.Amount is null && Minimum is null)
                return ValidationResult.Success!;
            if (money.Amount is null)
                return new ValidationResult(GetErrorMessage(null, null));
            amount = (float)money.Amount;
        }
        else
        {
            amount = value switch
            {
                int i => i,
                double d => (float)d,
                decimal d => (float)d,
                float f => f,
                _ => 0
            };
        }

        if (Minimum is not null && float.TryParse(Minimum, out float minValue))
        {
            if (amount < minValue)
                return new ValidationResult(GetErrorMessage(Minimum, ResourceForm.LessThanMin));
        }

        if (Maximum is not null && float.TryParse(Maximum, out float maxValue))
        {
            if (amount > maxValue)
                return new ValidationResult(GetErrorMessage(Maximum, ResourceForm.GreaterThanMax));
        }

        return ValidationResult.Success!;
    }

    private string GetErrorMessage(string? reference, string? optionalMessageName)
    {
        if (ErrorMessageResourceType != null)
        {
            var resourceManager = new System.Resources.ResourceManager(ErrorMessageResourceType);

            if (optionalMessageName is not null)
            {
                var errorMessage = resourceManager.GetString(optionalMessageName);
                if (errorMessage != null) return string.Format(errorMessage, reference!);
            }
            else if (ErrorMessageResourceName != null)
            {
                var errorMessage = resourceManager.GetString(ErrorMessageResourceName);
                if (errorMessage != null) return string.Format(errorMessage, reference!);
            }
        }
        return $"Invalid value {reference}";
    }
}
