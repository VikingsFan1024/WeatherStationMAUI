using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Devices;

using ContentView = Microsoft.Maui.Controls.ContentView;

using Serilog;
using TempestMonitor.Pages;
using TempestMonitor.Views.MainDeviceViews;
using TempestMonitor.Views.DailyForecastDeviceViews;
using TempestMonitor.Views.HourlyForecastDeviceViews;

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
