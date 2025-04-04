// using directives for precision in what specific classes are employed
using ObservableObject = CommunityToolkit.Mvvm.ComponentModel.ObservableObject;
using SettingsModel = TempestMonitor.Models.SettingsModel;

namespace TempestMonitor.ViewModels.Observables;

public class ObervableBase(SettingsModel settings) : ObservableObject
{
    protected readonly SettingsModel _settings = settings;
}
