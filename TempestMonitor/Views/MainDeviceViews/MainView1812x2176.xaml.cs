using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using ContentView = Microsoft.Maui.Controls.ContentView;
using IServiceProvider = System.IServiceProvider;
using MainViewModel = TempestMonitor.ViewModels.MainViewModel;

namespace TempestMonitor.Views.MainDeviceViews;

public partial class MainView1812x2176 : ContentView
{
	public MainView1812x2176(IServiceProvider serviceProvider)
	{
        BindingContext = serviceProvider.GetRequiredService<MainViewModel>();
        InitializeComponent();
	}
}