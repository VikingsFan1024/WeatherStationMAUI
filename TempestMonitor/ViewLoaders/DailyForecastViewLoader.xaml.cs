using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using DateTime = System.DateTime;

using Serilog;

using TempestMonitor.Pages;

using ContentView = Microsoft.Maui.Controls.ContentView;
using DeviceDisplay = Microsoft.Maui.Devices.DeviceDisplay;
using DisplayInfoChangedEventArgs = Microsoft.Maui.Devices.DisplayInfoChangedEventArgs;
using InvalidDataException = System.IO.InvalidDataException;

//using ContentPage = Microsoft.Maui.Controls.ContentPage;
//using DeviceDisplay = Microsoft.Maui.Devices.DeviceDisplay;
//using DeviceInfo = Microsoft.Maui.Devices.DeviceInfo;
//using DeviceType = Microsoft.Maui.Devices.DeviceType;
//using DisplayOrientation = Microsoft.Maui.Devices.DisplayOrientation;
//using DisplayRotation = Microsoft.Maui.Devices.DisplayRotation;

namespace TempestMonitor.ViewLoaders;

public partial class DailyForecastViewLoader : ContentView
{
    private readonly IServiceProvider _serviceProvider;
    public DailyForecastViewLoader(IServiceProvider serviceProvider)
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
            nameof(DailyForecastPage), displayInfo.Width, displayInfo.Height, _serviceProvider
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