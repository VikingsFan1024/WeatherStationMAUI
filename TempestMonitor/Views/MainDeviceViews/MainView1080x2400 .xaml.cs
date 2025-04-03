using IServiceProvider = System.IServiceProvider;
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

using TempestMonitor.ViewModels;

using ContentView = Microsoft.Maui.Controls.ContentView;

namespace TempestMonitor.Views.MainDeviceViews;

public partial class MainView1080x2400 : ContentView
{
	public MainView1080x2400(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<MainViewModel>();
        InitializeComponent();
    }
}