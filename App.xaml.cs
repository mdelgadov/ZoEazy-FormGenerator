using System.Windows.Input;

namespace ZoEazy
{
    public partial class App : Application
    {
        public static bool IsDark => Current!.UserAppTheme == AppTheme.Dark;
        public static bool IsLight => Current!.UserAppTheme == AppTheme.Light;
        public static bool IsPhone => DeviceInfo.Idiom == DeviceIdiom.Phone;
        public static bool IsNotPhone => DeviceInfo.Idiom != DeviceIdiom.Phone;
        public static bool IsDesktop => DeviceInfo.Idiom == DeviceIdiom.Tablet || DeviceInfo.Idiom == DeviceIdiom.Desktop;
        public static bool IsNotDesktop => !(DeviceInfo.Idiom == DeviceIdiom.Desktop || DeviceInfo.Idiom == DeviceIdiom.Tablet);
        
#if Android
    public static bool IsAndroid => true;
#else
        public static bool IsAndroid => false;
#endif
#if Windows
    public static bool IsWindows => true;
#else
        public static bool IsWindows => false;
#endif

        public static Dictionary<string, Type> RoutesDictionary { get; private set; } = new Dictionary<string, Type>();
        public ICommand HelpCommand => new Command<string>((url) => Launcher.OpenAsync(url));

        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}