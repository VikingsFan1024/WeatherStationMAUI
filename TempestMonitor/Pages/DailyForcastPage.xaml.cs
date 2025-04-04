using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using DailyForecastViewModel = TempestMonitor.ViewModels.DailyForecastViewModel;
using DailyForecastViewLoader = TempestMonitor.ViewLoaders.DailyForecastViewLoader;

namespace TempestMonitor.Pages;

using ContentPage = Microsoft.Maui.Controls.ContentPage;

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