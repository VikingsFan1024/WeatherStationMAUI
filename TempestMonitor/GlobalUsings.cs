// static using for extension method classes
#if DEBUG 
global using static Microsoft.Extensions.Logging.DebugLoggerFactoryExtensions;
#endif
global using static CommunityToolkit.Maui.AppBuilderExtensions;
global using static CommunityToolkit.Mvvm.Messaging.IMessengerExtensions;  // for Register method
global using static Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions;
global using static Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions;
global using static Microsoft.Maui.Hosting.FontCollectionExtensions;
global using static Microsoft.Maui.Hosting.FontsMauiAppBuilderExtensions;
global using static Serilog.ConsoleLoggerConfigurationExtensions;
global using static Serilog.Enrichers.WithCaller.LoggerCallerEnrichmentConfiguration;
global using static Serilog.FileLoggerConfigurationExtensions;
global using static Serilog.LoggerEnrichmentConfigurationExtensions;
global using static Serilog.ThreadLoggerConfigurationExtensions;
global using static System.Linq.Enumerable; // For ToArray() by JsonElement.ArrayEnumerator
global using static System.Threading.Tasks.Dataflow.DataflowBlock; // for BufferBlock and ActionBlock methods - SendAsync()

// Aliases for types used in this file to keep the code cleaner
#if ANDROID
// using directives for precision in what specific classes are employed
global using Toast = CommunityToolkit.Maui.Alerts.Toast; // Android uses the CommunityToolkit.Maui.Alerts.Toast for showing toast messages
global using ToastDuration = CommunityToolkit.Maui.Core.ToastDuration; // Android uses the ToastDuration enum for duration
#endif
global using ActionBlockOfByteArray = System.Threading.Tasks.Dataflow.ActionBlock<byte[]>;
//global using ActionBlockOfReadingModel = System.Threading.Tasks.Dataflow.ActionBlock<TempestMonitor.Models.ReadingModel?>;
global using BufferBlockOfByteArray = System.Threading.Tasks.Dataflow.BufferBlock<byte[]>;
global using DictionaryOfStringUnit = System.Collections.Generic.Dictionary<string, RedStar.Amounts.Unit>;
global using ListOfTasks = System.Collections.Generic.List<System.Threading.Tasks.Task>;
global using ObservableCollectionOfObservableDaily = System.Collections.ObjectModel.ObservableCollection<TempestMonitor.ViewModels.Observables.ObservableDaily>;
global using ObservableCollectionOfObservableHourly = System.Collections.ObjectModel.ObservableCollection<TempestMonitor.ViewModels.Observables.ObservableHourly>;
global using ObservableCollectionOfVW_HourlyObservationSummary = System.Collections.ObjectModel.ObservableCollection<TempestMonitor.Models.VW_HourlyObservationSummary>;
global using TaskOfBool = System.Threading.Tasks.Task<bool>;
global using TaskOfByteArray = System.Threading.Tasks.Task<byte[]>;
//global using TransformBlockOfByteArrayToReadingModel = System.Threading.Tasks.Dataflow.TransformBlock<byte[], TempestMonitor.Models.ReadingModel?>;
global using ValueChangedMessageOfTablenameRowId = CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<TempestMonitor.Models.TablenameRowId>;

