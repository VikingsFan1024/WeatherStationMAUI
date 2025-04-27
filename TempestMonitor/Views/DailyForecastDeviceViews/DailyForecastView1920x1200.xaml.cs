namespace TempestMonitor.Views.DailyForecastDeviceViews;

public partial class DailyForecastView1920x1200 : ContentView
{
    // ToDo: Find a way to confirm compile time bindings are being used, class check but not properties at compile time?
    public DailyForecastView1920x1200(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<DailyForecastViewModel>();
        InitializeComponent();
    }
}