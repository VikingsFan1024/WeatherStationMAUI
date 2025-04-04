// static using for extension method classes
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

// using directives for precision in what specific classes are employed
using ContentPage = Microsoft.Maui.Controls.ContentPage;
using DailyForecastViewLoader = TempestMonitor.ViewLoaders.DailyForecastViewLoader;
using DailyForecastViewModel = TempestMonitor.ViewModels.DailyForecastViewModel;
using IServiceProvider = System.IServiceProvider;

namespace TempestMonitor.Pages;

public partial class DailyForecastPage : ContentPage
{
    public DailyForecastPage(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<DailyForecastViewModel>();
        InitializeComponent();
        this.Content = new DailyForecastViewLoader(serviceProvider);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as DailyForecastViewModel)?.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as DailyForecastViewModel)?.OnDisappearing();
    }
}