// global using directives for precision in what specific classes are employed
global using Activator = System.Activator;
global using AggregateException = System.AggregateException;
global using Amount = RedStar.Amounts.Amount;
global using Application = Microsoft.Maui.Controls.Application;
global using ApplicationSettingsPage = TempestMonitor.Pages.ApplicationSettingsPage;
global using ApplicationSettingsViewModel = TempestMonitor.ViewModels.ApplicationSettingsViewModel;
global using ApplicationStatisticsModel = TempestMonitor.Models.ApplicationStatisticsModel;
global using ApplicationStatisticsPage = TempestMonitor.Pages.ApplicationStatisticsPage;
global using ApplicationStatisticsViewModel = TempestMonitor.ViewModels.ApplicationStatisticsViewModel;
global using CallerMemberNameAttribute = System.Runtime.CompilerServices.CallerMemberNameAttribute;
global using CancellationTokenSource = System.Threading.CancellationTokenSource;
global using Colors = Microsoft.Maui.Graphics.Colors;
global using ColumnAttribute = SQLite.ColumnAttribute;
global using CompactJsonFormatter = Serilog.Formatting.Compact.CompactJsonFormatter;
global using ContentPage = Microsoft.Maui.Controls.ContentPage;
global using ContentView = Microsoft.Maui.Controls.ContentView;
global using CultureInfo = System.Globalization.CultureInfo;
global using CurrentConditions = TempestMonitor.Models.WeatherForecastGraph.CurrentConditions; // for CurrentConditionsModel
global using DatabaseBaseModel = TempestMonitor.Models.DatabaseBaseModel;
global using DatabaseTableAndKeyMessage = TempestMonitor.Models.DatabaseTableAndKeyMessage;
global using DailyForecastPage = TempestMonitor.Pages.DailyForecastPage;
//global using DailyForecastView1812x2176 = TempestMonitor.Views.DailyForecastDeviceViews.DailyForecastView1812x2176; // 1812x2176
global using DailyForecastView1920x1200 = TempestMonitor.Views.DailyForecastDeviceViews.DailyForecastView1920x1200; // Default
//global using DailyForecastView2176x1812 = TempestMonitor.Views.DailyForecastDeviceViews.DailyForecastView2176x1812; // 2176x1812
//global using DailyForecastView2304x1440 = TempestMonitor.Views.DailyForecastDeviceViews.DailyForecastView2304x1440; // 2304x1440
global using DailyForecastViewLoader = TempestMonitor.ViewLoaders.DailyForecastViewLoader;
global using DailyForecastViewModel = TempestMonitor.ViewModels.DailyForecastViewModel;
global using Daily = TempestMonitor.Models.WeatherForecastGraph.Daily; // for DailyModel
global using DatabaseService = TempestMonitor.Services.DatabaseService;
global using DataflowBlockOptions = System.Threading.Tasks.Dataflow.DataflowBlockOptions; // for DataflowBlockOptions
global using DataflowLinkOptions = System.Threading.Tasks.Dataflow.DataflowLinkOptions; // for DataflowLinkOptions
global using DateTime = System.DateTime;
global using DateTimeOffset = System.DateTimeOffset;
global using DeviceDisplay = Microsoft.Maui.Devices.DeviceDisplay;
global using DeviceInfo = Microsoft.Maui.Devices.DeviceInfo;
global using DeviceStatusModel = TempestMonitor.Models.DeviceStatusModel; // for DeviceStatusModel
global using DeviceType = Microsoft.Maui.Devices.DeviceType;
global using DisplayInfoChangedEventArgs = Microsoft.Maui.Devices.DisplayInfoChangedEventArgs;
global using DisplayOrientation = Microsoft.Maui.Devices.DisplayOrientation;
global using DisplayRotation = Microsoft.Maui.Devices.DisplayRotation;
global using ElectricUnits = RedStar.Amounts.StandardUnits.ElectricUnits;
global using EnergyUnits = RedStar.Amounts.StandardUnits.EnergyUnits;
global using Environment = System.Environment;
global using EventArgs = System.EventArgs;
//global using Exception = System.Exception;   // When in GlobalUsings.cs and targeting android created a conflict with a HotReload file
global using ExecutionDataflowBlockOptions = System.Threading.Tasks.Dataflow.ExecutionDataflowBlockOptions; // for ExecutionDataflowBlockOptions
global using ExportPage = TempestMonitor.Pages.ExportPage;
global using ExportViewModel = TempestMonitor.ViewModels.ExportViewModel;
global using FileSystem = Microsoft.Maui.Storage.FileSystem;
global using FolderPicker = CommunityToolkit.Maui.Storage.FolderPicker;
//global using ForecastChildModel = TempestMonitor.Models.ForecastChildModel;
//global using ForecastModel = TempestMonitor.Models.OldForecastModel; // for ForecastModel
global using ForecastPage = TempestMonitor.Pages.ForecastPage;
global using ForecastViewModel = TempestMonitor.ViewModels.ForecastViewModel;
global using ForegroundServiceHandler = TempestMonitor.Services.ForegroundServiceHandler;
global using GC = System.GC;
global using HistoryPage = TempestMonitor.Pages.HistoryPage;
global using HistoryViewModel = TempestMonitor.ViewModels.HistoryViewModel;
global using HomePageViewLoader = TempestMonitor.ViewLoaders.HomePageViewLoader;
global using HourlyForecastPage = TempestMonitor.Pages.HourlyForecastPage;
//global using HourlyForecastView1812x2176 = TempestMonitor.Views.HourlyForecastDeviceViews.HourlyForecastView1812x2176; // 1812x2176
global using HourlyForecastView1920x1200 = TempestMonitor.Views.HourlyForecastDeviceViews.HourlyForecastView1920x1200; // 1812x2176
//global using HourlyForecastView2176x1812 = TempestMonitor.Views.HourlyForecastDeviceViews.HourlyForecastView2176x1812; // 2176x1812
//global using HourlyForecastView2304x1440 = TempestMonitor.Views.HourlyForecastDeviceViews.HourlyForecastView2304x1440; // 2304x1440
global using HourlyForecastViewLoader = TempestMonitor.ViewLoaders.HourlyForecastViewLoader;
global using HourlyForecastViewModel = TempestMonitor.ViewModels.HourlyForecastViewModel;
global using Hourly = TempestMonitor.Models.WeatherForecastGraph.Hourly;
global using HttpClient = System.Net.Http.HttpClient;
global using HttpRequestException = System.Net.Http.HttpRequestException;
global using HubStatusModel = TempestMonitor.Models.HubStatusModel;
global using IActivationState = Microsoft.Maui.IActivationState;
global using IDisposable = System.IDisposable;
global using IFolderPicker = CommunityToolkit.Maui.Storage.IFolderPicker;
global using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;
global using IServiceProvider = System.IServiceProvider;
global using IValueConverter = Microsoft.Maui.Controls.IValueConverter;
global using IgnoreAttribute = SQLite.IgnoreAttribute;
global using Int64 = System.Int64;
global using InvalidDataException = System.IO.InvalidDataException;
global using JsonSerializer = System.Text.Json.JsonSerializer;
global using LengthUnits = RedStar.Amounts.StandardUnits.LengthUnits;
global using LightningStrikeModel = TempestMonitor.Models.LightningStrikeModel; // for LightningStrikeModel
global using LiveStationReadingsPage = TempestMonitor.Pages.LiveStationReadingsPage;
global using LiveStationReadingsViewModel = TempestMonitor.ViewModels.LiveStationReadingsViewModel;
global using Log = Serilog.Log;
global using LogEventLevel = Serilog.Events.LogEventLevel;
global using LoggerConfiguration = Serilog.LoggerConfiguration;
global using MainPage = TempestMonitor.Pages.MainPage;
global using MainView1080x2400 = TempestMonitor.Views.MainDeviceViews.MainView1080x2400; // Motorola Moto G 5G Stylus Portrait
global using MainView1440x2304 = TempestMonitor.Views.MainDeviceViews.MainView1440x2304; // 1440x2304
global using MainView1812x2176 = TempestMonitor.Views.MainDeviceViews.MainView1812x2176; // 1812x2176
global using MainView1920x1200 = TempestMonitor.Views.MainDeviceViews.MainView1920x1200; // Default
global using MainView2176x1812 = TempestMonitor.Views.MainDeviceViews.MainView2176x1812; // 2176x1812
global using MainView2304x1440 = TempestMonitor.Views.MainDeviceViews.MainView2304x1440; // 2304x1440
global using MainView2400x1080 = TempestMonitor.Views.MainDeviceViews.MainView2400x1080; // Motorola Moto G 5G Stylus Landscape
global using MainView2485x970 = TempestMonitor.Views.MainDeviceViews.MainView2485x970; // 2485x970
global using MainView970x2485 = TempestMonitor.Views.MainDeviceViews.MainView970x2485; // 970x2485 (Samsung S9 FE 5G? Tablet)
global using MainViewModel = TempestMonitor.ViewModels.MainViewModel;
global using Math = System.Math;
global using MauiApp = Microsoft.Maui.Hosting.MauiApp;
global using MauiAppBuilder = Microsoft.Maui.Hosting.MauiAppBuilder;
global using Mutex = System.Threading.Mutex;
global using NotImplementedException = System.NotImplementedException;
global using ObservableDaily = TempestMonitor.ViewModels.Observables.ObservableDaily;
global using ObservableForecast = TempestMonitor.ViewModels.Observables.ObservableForecast;
global using ObservableHourly = TempestMonitor.ViewModels.Observables.ObservableHourly;
global using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;
global using ObservableVW_WindModel = TempestMonitor.ViewModels.Observables.ObservableVW_WindModel;
global using ObservationModel = TempestMonitor.Models.ObservationModel;
global using OperationCanceledException = System.OperationCanceledException;
global using Path = System.IO.Path;
global using Preferences = Microsoft.Maui.Storage.Preferences;
global using PressureUnits = RedStar.Amounts.StandardUnits.PressureUnits;
global using PrimaryKeyAttribute = SQLite.PrimaryKeyAttribute;
global using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;
global using PropertyChangedEventHandler = System.ComponentModel.PropertyChangedEventHandler;
global using RainStartModel = TempestMonitor.Models.RainStartModel;
global using ReadingBroadcastService = TempestMonitor.Services.ReadingBroadcastService;
global using ReadingsListenerService = TempestMonitor.Services.ReadingsListenerService;
global using RequestForecastsService = TempestMonitor.Services.RequestForecastsService;
global using RollingInterval = Serilog.RollingInterval;
global using SettingsModel = TempestMonitor.Models.SettingsModel;
global using Shell = Microsoft.Maui.Controls.Shell;
global using SocketException = System.Net.Sockets.SocketException;
global using SpeedUnits = RedStar.Amounts.StandardUnits.SpeedUnits;
global using SQLite3 = SQLite.SQLite3;
global using SQLiteConnection = SQLite.SQLiteConnection;
global using Station = TempestMonitor.Models.WeatherForecastGraph.Station;
global using Stopwatch = System.Diagnostics.Stopwatch;
global using StreamReader = System.IO.StreamReader;
global using TableAttribute = SQLite.TableAttribute;
global using TablenameRowId = TempestMonitor.Models.TablenameRowId;
//global using Task = System.Threading.Tasks.Task;   // When in GlobalUsings.cs and targeting android created a conflict with a HotReload file
global using TaskCanceledException = System.Threading.Tasks.TaskCanceledException;
global using TemperatureUnits = RedStar.Amounts.StandardUnits.TemperatureUnits;
global using TimeSpan = System.TimeSpan;
global using TimeUnits = RedStar.Amounts.StandardUnits.TimeUnits;
global using Timer = System.Threading.Timer;
global using TimerCallback = System.Threading.TimerCallback;
//global using Type = System.Type;  // When in GlobalUsings.cs and targeting android created a conflict with a HotReload file
global using UdpClient = System.Net.Sockets.UdpClient;
global using UdpReceiveResult = System.Net.Sockets.UdpReceiveResult;
global using Unit = RedStar.Amounts.Unit;
global using ForecastUnits = TempestMonitor.Models.WeatherForecastGraph.Units;
global using UnitManager = RedStar.Amounts.UnitManager;
global using ValueTaskOfUdpReceiveResult = System.Threading.Tasks.ValueTask<System.Net.Sockets.UdpReceiveResult>;
global using VW_DeviceStatusModel = TempestMonitor.Models.VW_DeviceStatusModel;
global using VW_HubStatusModel = TempestMonitor.Models.VW_HubStatusModel;
global using VW_LightningStrikeModel = TempestMonitor.Models.VW_LightningStrikeModel;
global using VW_ObservationModel = TempestMonitor.Models.VW_ObservationModel;
global using VW_RainStartModel = TempestMonitor.Models.VW_RainStartModel;
global using WeatherForecastGraph = TempestMonitor.Models.WeatherForecastGraph;
global using VW_WindModel = TempestMonitor.Models.VW_WindModel;
global using VW_HourlyObservationSummary = TempestMonitor.Models.VW_HourlyObservationSummary;
global using WeakReferenceMessenger = CommunityToolkit.Mvvm.Messaging.WeakReferenceMessenger;
global using WeatherForecastModel = TempestMonitor.Models.WeatherForecastModel;
global using WindModel = TempestMonitor.Models.WindModel;
global using Window = Microsoft.Maui.Controls.Window;


