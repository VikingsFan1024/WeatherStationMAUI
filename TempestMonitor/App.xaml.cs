﻿namespace TempestMonitor;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        var settings = _serviceProvider.GetRequiredService<SettingsModel>();
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                formatter: new CompactJsonFormatter(),
                path: $"{settings.LogFilename}",
                restrictedToMinimumLevel: LogEventLevel.Information,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                buffered: false
             )
            .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
            .Enrich.WithDemystifiedStackTraces()
            .Enrich.WithCaller(true)
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .MinimumLevel.Debug()
            .CreateLogger();

        InitializeComponent();
    }
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
