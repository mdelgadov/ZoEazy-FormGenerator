using CommunityToolkit.Mvvm.ComponentModel;

namespace ZoEazy.PageModels;

public partial class ProfileMessages : ObservableObject
{
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _firstName = string.Empty;
    [ObservableProperty] private string _lastName = string.Empty;
    [ObservableProperty] private string _appointmentDate = string.Empty;
    [ObservableProperty] private string _birthDate = string.Empty;
    [ObservableProperty] private string _appointmentTime = string.Empty;
    [ObservableProperty] private string _phoneMain = string.Empty;
    [ObservableProperty] private string _purpose = string.Empty;
    [ObservableProperty] private string _acceptance = "Sign at your own risk";
}