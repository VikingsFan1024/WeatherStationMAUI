using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using TempestMonitor.ViewModels;

using ContentView = Microsoft.Maui.Controls.ContentView;

namespace TempestMonitor.Views.MainDeviceViews;

public partial class MainView1812x2176 : ContentView
{
	public MainView1812x2176(IServiceProvider serviceProvider)
	{
        BindingContext = serviceProvider.GetRequiredService<MainViewModel>();
        InitializeComponent();
	}
}