namespace TempestMonitor.Pages;

public partial class LiveStationReadingsPage : ContentPage
{
	public LiveStationReadingsPage(IServiceProvider serviceProvider)
	{
        BindingContext = serviceProvider.GetRequiredService<LiveStationReadingsViewModel>();
        
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as LiveStationReadingsViewModel)?.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as LiveStationReadingsViewModel)?.OnDisappearing();
    }
}