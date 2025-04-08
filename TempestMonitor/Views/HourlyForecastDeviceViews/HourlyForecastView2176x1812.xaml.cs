namespace TempestMonitor.Views.HourlyForecastDeviceViews;

public partial class HourlyForecastView2176x1812 : ContentView
{
    public HourlyForecastView2176x1812(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<HourlyForecastViewModel>();
        InitializeComponent();
    }
}