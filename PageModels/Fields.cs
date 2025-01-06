using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ZoEazy.PageModels;

public partial class Fields : ObservableValidator
{
    public new void ValidateAllProperties()
    {
        base.ValidateAllProperties();
    }

    public new void ValidateProperty(object? value, string prop)
    {
        base.ValidateProperty(value, prop);
    }

    public bool HasNoErrors => !HasErrors;
}