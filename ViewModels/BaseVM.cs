using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using ZoEazy.Models;

namespace ZoEazy.ViewModels;

public  partial class BaseVM : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;
    [ObservableProperty]
    private string? _title;

    protected List<string> Properties = new();
    public ButtonBarData?[] ButtonBar;

    public bool HasRequired { get; set; }

    
    protected  IPopupService? _popupService;

    protected readonly Popup? _popup;

    
    protected readonly string? _inputStyle;
    protected ObservableCollection<string> Errors { get; set; } = new();

    protected BaseVM(IConfiguration config)
    {
        
        ButtonBar =
        [
            new ButtonBarData(ResourceForm.Ok, true)
        ];

    }
   
}

public abstract partial class BaseVmValidator : ObservableValidator
{
    [ObservableProperty]
    private bool _isBusy = false;
    [ObservableProperty]
    private string? _title;
}

