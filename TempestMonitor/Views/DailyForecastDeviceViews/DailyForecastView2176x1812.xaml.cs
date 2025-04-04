using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using ContentView = Microsoft.Maui.Controls.ContentView;
using DailyForecastViewModel = TempestMonitor.ViewModels.DailyForecastViewModel;
using IServiceProvider = System.IServiceProvider;

namespace TempestMonitor.Views.DailyForecastDeviceViews;

public partial class DailyForecastView2176x1812 : ContentView
{
    public DailyForecastView2176x1812(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<DailyForecastViewModel>();
        InitializeComponent();
    }
}