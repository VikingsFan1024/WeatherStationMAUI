// static using for extension method classes
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

// using directives for precision in what specific classes are employed
using ContentPage = Microsoft.Maui.Controls.ContentPage;
using HourlyForecastViewLoader = TempestMonitor.ViewLoaders.HourlyForecastViewLoader;
using HourlyForecastViewModel = TempestMonitor.ViewModels.HourlyForecastViewModel;
using IServiceProvider = System.IServiceProvider;

namespace TempestMonitor.Pages;

public partial class HourlyForecastPage : ContentPage
{
	public HourlyForecastPage(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<HourlyForecastViewModel>();
        InitializeComponent();
        Content = new HourlyForecastViewLoader(serviceProvider);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as HourlyForecastViewModel)?.OnAppearing();
    }

    protected override void OnDisappearing() {
        base.OnDisappearing();
        (BindingContext as HourlyForecastViewModel)?.OnDisappearing();
    }
}