namespace TempestMonitor.Pages;

public partial class HourlyForecastPage : ContentPage
{
	public HourlyForecastPage(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<HourlyForecastViewModel>();
        InitializeComponent();
        Content = new HourlyForecastViewLoader(serviceProvider);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as HourlyForecastViewModel)?.OnAppearing();
    }

    protected override void OnDisappearing() {
        base.OnDisappearing();
        (BindingContext as HourlyForecastViewModel)?.OnDisappearing();
    }
}