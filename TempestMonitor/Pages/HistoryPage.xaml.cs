// static using for extension method classes
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

// using directives for precision in what specific classes are employed
using ContentPage = Microsoft.Maui.Controls.ContentPage;
using HistoryViewModel = TempestMonitor.ViewModels.HistoryViewModel;
using IServiceProvider = System.IServiceProvider;

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