// static using for extension method classes
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

// using directives for precision in what specific classes are employed
using ContentView = Microsoft.Maui.Controls.ContentView;
using DailyForecastViewModel = TempestMonitor.ViewModels.DailyForecastViewModel;
using IServiceProvider = System.IServiceProvider;

namespace TempestMonitor.Views.DailyForecastDeviceViews;

public partial class DailyForecastView1812x2176 : ContentView
{
    public DailyForecastView1812x2176(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<DailyForecastViewModel>();
        InitializeComponent();
    }
}