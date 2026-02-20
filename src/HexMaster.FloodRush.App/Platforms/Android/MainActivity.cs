using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace HexMaster.FloodRush.App;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ScreenOrientation = ScreenOrientation.SensorLandscape, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Enable full screen immersive mode
        if (Window != null)
        {
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
#pragma warning disable CA1416
                SetFullScreenForAndroid30AndAbove();
#pragma warning restore CA1416
            }
            else
            {
#pragma warning disable CS0618
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
                    SystemUiFlags.Fullscreen |
                    SystemUiFlags.HideNavigation |
                    SystemUiFlags.Immersive |
                    SystemUiFlags.ImmersiveSticky);
#pragma warning restore CS0618
            }
        }
    }

    [System.Runtime.Versioning.SupportedOSPlatform("android30.0")]
    private void SetFullScreenForAndroid30AndAbove()
    {
        if (Window != null)
        {
#pragma warning disable CA1422
            Window.SetDecorFitsSystemWindows(false);
#pragma warning restore CA1422
            var controller = Window.InsetsController;
            if (controller != null)
            {
                controller.Hide(WindowInsets.Type.StatusBars() | WindowInsets.Type.NavigationBars());
                controller.SystemBarsBehavior = (int)WindowInsetsControllerBehavior.ShowTransientBarsBySwipe;
            }
        }
    }
}
