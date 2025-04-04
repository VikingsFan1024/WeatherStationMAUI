// using directives for precision in what specific classes are employed
using ContentView = Microsoft.Maui.Controls.ContentView;
using DeviceDisplay = Microsoft.Maui.Devices.DeviceDisplay;
using DisplayInfoChangedEventArgs = Microsoft.Maui.Devices.DisplayInfoChangedEventArgs;
using InvalidDataException = System.IO.InvalidDataException;
using IServiceProvider = System.IServiceProvider;
using Log = Serilog.Log;
using MainPage = TempestMonitor.Pages.MainPage;

namespace TempestMonitor.ViewLoaders;

public partial class HomePageViewLoader : ContentView
{
    private readonly IServiceProvider _serviceProvider;
    public HomePageViewLoader(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;

        SetContentView();

        DeviceDisplay.Current.MainDisplayInfoChanged += Current_MainDisplayInfoChanged;
    }
    private void SetContentView()
    {
        var oldContentView = this.Content;
        var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
        var newContentView = PageToDeviceContentView.GetDeviceContentView(
            nameof(MainPage), displayInfo.Width, displayInfo.Height, _serviceProvider
        );

        if (newContentView is null)
        {
            var message = "Could not determine the ContentView type for the main page.";
            Log.Error(message);
            throw new InvalidDataException(message);
        }

        if (oldContentView is null || oldContentView.GetType() != newContentView.GetType())
        {
            this.Content = newContentView;
        }
    }
    private void Current_MainDisplayInfoChanged(object? sender, DisplayInfoChangedEventArgs e)
    {
        SetContentView();
    }
}