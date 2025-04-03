using System.Net.Sockets;
using System.Net;
using TempestMonitor.Models;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using TempestMonitor.Services;
using SQLite;
using System.Diagnostics;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Events;
using Serilog.Sinks.File;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using Serilog.Enrichers.WithCaller;
using System.Globalization;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using Application = Microsoft.Maui.Controls.Application;

using IServiceProvider = System.IServiceProvider;
using IDisposable = System.IDisposable;
using IActivationState = Microsoft.Maui.IActivationState;

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
