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
