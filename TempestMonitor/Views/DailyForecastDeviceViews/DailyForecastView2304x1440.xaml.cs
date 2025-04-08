namespace TempestMonitor.Views.DailyForecastDeviceViews;

public partial class DailyForecastView2304x1440 : ContentView
{
    public DailyForecastView2304x1440(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<DailyForecastViewModel>();
        InitializeComponent();
    }
}