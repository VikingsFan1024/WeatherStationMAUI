﻿using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Microsoft.Maui.Hosting; // For MauiApplication
using System;

namespace TempestMonitor
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
