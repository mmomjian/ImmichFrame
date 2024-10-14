﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Avalonia;
using Avalonia.Android;
using ImmichFrame.Helpers;

namespace ImmichFrame.Android;

[Activity(
    Label = "ImmichFrame",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/AppIcon",
    Name = "com.immichframe.immichframe.MainActivity",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Window!.AddFlags(WindowManagerFlags.KeepScreenOn);
        Window!.AddFlags(WindowManagerFlags.Fullscreen);
    }
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }
    public override bool OnKeyDown(Keycode keyCode, KeyEvent? e)
    {
        if (PlatformDetector.IsScreenSaverMode)
        {
            Finish();
            return true;
        }
        else
        {
            return base.OnKeyDown(keyCode, e);
        }
    }
}
