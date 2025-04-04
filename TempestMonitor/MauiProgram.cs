using SettingsModel = TempestMonitor.Models.SettingsModel;

using ApplicationSettingsViewModel = TempestMonitor.ViewModels.ApplicationSettingsViewModel;
using ApplicationStatisticsViewModel = TempestMonitor.ViewModels.ApplicationStatisticsViewModel;
using DailyForecastViewModel = TempestMonitor.ViewModels.DailyForecastViewModel;
using ForecastViewModel = TempestMonitor.ViewModels.ForecastViewModel;
using HistoryViewModel = TempestMonitor.ViewModels.HistoryViewModel;
using HourlyForecastViewModel = TempestMonitor.ViewModels.HourlyForecastViewModel;
using LiveStationReadingsViewModel = TempestMonitor.ViewModels.LiveStationReadingsViewModel;
using MainViewModel = TempestMonitor.ViewModels.MainViewModel;

using ApplicationSettingsPage = TempestMonitor.Pages.ApplicationSettingsPage;
using ApplicationStatisticsPage = TempestMonitor.Pages.ApplicationStatisticsPage;
using DailyForecastPage = TempestMonitor.Pages.DailyForecastPage;
using ForecastPage = TempestMonitor.Pages.ForecastPage;
using HistoryPage = TempestMonitor.Pages.HistoryPage;
using HourlyForecastPage = TempestMonitor.Pages.HourlyForecastPage;
using LiveStationReadingsPage = TempestMonitor.Pages.LiveStationReadingsPage;
using MainPage = TempestMonitor.Pages.MainPage;

using DatabasePersistenceService = TempestMonitor.Services.DatabasePersistenceService;
using DatabaseService = TempestMonitor.Services.DatabaseService;

using ForegroundServiceHandler = TempestMonitor.Services.ForegroundServiceHandler;
using RequestForecastsService = TempestMonitor.Services.RequestForecastsService;
using ReadingsListenerService = TempestMonitor.Services.ReadingsListenerService;

#if DEBUG
using static Microsoft.Extensions.Logging.DebugLoggerFactoryExtensions;
#endif

using CommunityToolkit.Maui;

using IFolderPicker = CommunityToolkit.Maui.Storage.IFolderPicker;
using FolderPicker = CommunityToolkit.Maui.Storage.FolderPicker;

using UnitManager = RedStar.Amounts.UnitManager;
using TemperatureUnits = RedStar.Amounts.StandardUnits.TemperatureUnits;
using MauiApp = Microsoft.Maui.Hosting.MauiApp;
using Microsoft.Maui.Controls.Hosting; // For UseMauiApp
using Microsoft.Maui.Hosting; // For MauiAppBuilder
using Microsoft.Extensions.DependencyInjection; // For IServiceCollection

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
