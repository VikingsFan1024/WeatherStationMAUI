// static using for extension method classes
using static Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions;
#if DEBUG 
using static Microsoft.Extensions.Logging.DebugLoggerFactoryExtensions;
#endif
using static Microsoft.Maui.Hosting.FontsMauiAppBuilderExtensions;
using static Microsoft.Maui.Hosting.FontCollectionExtensions;
using static CommunityToolkit.Maui.AppBuilderExtensions;

// using directives for precision in what specific classes are employed
using ApplicationSettingsPage = TempestMonitor.Pages.ApplicationSettingsPage;
using ApplicationSettingsViewModel = TempestMonitor.ViewModels.ApplicationSettingsViewModel;
using ApplicationStatisticsPage = TempestMonitor.Pages.ApplicationStatisticsPage;
using ApplicationStatisticsViewModel = TempestMonitor.ViewModels.ApplicationStatisticsViewModel;
using DailyForecastPage = TempestMonitor.Pages.DailyForecastPage;
using DailyForecastViewModel = TempestMonitor.ViewModels.DailyForecastViewModel;
using DatabasePersistenceService = TempestMonitor.Services.DatabasePersistenceService;
using DatabaseService = TempestMonitor.Services.DatabaseService;
using FolderPicker = CommunityToolkit.Maui.Storage.FolderPicker;
using ForecastPage = TempestMonitor.Pages.ForecastPage;
using ForecastViewModel = TempestMonitor.ViewModels.ForecastViewModel;
using ForegroundServiceHandler = TempestMonitor.Services.ForegroundServiceHandler;
using HistoryPage = TempestMonitor.Pages.HistoryPage;
using HistoryViewModel = TempestMonitor.ViewModels.HistoryViewModel;
using HourlyForecastPage = TempestMonitor.Pages.HourlyForecastPage;
using HourlyForecastViewModel = TempestMonitor.ViewModels.HourlyForecastViewModel;
using IFolderPicker = CommunityToolkit.Maui.Storage.IFolderPicker;
using LiveStationReadingsPage = TempestMonitor.Pages.LiveStationReadingsPage;
using LiveStationReadingsViewModel = TempestMonitor.ViewModels.LiveStationReadingsViewModel;
using MainPage = TempestMonitor.Pages.MainPage;
using MainViewModel = TempestMonitor.ViewModels.MainViewModel;
using MauiApp = Microsoft.Maui.Hosting.MauiApp;
using MauiAppBuilder = Microsoft.Maui.Hosting.MauiAppBuilder;
using ReadingsListenerService = TempestMonitor.Services.ReadingsListenerService;
using RequestForecastsService = TempestMonitor.Services.RequestForecastsService;
using SettingsModel = TempestMonitor.Models.SettingsModel;
using TemperatureUnits = RedStar.Amounts.StandardUnits.TemperatureUnits;
using UnitManager = RedStar.Amounts.UnitManager;

//using static Microsoft.Maui.Controls.Hosting.AppHostBuilderExtensions;
using Microsoft.Maui.Controls.Hosting;  // ToDo: Needed for UseMauiApp<App> in MauiApp.CreateBuilder() but unable to get to work with
    // static using for extension method

namespace TempestMonitor;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .RegisterCustomSingleton()
            .ConfigureFonts(
                fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }
            );
#if DEBUG
        builder.Logging.AddDebug();
#endif
        UnitManager.RegisterByAssembly(typeof(TemperatureUnits).Assembly);
        return builder.Build();
    }
    public static MauiAppBuilder RegisterCustomSingleton(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<IFolderPicker>(FolderPicker.Default);

        mauiAppBuilder.Services.AddSingleton<SettingsModel>();
        mauiAppBuilder.Services.AddSingleton<ForegroundServiceHandler>();
        mauiAppBuilder.Services.AddSingleton<RequestForecastsService>();
        mauiAppBuilder.Services.AddSingleton<ReadingsListenerService>();
        mauiAppBuilder.Services.AddSingleton<DatabaseService>();
        mauiAppBuilder.Services.AddSingleton<DatabasePersistenceService>();

        mauiAppBuilder.Services.AddSingleton<LiveStationReadingsPage>();
        mauiAppBuilder.Services.AddSingleton<LiveStationReadingsViewModel>();

        mauiAppBuilder.Services.AddSingleton<ApplicationSettingsPage>();
        mauiAppBuilder.Services.AddSingleton<ApplicationSettingsViewModel>();

        mauiAppBuilder.Services.AddSingleton<ApplicationStatisticsPage>();
        mauiAppBuilder.Services.AddSingleton<ApplicationStatisticsViewModel>();

        mauiAppBuilder.Services.AddSingleton<DailyForecastPage>();
        mauiAppBuilder.Services.AddSingleton<DailyForecastViewModel>();

        mauiAppBuilder.Services.AddSingleton<ForecastPage>();
        mauiAppBuilder.Services.AddSingleton<ForecastViewModel>();

        mauiAppBuilder.Services.AddSingleton<HourlyForecastPage>();
        mauiAppBuilder.Services.AddSingleton<HourlyForecastViewModel>();

        mauiAppBuilder.Services.AddSingleton<HistoryPage>();
        mauiAppBuilder.Services.AddSingleton<HistoryViewModel>();

        mauiAppBuilder.Services.AddSingleton<MainPage>();
        mauiAppBuilder.Services.AddSingleton<MainViewModel>();

        return mauiAppBuilder;
    }
}
