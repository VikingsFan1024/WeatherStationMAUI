using Microsoft.Extensions.DependencyInjection;
using TempestMonitor.ViewModels;

namespace TempestMonitor.Views.MainDeviceViews;

public partial class MainView2176x1812 : ContentView
{
    public MainView2176x1812(IServiceProvider serviceProvider)
	{
        BindingContext = serviceProvider.GetRequiredService<MainViewModel>();
        InitializeComponent();
	}
}