namespace MauiDemoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Follow the device's light / dark setting. Pages pick their colors from
            // DemoTheme when they're built, so when the setting changes, rebuild the
            // app's pages (this returns to the home screen).
            // The user can pin light or dark in Settings; "system" leaves it Unspecified.
            UserAppTheme = SettingsPage.SavedAppearance;
            RequestedThemeChanged += (s, e) =>
                Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
                {
                    bool reopenSettings = SettingsPage.ReopenAfterThemeChange;
                    SettingsPage.ReopenAfterThemeChange = false;
                    foreach (var window in Windows)
                    {
                        var shell = new AppShell();
                        window.Page = shell;
                        // Changed from the Settings page: land back on it, not on home.
                        // Push once the new shell is on screen (iOS ignores a push
                        // made before it has loaded).
                        if (reopenSettings)
                            shell.Loaded += async (ls, le) => await shell.Navigation.PushAsync(new SettingsPage(), false);
                    }
                });

            Emgu.CV.Platform.Maui.MauiInvoke.Init();

            // Ask before any model download, showing the total size. Runs on the
            // main thread (the download happens on a background thread).
            Emgu.Util.FileDownloadManager.DownloadConfirmation = (long bytes) =>
                Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync(() =>
                {
                    double mb = bytes / (1024.0 * 1024.0);
                    string size = bytes <= 0 ? "some"
                        : mb >= 1024 ? $"{mb / 1024.0:0.0} GB"
                        : mb >= 1 ? $"{mb:0} MB"
                        : $"{bytes / 1024.0:0} KB";
                    Page? page = Shell.Current;
                    if (page == null)
                        return System.Threading.Tasks.Task.FromResult(true);
                    return page.DisplayAlertAsync(
                        "Download model?",
                        $"This feature needs to download about {size} of model files (uses internet data). Download now?",
                        "Download", "Cancel");
                });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}