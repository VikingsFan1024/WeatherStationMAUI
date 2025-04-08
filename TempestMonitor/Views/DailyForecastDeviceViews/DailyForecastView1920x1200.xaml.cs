namespace TempestMonitor.Views.DailyForecastDeviceViews;

public partial class DailyForecastView1920x1200 : ContentView
{
    public DailyForecastView1920x1200(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<DailyForecastViewModel>();
        InitializeComponent();
    }
}