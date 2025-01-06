using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using ZoEazy.Models;

namespace ZoEazy.PageModels;

public partial class ProfileFields : Fields, IHuman
{
    public ProfileFields()
    {
        // the default non-null birthdate is converted to a minvalue date
        // which is nonsense for the user, so we move it for the next worse thing, DatTime.Now...
        // considerations like these are the reason why we need to test the UI
        // also, all fields need to be evaluated on this when a simple null won't do
        _email = string.Empty;
        _firstName = string.Empty;
        _lastName = string.Empty;
        _birthDate = null;
        _acceptance = false;
        _phoneMain = string.Empty;
        _purpose = string.Empty;

        _appointmentDate = null; // strings are better left empty... in my humble opinion
        _appointmentTime = TimeOnly.MinValue;
    }

    [ObservableProperty]
    [Display(Name = "Email", ResourceType = typeof(ResourceForm))]
    [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(ResourceForm), ErrorMessageResourceName = "Required")]
    [EmailAddress(ErrorMessageResourceType = typeof(ResourceForm), ErrorMessageResourceName = "InvalidEmail")]
    [MaxLength(40, ErrorMessageResourceType = typeof(ResourceForm), ErrorMessageResourceName = "TooLong")]
    [Keyboard(KeyboardType.Email)]
    private string _email;
    
    [ObservableProperty]
    [Display(Name = "FirstName", ResourceType = typeof(ResourceForm))]
    [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(ResourceForm),
        ErrorMessageResourceName = "Required")]
    private string _firstName;

    [ObservableProperty]
    [Display(Name = "MiddleName", ResourceType = typeof(ResourceForm))] private string? _middleName;

    [ObservableProperty]
    [Display(Name = "LastName", ResourceType = typeof(ResourceForm))]
    [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(ResourceForm),
        ErrorMessageResourceName = "Required")]
    private string _lastName;

    [ObservableProperty]
    [Display(Name = "BirthDate", ResourceType = typeof(ResourceForm))]
    [DynamicDateRange(10, 0, ErrorMessageResourceType = typeof(ResourceForm), ErrorMessageResourceName = "InvalidBirthDate")]
    [DesktopGroup(0)]
    private DateTime? _birthDate;


    [ObservableProperty]
    [Display(Name = "AppointmentDate", ResourceType = typeof(ResourceForm))]
    [DateOnlyValidation(DateValidationType.NumericNull, ErrorMessageResourceType = typeof(ResourceForm), ErrorMessageResourceName = "InvalidDate")]
    [DynamicDateRange(0, 1, ErrorMessageResourceType = typeof(ResourceForm), ErrorMessageResourceName = "WithinOneYear")]
    [DesktopGroup(0)]
    [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(ResourceForm),
        ErrorMessageResourceName = "Required")]
    private DateOnly? _appointmentDate;

    [ObservableProperty]
    [Display(Name = "AppointmentTime", ResourceType = typeof(ResourceForm))]
    [DynamicTimeRange(2, 4, ErrorMessageResourceType = typeof(ResourceForm), ErrorMessageResourceName = "InvalidAppointment")]
    [DesktopGroup(0)]
    private TimeOnly? _appointmentTime;

    [ObservableProperty]
    [Picker("GetPurposes")]
    [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(ResourceForm),
        ErrorMessageResourceName = "Required")]
    [DesktopGroup(1)]
    private string _purpose;

    [ObservableProperty]
    [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(ResourceForm), ErrorMessageResourceName = "Required")]
    [Phone(ErrorMessageResourceType = typeof(ResourceForm), ErrorMessageResourceName = "Invalid")]
    [MaxLength(14, ErrorMessageResourceType = typeof(ResourceForm), ErrorMessageResourceName = "TooLong")]
    [MinLength(14, ErrorMessageResourceType = typeof(ResourceForm), ErrorMessageResourceName = "TooShort")]
    [Keyboard(KeyboardType.Telephone)]
    [DesktopGroup(1)]
    private string _phoneMain;
    
    [ObservableProperty]
    [DesktopGroup(3)]
    
    [PhoneGroup(3)]
    private Gender _gender;

    [ObservableProperty] [DesktopGroup(3)] [PhoneGroup(3)]
    [Numeric(CustomFormat.C2, ErrorMessageResourceType = typeof(ResourceForm), ErrorMessageResourceName = "Required")]
    private int _willingToPay;

    [ObservableProperty]
    [Display(Name = "Terms", ResourceType = typeof(ResourceForm))]
    [Editor("GetTerms", readOnly: true, autoSize: EditorAutoSizeOption.Disabled, height: 100 )]
    [DesktopGroup(4)]
    private string? _terms;

    [ObservableProperty]
    [DesktopGroup(4)]
    [MustBeTrue(ErrorMessageResourceType = typeof(ResourceForm), ErrorMessageResourceName = "MustBeTrue")]
    private bool _acceptance;

    

    [Hidden] public string? Signature { get; set; }
    
    public static List<string> GetPurposes =>
    [
        "Power", "Fame", "Wealth", "Pleasure", "Weirdness", "Happiness, just kidding, this is not offered",
        "Other"
    ];

    public static string GetTerms =>
        "\"Terms and phyneth printeth. The signataire, here by known as the victim and the provider, here by known as the Devil, agree to the following terms:Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.\\r\\n \";";
}
