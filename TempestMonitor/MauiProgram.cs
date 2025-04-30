using Microsoft.Maui.Controls.Hosting;
using TempestMonitor.Services;  // ToDo: Needed for UseMauiApp<App> in MauiApp.CreateBuilder() but unable to get to work with

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
        mauiAppBuilder.Services.AddSingleton<ApplicationSettingsPage>();
        mauiAppBuilder.Services.AddSingleton<ApplicationSettingsViewModel>();
        mauiAppBuilder.Services.AddSingleton<ApplicationStatisticsPage>();
        mauiAppBuilder.Services.AddSingleton<ApplicationStatisticsViewModel>();
        mauiAppBuilder.Services.AddSingleton<AzureMongoDBService>();
        mauiAppBuilder.Services.AddSingleton<DailyForecastPage>();
        mauiAppBuilder.Services.AddSingleton<DailyForecastViewModel>();
        mauiAppBuilder.Services.AddSingleton<DatabaseService>();
        mauiAppBuilder.Services.AddSingleton<ExportPage>();
        //mauiAppBuilder.Services.AddSingleton<ExportViewModel>();
        mauiAppBuilder.Services.AddSingleton<ForecastPage>();
        mauiAppBuilder.Services.AddSingleton<ForecastViewModel>();
        mauiAppBuilder.Services.AddSingleton<ForegroundServiceHandler>();
        mauiAppBuilder.Services.AddSingleton<HistoryPage>();
        mauiAppBuilder.Services.AddSingleton<HistoryViewModel>();
        mauiAppBuilder.Services.AddSingleton<HourlyForecastPage>();
        mauiAppBuilder.Services.AddSingleton<HourlyForecastViewModel>();
        mauiAppBuilder.Services.AddSingleton<IFolderPicker>(FolderPicker.Default);
        mauiAppBuilder.Services.AddSingleton<LiveStationReadingsPage>();
        mauiAppBuilder.Services.AddSingleton<LiveStationReadingsViewModel>();
        mauiAppBuilder.Services.AddSingleton<MainPage>();
        mauiAppBuilder.Services.AddSingleton<MainViewModel>();
        mauiAppBuilder.Services.AddSingleton<ReadingBroadcastService>();
        mauiAppBuilder.Services.AddSingleton<ReadingsListenerService>();
        mauiAppBuilder.Services.AddSingleton<RequestForecastsService>();
        mauiAppBuilder.Services.AddSingleton<SettingsModel>();
        mauiAppBuilder.Services.AddSingleton<SQLiteDBService>();

        return mauiAppBuilder;
    }
}
