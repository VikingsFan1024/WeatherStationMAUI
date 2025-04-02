using CommunityToolkit.Mvvm.ComponentModel;
using TempestMonitor.Models;

namespace TempestMonitor.ViewModels.Observables;

public class BaseObservable(SettingsModel settings) : ObservableObject
{
    protected readonly SettingsModel _settings = settings;
}
