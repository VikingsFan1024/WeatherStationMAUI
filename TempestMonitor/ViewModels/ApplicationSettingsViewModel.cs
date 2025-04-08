namespace TempestMonitor.ViewModels;
sealed partial class ApplicationSettingsViewModel(IServiceProvider serviceProvider) : 
    INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private readonly SettingsModel _settings = serviceProvider.GetRequiredService<SettingsModel>();

    public void OnDisappearing()
    {
        _settings.SaveSettings();
    }
#pragma warning disable CA1822 // Mark members as static
    public string[] DistanceUnitOptions => SettingsModel.DistanceUnitOptions;
    public string[] PrecipitationUnitOptions => SettingsModel.PrecipitationUnitOptions;
    public string[] PressureUnitOptions => SettingsModel.PressureUnitOptions;
    public string[] TemperatureUnitOptions => SettingsModel.TemperatureUnitOptions;
    public string[] TimeFormatOptions => SettingsModel.TimeFormatOptions;
    public string[] SpeedUnitOptions => SettingsModel.WindspeedUnitOptions;
#pragma warning restore CA1822 // Mark members as static
    public string DatabaseFolder
    {
        get => _settings.DatabaseFolder;
        set
        {
            _settings.DatabaseFolder = value;
            OnPropertyChanged();
        }
    }
    public string DistanceUnit
    {
        get => _settings.DistanceUnit.ToString();
        set
        {
            _settings.DistanceUnit = value;
            OnPropertyChanged();
        }
    }
    public string LogFolder
    {
        get => _settings.LogFolder;
        set
        {
            _settings.LogFolder = value;
            OnPropertyChanged();
        }
    }
    public string PrecipitationUnit
    {
        get => _settings.PrecipitationUnit;
        set
        {
            _settings.PrecipitationUnit = value;
            OnPropertyChanged();
        }
    }
    public string PressureUnit
    {
        get => _settings.PressureUnit;
        set
        {
            _settings.PressureUnit = value;
            OnPropertyChanged();
        }
    }
    public string SpeedUnit
    {
        get => _settings.WindspeedUnit;
        set
        {
            _settings.WindspeedUnit = value;
            OnPropertyChanged();
        }
    }
    public string TemperatureUnit
    {
        get => _settings.TemperatureUnit;
        set
        {
            _settings.TemperatureUnit = value;
            OnPropertyChanged();
        }
    }
    public long TimeBetweenHttpRequestsInMinutes
    {
        get => _settings.TimeBetweenHttpRequestsInMinutes;
        set
        {
            _settings.TimeBetweenHttpRequestsInMinutes = value;
            OnPropertyChanged();
        }
    }
    public string TimeFormat
    {
        get => _settings.TimeFormat.ToString();
        set
        {
            _settings.TimeFormat = value;
            OnPropertyChanged();
        }
    }
    public string RestAPIKey
    {
        get => _settings.RestAPIKey;
        set
        {
            _settings.RestAPIKey = value;
            OnPropertyChanged();
        }
    }
    public string StationID
    {
        get => _settings.StationID;
        set
        {
            _settings.StationID = value;
            OnPropertyChanged();
        }
    }
}
