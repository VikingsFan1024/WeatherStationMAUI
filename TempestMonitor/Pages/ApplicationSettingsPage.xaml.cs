using CommunityToolkit.Maui.Storage;
using Serilog;
using TempestMonitor.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace TempestMonitor.Pages;
public partial class ApplicationSettingsPage : ContentPage
{
    private readonly IFolderPicker? _folderPicker;
    public ApplicationSettingsPage(IServiceProvider serviceProvider)
	{
        if((BindingContext = 
            serviceProvider.GetService<ApplicationSettingsViewModel>()) is null)
                Log.Error("ApplicationSettingsPage BindingContext is null");

        if ((_folderPicker =
            serviceProvider.GetService<IFolderPicker>()) is null)
                Log.Error("ApplicationSettingsPage _folderPicker is null");

        InitializeComponent();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ((ApplicationSettingsViewModel)BindingContext).OnDisappearing();
    }
    private async void LogFolderButton_Clicked(object sender, EventArgs e)
    {
        if (_folderPicker is null) return;

        try
        {
            CancellationTokenSource cancellationTokenSource = new();
            var folderPickerResult = await _folderPicker.PickAsync(cancellationTokenSource.Token);
            folderPickerResult.EnsureSuccess();
#if ANDROID
            await Toast.Make("Restart application for this change to take affect", 
                ToastDuration.Long).Show(cancellationTokenSource.Token);
#endif
            if (folderPickerResult.IsSuccessful)
                if (BindingContext is ApplicationSettingsViewModel applicationSettingsViewModel)
                    applicationSettingsViewModel.LogFolder = folderPickerResult.Folder.Path;
                else
                    Log.Error("LogFolderButton_Clicked BindingContext as ApplicationSettingsViewModel is null");
        }

        catch(Exception ex)
        {
            Log.Error(ex, "Error picking log folder");
#if ANDROID
            await Toast.Make("Error picking log folder", ToastDuration.Long).Show();
#endif
        }
    }
    private async void DatabaseFolderButton_Clicked(object sender, EventArgs e)
    {
        if (_folderPicker is null) return;

        try
        {
            CancellationTokenSource cancellationTokenSource = new();
            var folderPickerResult = await _folderPicker.PickAsync(cancellationTokenSource.Token);
            folderPickerResult.EnsureSuccess();
#if ANDROID
            await Toast.Make("Restart application for this change to take affect",
                ToastDuration.Long).Show(cancellationTokenSource.Token);
#endif
            if (folderPickerResult.IsSuccessful)
                if (BindingContext is ApplicationSettingsViewModel applicationSettingsViewModel)
                    applicationSettingsViewModel.DatabaseFolder = folderPickerResult.Folder.Path;
                else
                    Log.Error("DatabaseFolderButton_Clicked BindingContext as ApplicationSettingsViewModel is null");
        }

        catch (Exception ex)
        {
            Log.Error(ex, "Error picking log folder");
#if ANDROID
            await Toast.Make("Error picking log folder", ToastDuration.Long).Show();
#endif
        }
    }
}