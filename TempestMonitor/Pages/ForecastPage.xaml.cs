// static using for extension method classes
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

// using directives for precision in what specific classes are employed
using ContentPage = Microsoft.Maui.Controls.ContentPage;
using ForecastViewModel = TempestMonitor.ViewModels.ForecastViewModel;
using IServiceProvider = System.IServiceProvider;

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