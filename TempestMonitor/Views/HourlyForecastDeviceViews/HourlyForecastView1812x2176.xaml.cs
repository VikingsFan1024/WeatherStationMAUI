using Microsoft.Extensions.DependencyInjection;
using TempestMonitor.ViewModels;

namespace TempestMonitor.Views.HourlyForecastDeviceViews;

public partial class HourlyForecastView1812x2176 : ContentView
{
    public HourlyForecastView1812x2176(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<HourlyForecastViewModel>();
        InitializeComponent();
    }
}