namespace TempestMonitor.Views.HourlyForecastDeviceViews;

public partial class HourlyForecastView2304x1440 : ContentView
{
    public HourlyForecastView2304x1440(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<HourlyForecastViewModel>();
        InitializeComponent();
    }
}