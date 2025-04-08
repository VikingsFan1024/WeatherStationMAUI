namespace TempestMonitor.Pages;
public partial class MainPage : ContentPage
{
    public MainPage(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<MainViewModel>();

        InitializeComponent();

        DeviceDisplay.Current.KeepScreenOn = true;
        LogDisplayInfo();
        Content = new HomePageViewLoader(serviceProvider);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as MainViewModel)?.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as MainViewModel)?.OnDisappearing();
    }

    private static void LogDisplayInfo()
    {
        var iDeviceInfo = DeviceInfo.Current;

        Log.Information(
            "{DeviceType}," +
            "{Platform}," +
            "{Manufacturer}," +
            "{Model}," +
            "{@Version}" +
            "{Idiom}",
            (
                iDeviceInfo.DeviceType switch
                {
                    DeviceType.Physical => "Physical",
                    DeviceType.Virtual => "Virtual",
                    _ => "Unknown"
                }
            ),
            iDeviceInfo.Platform,
            iDeviceInfo.Manufacturer,
            iDeviceInfo.Model,
            iDeviceInfo.Version,
            iDeviceInfo.Idiom
        );

        var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
        Log.Information(
            "{Density}," +
            "{Height}," +
            "{Orientation}," +
            "{RefreshRate}," +
            "{Rotation}" +
            "{Width}",
            displayInfo.Density,
            displayInfo.Height,
            (
                displayInfo.Orientation switch
                {
                    DisplayOrientation.Landscape => "Landscape",
                    DisplayOrientation.Portrait => "Portrait",
                    _ => "Unknown"
                }
            ),
            displayInfo.RefreshRate,
            (
                displayInfo.Rotation switch
                {
                    DisplayRotation.Rotation0 => "Rotation0",
                    DisplayRotation.Rotation90 => "Rotation90",
                    DisplayRotation.Rotation180 => "Rotation180",
                    DisplayRotation.Rotation270 => "Rotation270",
                    _ => "Unknown"
                }
            ),
            displayInfo.Width
        );
    }
}
