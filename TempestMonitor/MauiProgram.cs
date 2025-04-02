using Microsoft.Extensions.Logging;
using TempestMonitor.Services;
using CommunityToolkit.Maui;
using TempestMonitor.Models;
using TempestMonitor.ViewModels;
using TempestMonitor.Pages;

using IFolderPicker = CommunityToolkit.Maui.Storage.IFolderPicker;
using FolderPicker = CommunityToolkit.Maui.Storage.FolderPicker;

using UnitManager = RedStar.Amounts.UnitManager;
using TemperatureUnits = RedStar.Amounts.StandardUnits.TemperatureUnits;

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
