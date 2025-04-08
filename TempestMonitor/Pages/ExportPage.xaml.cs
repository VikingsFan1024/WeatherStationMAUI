namespace TempestMonitor.Pages;

public partial class ExportPage : ContentPage
{
    public ExportPage(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<ExportViewModel>();
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ExportViewModel)?.OnAppearing();
    }
}