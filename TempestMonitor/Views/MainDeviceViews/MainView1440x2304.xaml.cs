// static using for extension method classes
using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;

// using directives for precision in what specific classes are employed
using ContentView = Microsoft.Maui.Controls.ContentView;
using IServiceProvider = System.IServiceProvider;
using MainViewModel = TempestMonitor.ViewModels.MainViewModel;

namespace TempestMonitor.Views.MainDeviceViews;

public partial class MainView1440x2304 : ContentView
{
	public MainView1440x2304(IServiceProvider serviceProvider)
    {
        BindingContext = serviceProvider.GetRequiredService<MainViewModel>();
        InitializeComponent();
    }
}