using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using Microsoft.Extensions.DependencyInjection;
using TempestMonitor.ViewModels;

using ContentView = Microsoft.Maui.Controls.ContentView;

namespace TempestMonitor.Views.MainDeviceViews;

public partial class MainView2176x1812 : ContentView
{
    public MainView2176x1812(IServiceProvider serviceProvider)
	{
        BindingContext = serviceProvider.GetRequiredService<MainViewModel>();
        InitializeComponent();
	}
}