using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using ApplicationStatisticsViewModel = TempestMonitor.ViewModels.ApplicationStatisticsViewModel;
using ContentPage = Microsoft.Maui.Controls.ContentPage;

namespace TempestMonitor.Pages;

public partial class ApplicationStatisticsPage : ContentPage
{
    public ApplicationStatisticsPage(IServiceProvider serviceProvider)
	{
        BindingContext = serviceProvider.GetRequiredService<ApplicationStatisticsViewModel>();
        InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ApplicationStatisticsViewModel)?.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as ApplicationStatisticsViewModel)?.OnDisappearing();
    }
}