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
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}