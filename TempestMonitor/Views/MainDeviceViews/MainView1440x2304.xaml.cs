using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using TempestMonitor.ViewModels;

using ContentView = Microsoft.Maui.Controls.ContentView;

namespace TempestMonitor.Views.MainDeviceViews;

public partial class MainView1440x2304 : ContentView
{
	public MainView1440x2304(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<MainViewModel>();
        InitializeComponent();
    }
}