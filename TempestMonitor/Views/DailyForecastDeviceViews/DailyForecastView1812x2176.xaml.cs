using TempestMonitor.ViewModels;

namespace TempestMonitor.Views.DailyForecastDeviceViews;

public partial class DailyForecastView1812x2176 : ContentView
{
    public DailyForecastView1812x2176(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<DailyForecastViewModel>();
        InitializeComponent();
    }
}