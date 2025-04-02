using TempestMonitor.ViewModels;

namespace TempestMonitor.Views.DailyForecastDeviceViews;

public partial class DailyForecastView2176x1812 : ContentView
{
    public DailyForecastView2176x1812(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<DailyForecastViewModel>();
        InitializeComponent();
    }
}