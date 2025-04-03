using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using TempestMonitor.ViewModels;

using ContentView = Microsoft.Maui.Controls.ContentView;

namespace TempestMonitor.Views.DailyForecastDeviceViews;

public partial class DailyForecastView2176x1812 : ContentView
{
    public DailyForecastView2176x1812(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<DailyForecastViewModel>();
        InitializeComponent();
    }
}