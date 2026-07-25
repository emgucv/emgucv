namespace MauiDemoApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Force light mode throughout the app for now. Dark mode can be
            // introduced later; until then every page renders against the
            // light palette used by the home screen.
            UserAppTheme = AppTheme.Light;

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
                    return page.DisplayAlert(
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