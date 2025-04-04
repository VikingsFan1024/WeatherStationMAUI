// static using for extension method classes
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

// using directives for precision in what specific classes are employed
using ContentView = Microsoft.Maui.Controls.ContentView;
using DailyForecastViewModel = TempestMonitor.ViewModels.DailyForecastViewModel;
using IServiceProvider = System.IServiceProvider;

namespace TempestMonitor.Views.DailyForecastDeviceViews;

public partial class DailyForecastView2304x1440 : ContentView
{
    public DailyForecastView2304x1440(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<DailyForecastViewModel>();
        InitializeComponent();
    }
}