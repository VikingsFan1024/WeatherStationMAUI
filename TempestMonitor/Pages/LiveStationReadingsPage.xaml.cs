// static using for extension method classes
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

// using directives for precision in what specific classes are employed
using ContentPage = Microsoft.Maui.Controls.ContentPage;
using IServiceProvider = System.IServiceProvider;
using LiveStationReadingsViewModel = TempestMonitor.ViewModels.LiveStationReadingsViewModel;

namespace TempestMonitor.Pages;

public partial class LiveStationReadingsPage : ContentPage
{
	public LiveStationReadingsPage(IServiceProvider serviceProvider)
	{
        BindingContext = serviceProvider.GetRequiredService<LiveStationReadingsViewModel>();
        
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as LiveStationReadingsViewModel)?.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as LiveStationReadingsViewModel)?.OnDisappearing();
    }
}