using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using ContentView = Microsoft.Maui.Controls.ContentView;
using IServiceProvider = System.IServiceProvider;
using MainViewModel = TempestMonitor.ViewModels.MainViewModel;

namespace TempestMonitor.Views.MainDeviceViews;
public partial class MainView2485x970 : ContentView
{
	public MainView2485x970(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<MainViewModel>();
        InitializeComponent();
    }
}