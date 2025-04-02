using Microsoft.Extensions.DependencyInjection;
using TempestMonitor.ViewModels;

namespace TempestMonitor.Views.HourlyForecastDeviceViews;

public partial class HourlyForecastView1920x1200 : ContentView
{
    public HourlyForecastView1920x1200(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<HourlyForecastViewModel>();
        InitializeComponent();
    }
}