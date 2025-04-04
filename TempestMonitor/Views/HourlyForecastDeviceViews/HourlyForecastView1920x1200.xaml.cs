// static using for extension method classes
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

// using directives for precision in what specific classes are employed
using ContentView = Microsoft.Maui.Controls.ContentView;
using HourlyForecastViewModel = TempestMonitor.ViewModels.HourlyForecastViewModel;
using IServiceProvider = System.IServiceProvider;

namespace TempestMonitor.Views.HourlyForecastDeviceViews;

public partial class HourlyForecastView1920x1200 : ContentView
{
    public HourlyForecastView1920x1200(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<HourlyForecastViewModel>();
        InitializeComponent();
    }
}