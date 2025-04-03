using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using TempestMonitor.ViewModels;

using ContentView = Microsoft.Maui.Controls.ContentView;

namespace TempestMonitor.Views.MainDeviceViews;

public partial class MainView2304x1440 : ContentView
{
	public MainView2304x1440(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<MainViewModel>();
        InitializeComponent();
    }
}