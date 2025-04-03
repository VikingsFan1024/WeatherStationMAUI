using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using Microsoft.Extensions.DependencyInjection;
using TempestMonitor.ViewModels;

using ContentView = Microsoft.Maui.Controls.ContentView;

namespace TempestMonitor.Views.HourlyForecastDeviceViews;

public partial class HourlyForecastView2176x1812 : ContentView
{
    public HourlyForecastView2176x1812(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<HourlyForecastViewModel>();
        InitializeComponent();
    }
}