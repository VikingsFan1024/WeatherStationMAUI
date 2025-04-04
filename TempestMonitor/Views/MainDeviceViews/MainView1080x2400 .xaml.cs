using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using ContentView = Microsoft.Maui.Controls.ContentView;
using IServiceProvider = System.IServiceProvider;
using MainViewModel = TempestMonitor.ViewModels.MainViewModel;

namespace TempestMonitor.Views.MainDeviceViews;

public partial class MainView1080x2400 : ContentView
{
	public MainView1080x2400(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<MainViewModel>();
        InitializeComponent();
    }
}