using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using HistoryViewModel = TempestMonitor.ViewModels.HistoryViewModel;

using ContentPage = Microsoft.Maui.Controls.ContentPage;

namespace TempestMonitor.Pages;

public partial class HistoryPage : ContentPage
{
    public HistoryPage(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<HistoryViewModel>();
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as HistoryViewModel)?.OnAppearing();
    }
}