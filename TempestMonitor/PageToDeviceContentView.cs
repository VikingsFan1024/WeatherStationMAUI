using Activator = System.Activator;
using Type = System.Type;
using ContentView = Microsoft.Maui.Controls.ContentView;
using DailyForecastPage = TempestMonitor.Pages.DailyForecastPage;
using DailyForecastView1812x2176 = TempestMonitor.Views.DailyForecastDeviceViews.DailyForecastView1812x2176; // 1812x2176
using DailyForecastView1920x1200 = TempestMonitor.Views.DailyForecastDeviceViews.DailyForecastView1920x1200; // Default
using DailyForecastView2176x1812 = TempestMonitor.Views.DailyForecastDeviceViews.DailyForecastView2176x1812; // 2176x1812
using DailyForecastView2304x1440 = TempestMonitor.Views.DailyForecastDeviceViews.DailyForecastView2304x1440; // 2304x1440
using HourlyForecastPage = TempestMonitor.Pages.HourlyForecastPage;
using HourlyForecastView1812x2176 = TempestMonitor.Views.HourlyForecastDeviceViews.HourlyForecastView1812x2176; // 1812x2176
using HourlyForecastView1920x1200 = TempestMonitor.Views.HourlyForecastDeviceViews.HourlyForecastView1920x1200; // Default
using HourlyForecastView2176x1812 = TempestMonitor.Views.HourlyForecastDeviceViews.HourlyForecastView2176x1812; // 2176x1812
using HourlyForecastView2304x1440 = TempestMonitor.Views.HourlyForecastDeviceViews.HourlyForecastView2304x1440; // 2304x1440
using IServiceProvider = System.IServiceProvider;
using Log = Serilog.Log;
using MainPage = TempestMonitor.Pages.MainPage;
using MainView970x2485 = TempestMonitor.Views.MainDeviceViews.MainView970x2485; // 970x2485 (Samsung S9 FE 5G? Tablet)
using MainView1080x2400 = TempestMonitor.Views.MainDeviceViews.MainView1080x2400; // Motorola Moto G 5G Stylus Portrait
using MainView1440x2304 = TempestMonitor.Views.MainDeviceViews.MainView1440x2304; // 1440x2304
using MainView1920x1200 = TempestMonitor.Views.MainDeviceViews.MainView1920x1200; // Default
using MainView1812x2176 = TempestMonitor.Views.MainDeviceViews.MainView1812x2176; // 1812x2176
using MainView2176x1812 = TempestMonitor.Views.MainDeviceViews.MainView2176x1812; // 2176x1812
using MainView2304x1440 = TempestMonitor.Views.MainDeviceViews.MainView2304x1440; // 2304x1440
using MainView2400x1080 = TempestMonitor.Views.MainDeviceViews.MainView2400x1080; // Motorola Moto G 5G Stylus Landscape
using MainView2485x970 = TempestMonitor.Views.MainDeviceViews.MainView2485x970; // 2485x970

namespace TempestMonitor;

public class PageToDeviceContentView
{
    public static int DefaultWidth { get; set; } = 1920;
    public static int DefaultHeight { get; set; } = 1200;
    sealed private class PageContentViewToDeviceData
    {
        public PageContentViewToDeviceData()
        {
        }
        public string? PageName { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public Type? ViewType { get; set; }
    }

    private static readonly PageContentViewToDeviceData[] _displayToMainPageContentView =
    [
        new PageContentViewToDeviceData(){ PageName=nameof(MainPage), Width=1812, Height=2176, ViewType=typeof(MainView1812x2176)},
        new PageContentViewToDeviceData(){ PageName=nameof(MainPage), Width=2176, Height=1812, ViewType=typeof(MainView2176x1812)},

        new PageContentViewToDeviceData(){ PageName=nameof(MainPage), Width=1920, Height=1200, ViewType=typeof(MainView1920x1200)},

        new PageContentViewToDeviceData(){ PageName=nameof(MainPage), Width=2304, Height=1440, ViewType=typeof(MainView2304x1440)},
        new PageContentViewToDeviceData(){ PageName=nameof(MainPage), Width=1440, Height=2304, ViewType=typeof(MainView1440x2304)},

        new PageContentViewToDeviceData(){ PageName=nameof(MainPage), Width=2485, Height=970,  ViewType=typeof(MainView2485x970)},
        new PageContentViewToDeviceData(){ PageName=nameof(MainPage), Width=970,  Height=2485, ViewType=typeof(MainView970x2485)},  // Samsung S9 FE 5G? Tablet

        new PageContentViewToDeviceData(){ PageName=nameof(MainPage), Width=2400, Height=1080, ViewType=typeof(MainView2400x1080)},   //Motorola Moto G 5G Stylus Landscape
        new PageContentViewToDeviceData(){ PageName=nameof(MainPage), Width=1080, Height=2400, ViewType=typeof(MainView1080x2400)},   //Motorola Moto G 5G Stylus Portrait

        new PageContentViewToDeviceData(){ PageName=nameof(DailyForecastPage), Width=1920, Height=1200, ViewType=typeof(DailyForecastView1920x1200)},
        new PageContentViewToDeviceData(){ PageName=nameof(DailyForecastPage), Width=2176, Height=1812, ViewType=typeof(DailyForecastView2176x1812)},
        new PageContentViewToDeviceData(){ PageName=nameof(DailyForecastPage), Width=1812, Height=2176, ViewType=typeof(DailyForecastView1812x2176)},
        new PageContentViewToDeviceData(){ PageName=nameof(DailyForecastPage), Width=2304, Height=1440, ViewType=typeof(DailyForecastView2304x1440)},        

        new PageContentViewToDeviceData(){ PageName=nameof(HourlyForecastPage), Width=1920, Height=1200, ViewType=typeof(HourlyForecastView1920x1200)},
        new PageContentViewToDeviceData(){ PageName=nameof(HourlyForecastPage), Width=2176, Height=1812, ViewType=typeof(HourlyForecastView2176x1812)},
        new PageContentViewToDeviceData(){ PageName=nameof(HourlyForecastPage), Width=1812, Height=2176, ViewType=typeof(HourlyForecastView1812x2176)},
        new PageContentViewToDeviceData(){ PageName=nameof(HourlyForecastPage), Width=2304, Height=1440, ViewType=typeof(HourlyForecastView2304x1440)},
    ];

    public PageToDeviceContentView() { }

    public static ContentView? GetDeviceContentView(string pageName, double width, double height, IServiceProvider serviceProvider)
    {
        var intWidth = (int)width;
        var intHeight = (int)height;
        Type? contentViewType = null;
        foreach (var item in _displayToMainPageContentView)
        {
            if (item.PageName == pageName && item.Width == intWidth && item.Height == intHeight)
            {
                contentViewType = item.ViewType;
                break;
            }
        }

        if (contentViewType is null)
        {
            Log.Warning($"Could not find ContentView for PageName: {pageName}, Width: {width}, Height: {height}");
            Log.Warning($"Using 1920x1200 for PageName: {pageName}");
            return GetDeviceContentView(
                pageName, PageToDeviceContentView.DefaultWidth, PageToDeviceContentView.DefaultHeight, serviceProvider);
        }

        return (ContentView?)Activator.CreateInstance(contentViewType, serviceProvider);
    }
}
