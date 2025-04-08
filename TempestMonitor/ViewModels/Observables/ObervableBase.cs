namespace TempestMonitor.ViewModels.Observables;

public class ObervableBase(SettingsModel settings) : ObservableObject
{
    protected readonly SettingsModel _settings = settings;
}
