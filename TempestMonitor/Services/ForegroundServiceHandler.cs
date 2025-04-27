namespace TempestMonitor.Services;

public class ForegroundServiceHandler
{
    readonly RequestForecastsService _collectForecastService;
    readonly ReadingsListenerService _collectReadingsService;
    //readonly DeleteMeDatabasePersistenceService _saveJsonFilesToDBService;
    readonly ReadingBroadcastService _readingBroadcastService;

    private int _registrationCount;

    public ForegroundServiceHandler(IServiceProvider serviceProvider)
    {
        _collectForecastService = serviceProvider.GetRequiredService<RequestForecastsService>();
        _collectReadingsService = serviceProvider.GetRequiredService<ReadingsListenerService>();
        //_saveJsonFilesToDBService = serviceProvider.GetRequiredService<DeleteMeDatabasePersistenceService>();
        _readingBroadcastService = serviceProvider.GetRequiredService<ReadingBroadcastService>();
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
        _collectReadingsService.Start();
        _collectForecastService.Start();
        //_saveJsonFilesToDBService.Start();
        _readingBroadcastService.Start();
    }
    private void Stop()
    {
        _collectReadingsService.Stop();
        _collectForecastService.Stop();
        //_saveJsonFilesToDBService.Stop();
        _readingBroadcastService.Stop();
    }
}
