namespace TempestMonitor.Pages;

public partial class HistoryPage : ContentPage
{
    public HistoryPage(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<HistoryViewModel>();
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as HistoryViewModel)?.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as HistoryViewModel)?.OnDisappearing();
    }
}