// static using for extension method classes
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

// using directives for precision in what specific classes are employed
using ContentView = Microsoft.Maui.Controls.ContentView;
using HourlyForecastViewModel = TempestMonitor.ViewModels.HourlyForecastViewModel;
using IServiceProvider = System.IServiceProvider;

namespace TempestMonitor.Views.HourlyForecastDeviceViews;

public partial class HourlyForecastView2176x1812 : ContentView
{
    public HourlyForecastView2176x1812(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<HourlyForecastViewModel>();
        InitializeComponent();
    }
}