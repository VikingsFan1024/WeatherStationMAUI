namespace TempestMonitor.ViewModels;

sealed partial class HistoryViewModel(IServiceProvider serviceProvider) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly DatabaseService _databaseService = serviceProvider.GetRequiredService<DatabaseService>();
    private readonly ForegroundServiceHandler _foregroundServiceHandler = serviceProvider.GetRequiredService<ForegroundServiceHandler>();
    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();

    private ObservableCollectionOfVW_HourlyObservationSummary _vw_HourlyObservationSummaries = [];

    public void OnDisappearing()
    {
        _foregroundServiceHandler.Unregister(this);
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public void OnAppearing()
    {
        _foregroundServiceHandler.Register(this);

        var vw_HourlyObservationSummaryArray = _databaseService.GetObservationSummary(120);
        if (vw_HourlyObservationSummaryArray is not null)
            _vw_HourlyObservationSummaries = new(vw_HourlyObservationSummaryArray);

        OnPropertyChanged(nameof(VW_HourlyObservationSummaries));
        OnPropertyChanged(nameof(Settings));
    }

    public ObservableCollectionOfVW_HourlyObservationSummary VW_HourlyObservationSummaries => _vw_HourlyObservationSummaries;
    public SettingsModel Settings => _settings;
}