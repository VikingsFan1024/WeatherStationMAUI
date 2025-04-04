using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using ContentView = Microsoft.Maui.Controls.ContentView;
using HourlyForecastViewModel = TempestMonitor.ViewModels.HourlyForecastViewModel;
using IServiceProvider = System.IServiceProvider;

namespace TempestMonitor.Views.HourlyForecastDeviceViews;

public partial class HourlyForecastView1812x2176 : ContentView
{
    public HourlyForecastView1812x2176(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<HourlyForecastViewModel>();
        InitializeComponent();
    }
}