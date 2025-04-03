using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using Serilog;

using ContentPage = Microsoft.Maui.Controls.ContentPage;
using DeviceDisplay = Microsoft.Maui.Devices.DeviceDisplay;
using DeviceInfo = Microsoft.Maui.Devices.DeviceInfo;
using DeviceType = Microsoft.Maui.Devices.DeviceType;
using DisplayOrientation = Microsoft.Maui.Devices.DisplayOrientation;
using DisplayRotation = Microsoft.Maui.Devices.DisplayRotation;
using DisplayInfoChangedEventArgs = Microsoft.Maui.Devices.DisplayInfoChangedEventArgs;
using InvalidDataException = System.IO.InvalidDataException;

using Microsoft.Maui.Devices;

using TempestMonitor.Pages;

using ContentView = Microsoft.Maui.Controls.ContentView;

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