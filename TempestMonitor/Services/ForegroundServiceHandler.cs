namespace TempestMonitor.Services;

public class ForegroundServiceHandler
{
    readonly AzurePostgreSQLService _azurePostgreSQLService;
    readonly RequestForecastsService _collectForecastService;
    readonly ReadingsListenerService _collectReadingsService;
    readonly ReadingBroadcastService _readingBroadcastService;
    readonly SQLiteDBService _sqliteDBService;

    private int _registrationCount;

    public ForegroundServiceHandler(IServiceProvider serviceProvider)
    {
        _azurePostgreSQLService = serviceProvider.GetRequiredService<AzurePostgreSQLService>();
        _collectForecastService = serviceProvider.GetRequiredService<RequestForecastsService>();
        _collectReadingsService = serviceProvider.GetRequiredService<ReadingsListenerService>();
        _readingBroadcastService = serviceProvider.GetRequiredService<ReadingBroadcastService>();
        _sqliteDBService = serviceProvider.GetRequiredService<SQLiteDBService>();
    }

    public void Register(object registrant)
    {
        if (_registrationCount++ == 0) Start();
    }
    public void Unregister(object registrant)
    {
        if (--_registrationCount == 0) Stop();
    }
    private void Start()
    {
        _azurePostgreSQLService.Start();
        _collectForecastService.Start();
        _collectReadingsService.Start();
        _readingBroadcastService.Start();
        _sqliteDBService.Start();
    }
    private void Stop()
    {
        _azurePostgreSQLService.Stop();
        _collectForecastService.Stop();
        _collectReadingsService.Stop();
        _readingBroadcastService.Stop();
        _sqliteDBService.Stop();
    }
}
