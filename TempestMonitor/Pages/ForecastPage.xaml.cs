using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using TempestMonitor.ViewModels;

using ContentPage = Microsoft.Maui.Controls.ContentPage;

namespace TempestMonitor.Pages;

public partial class ForecastPage : ContentPage
{
    public ForecastPage(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<ForecastViewModel>();

        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ForecastViewModel)?.OnAppearing();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as ForecastViewModel)?.OnDisappearing();
    }
}