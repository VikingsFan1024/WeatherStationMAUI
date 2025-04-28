using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui;

namespace TempestMonitor
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        // custom code below for requesting permission MANAGE_APP_ALL_FILE_ACCESS_PERMISSION
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            if (!Android.OS.Environment.IsExternalStorageManager)
            {
                var intent = new Android.Content.Intent();
                intent.SetAction(Android.Provider.Settings.ActionManageAppAllFilesAccessPermission);
                var uri = Android.Net.Uri.FromParts("package", this.PackageName, null);
                intent.SetData(uri);
                StartActivity(intent);
            }
            base.OnCreate(savedInstanceState);
        }
    }
}
