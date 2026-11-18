//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using Microsoft.Maui.Controls.Shapes;

namespace MauiDemoApp
{
    /// <summary>
    /// Small settings screen: light / dark appearance, plus links to the system
    /// info and open source license pages.
    /// </summary>
    public class SettingsPage : ContentPage
    {
        internal const string GlyphSettings = "";   // settings (gear)
        private const string GlyphChevronRight = ""; // chevron_right

        private const string AppearanceKey = "appearance";

        /// <summary>
        /// Set while the app rebuilds itself after an appearance change, so it can
        /// bring the user straight back to this page.
        /// </summary>
        internal static bool ReopenAfterThemeChange;

        /// <summary>The saved appearance choice, applied when the app starts.</summary>
        internal static AppTheme SavedAppearance
        {
            get
            {
                switch (Preferences.Default.Get(AppearanceKey, "system"))
                {
                    case "light": return AppTheme.Light;
                    case "dark": return AppTheme.Dark;
                    default: return AppTheme.Unspecified;
                }
            }
        }

        public SettingsPage()
        {
            Shell.SetNavBarIsVisible(this, false);
            BackgroundColor = DemoTheme.PageBackground;

            var body = new VerticalStackLayout
            {
                Padding = new Thickness(20, 16, 20, 28),
                Spacing = 20,
                Children =
                {
                    DemoTheme.PageHeader(this, "settings", null, null, null, null, showInfo: false),
                    DemoTheme.Divider(),
                    DemoTheme.SectionLabel("Appearance"),
                    AppearancePills(),
                    new Label
                    {
                        Text = "System follows your device's light or dark mode.",
                        FontFamily = DemoTheme.BodyFont,
                        FontSize = 13,
                        TextColor = DemoTheme.MutedText
                    },
                    DemoTheme.Divider(),
                    DemoTheme.SectionLabel("Models"),
                    _models,
                    DemoTheme.Divider(),
                    DemoTheme.SectionLabel("Backend"),
                    BackendList(),
                    DemoTheme.Divider(),
                    DemoTheme.SectionLabel("About"),
                    Row("System info", () => Navigation.PushAsync(new AboutPage())),
                    Row("Open source licenses", () => Navigation.PushAsync(new LicensePage())),
                    new Label
                    {
                        Text = "version " + AppInfo.Current.VersionString,
                        FontFamily = DemoTheme.BodyFont,
                        FontSize = 13,
                        TextColor = DemoTheme.MutedText,
                        Margin = new Thickness(0, 8, 0, 0)
                    }
                }
            };

            Content = new ScrollView { Content = body };
            RefreshModels();
        }

        // ---------- Models: what's downloaded, with sizes, and delete ----------

        private readonly VerticalStackLayout _models = new VerticalStackLayout { Spacing = 0 };

        // Every demo downloads its models under <LocalApplicationData>/emgu/<folder>/.
        private static string ModelRoot => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "emgu");

        private static readonly HashSet<string> ModelExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { ".onnx", ".onnx_data", ".traineddata", ".caffemodel", ".prototxt", ".pb", ".pbtxt", ".tflite", ".weights", ".bin", ".names", ".xml", ".yml", ".yaml" };
        private static readonly HashSet<string> PersonalExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { ".png", ".jpg", ".jpeg", ".gif", ".heic", ".webp", ".md", ".pdf", ".doc", ".docx", ".pptx", ".xlsx", ".mov", ".mp4" };

