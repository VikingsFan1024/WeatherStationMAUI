using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;
using static Serilog.ConsoleLoggerConfigurationExtensions;
using static Serilog.Enrichers.WithCaller.LoggerCallerEnrichmentConfiguration;
using static Serilog.FileLoggerConfigurationExtensions;
using static Serilog.LoggerEnrichmentConfigurationExtensions;
using static Serilog.ThreadLoggerConfigurationExtensions;

using Application = Microsoft.Maui.Controls.Application;
using CompactJsonFormatter = Serilog.Formatting.Compact.CompactJsonFormatter;
using CultureInfo = System.Globalization.CultureInfo;
using IActivationState = Microsoft.Maui.IActivationState;
using IServiceProvider = System.IServiceProvider;
using Log = Serilog.Log;
using LogEventLevel = Serilog.Events.LogEventLevel;
using LoggerConfiguration = Serilog.LoggerConfiguration;
using RollingInterval = Serilog.RollingInterval;
using SettingsModel = TempestMonitor.Models.SettingsModel;
using Window = Microsoft.Maui.Controls.Window;

namespace TempestMonitor;

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