        // On some platforms (Mac Catalyst) this folder is ~/Documents/emgu, where
        // people may keep their own files too. Only treat a folder as a model if it
        // holds model files and nothing that looks like a photo or document, so a
        // delete button never appears next to someone's own folder.
        private static bool IsModelFolder(System.IO.DirectoryInfo d)
        {
            try
            {
                var files = d.EnumerateFiles("*", System.IO.SearchOption.AllDirectories).ToList();
                return files.Any(f => ModelExtensions.Contains(f.Extension))
                    && !files.Any(f => PersonalExtensions.Contains(f.Extension));
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string Pretty(string folder) => folder.Replace('_', ' ').Replace('-', ' ').ToLowerInvariant();

        private static string Size(long bytes) =>
            bytes >= 1L << 30 ? $"{bytes / (double)(1L << 30):0.0} GB"
            : bytes >= 1 << 20 ? $"{bytes / (double)(1 << 20):0.0} MB"
            : $"{Math.Max(1, bytes / 1024)} KB";

        private void RefreshModels()
        {
            _models.Children.Clear();
            var folders = System.IO.Directory.Exists(ModelRoot)
                ? new System.IO.DirectoryInfo(ModelRoot).GetDirectories()
                    .Where(IsModelFolder)
                    .Select(d => (dir: d, bytes: d.EnumerateFiles("*", System.IO.SearchOption.AllDirectories).Sum(f => f.Length)))
                    .Where(x => x.bytes > 0)
                    .OrderByDescending(x => x.bytes)
                    .ToList()
                : new List<(System.IO.DirectoryInfo dir, long bytes)>();

            if (folders.Count == 0)
            {
                _models.Children.Add(new Label { Text = "No models downloaded yet. Each demo downloads its model the first time you run it.", FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.MutedText });
                return;
            }
            foreach (var (dir, bytes) in folders)
            {
                var row = new Grid
                {
                    Padding = new Thickness(0, 8),
                    ColumnSpacing = 12,
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Auto) }
                };
                row.Add(new VerticalStackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label { Text = Pretty(dir.Name), FontFamily = DemoTheme.TitleFont, FontSize = 16, TextColor = DemoTheme.PrimaryText },
                        new Label { Text = $"{dir.EnumerateFiles("*", System.IO.SearchOption.AllDirectories).Count()} files", FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = DemoTheme.MutedText }
                    }
                }, 0, 0);
                row.Add(new Label { Text = Size(bytes), FontFamily = DemoTheme.BodyFont, FontSize = 14, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center }, 1, 0);
                var target = dir;
                row.Add(DemoControls.CircleButton(DemoTheme.GlyphClose, async () =>
                {
                    bool ok = await DisplayAlertAsync("Delete model?",
                        $"Delete \"{Pretty(target.Name)}\" ({Size(bytes)})? The demo downloads it again the next time you run it.", "Delete", "Cancel");
                    if (!ok)
                        return;
                    try { target.Delete(true); }
                    catch (Exception ex) { await DisplayAlertAsync("Could not delete", ex.Message, "OK"); }
                    RefreshModels();
                }, 32), 2, 0);
                _models.Children.Add(row);
            }
            long total = folders.Sum(f => f.bytes);
            _models.Children.Add(new Label { Text = $"{Size(total)} in total · close a demo before deleting its model", FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = DemoTheme.MutedText, Margin = new Thickness(0, 6, 0, 0) });
        }

        // ---------- Backend: what OpenCV DNN can run on, on this device ----------

        private static View BackendList()
        {
            var stack = new VerticalStackLayout { Spacing = 6 };
            try
            {
                if (Emgu.CV.CvInvoke.ConfigDict["HAVE_OPENCV_DNN"] != 0)
                {
                    foreach (var b in Emgu.CV.Dnn.DnnInvoke.AvailableBackends)
                    {
                        var row = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
                        row.Add(new Label { Text = b.Backend.ToString(), FontFamily = DemoTheme.TitleFont, FontSize = 15, TextColor = DemoTheme.PrimaryText }, 0, 0);
                        row.Add(DemoControls.Tag(b.Target.ToString()), 1, 0);
                        stack.Children.Add(row);
                    }
                }
            }
            catch (Exception) { }
            if (stack.Children.Count == 0)
                stack.Children.Add(new Label { Text = "OpenCV DNN isn't available in this build.", FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.MutedText });
            stack.Children.Add(new Label { Text = "What OpenCV's DNN module can use on this device. The demos run on the default backend (the CPU here).", FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = DemoTheme.MutedText, Margin = new Thickness(0, 4, 0, 0) });
            return stack;
        }

        private View AppearancePills()
        {
            string current = Preferences.Default.Get(AppearanceKey, "system");
            var row = new HorizontalStackLayout { Spacing = 10 };
            foreach (string option in new[] { "system", "light", "dark" })
            {
                bool selected = option == current;
                var pill = new Border
                {
                    BackgroundColor = selected ? DemoTheme.Accent : DemoTheme.PillBackground,
                    Stroke = Colors.Transparent,
                    StrokeThickness = 0,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                    Padding = new Thickness(20, 10),
                    Content = new Label
                    {
                        Text = option.ToUpperInvariant(),
                        FontFamily = DemoTheme.BodyFont,
                        FontSize = 13,
                        CharacterSpacing = 1.5,
                        TextColor = selected ? DemoTheme.OnAccent : DemoTheme.PrimaryText
                    }
                };
                string choice = option;
                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) => SetAppearance(choice);
                pill.GestureRecognizers.Add(tap);
                row.Children.Add(pill);
            }
            return row;
        }

        private void SetAppearance(string choice)
        {
            if (choice == Preferences.Default.Get(AppearanceKey, "system"))
                return;
            Preferences.Default.Set(AppearanceKey, choice);

            var app = Application.Current;
            if (app == null)
                return;
            AppTheme before = app.RequestedTheme;
            ReopenAfterThemeChange = true;
            app.UserAppTheme = SavedAppearance;
            // If the colors didn't actually change (e.g. "system" while the device is
            // already dark), no rebuild happens; just refresh the pills here.
            if (app.RequestedTheme == before)
            {
                ReopenAfterThemeChange = false;
                ((VerticalStackLayout)((ScrollView)Content).Content).Children[3] = AppearancePills();
            }
        }

        private View Row(string text, Action onTap)
        {
            var grid = new Grid
            {
                Padding = new Thickness(0, 10),
                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
            };
            grid.Add(new Label { Text = text, FontFamily = DemoTheme.TitleFont, FontSize = 17, TextColor = DemoTheme.PrimaryText, VerticalOptions = LayoutOptions.Center }, 0, 0);
            grid.Add(DemoTheme.MakeIcon(GlyphChevronRight, DemoTheme.MutedText, 22), 1, 0);
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => onTap();
            grid.GestureRecognizers.Add(tap);
            return grid;
        }
    }
}
