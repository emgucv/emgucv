//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.IO;

using Emgu.CV;

using Microsoft.Maui.Controls.Shapes;

namespace MauiDemoApp
{
    /// <summary>
    /// Shared, on-theme scaffolding for the "generate / process → show an image"
    /// demos (Hello World, Planar Subdivision, Feature Matching, Unicode
    /// Rendering) so they match the model pages' look. It provides the shared
    /// DemoTheme header, a scrolling body that subclasses fill with flush sections
    /// separated by thin dividers, a loading overlay, and styled builders (section
    /// headers, captions, primary button, option pills) plus the same collapsible
    /// "About this module" section.
    /// </summary>
    public abstract class SimpleDemoPage : ContentPage
    {
        private readonly VerticalStackLayout _body;
        private readonly Grid _loadingOverlay;
        private readonly Label _loadingLabel;
        private readonly ActivityIndicator _loadingIndicator;

        // Bottom-sheet chooser (same style as the Mask R-CNN Change Photo sheet).
        private readonly Grid _sheetOverlay;
        private readonly BoxView _sheetScrim;
        private readonly Border _sheetCard;
        private readonly VerticalStackLayout _sheetList;
        private System.Threading.Tasks.TaskCompletionSource<string> _sheetTcs;
        private bool _sheetAnimating;

        /// <summary>Category shown in the header of the image / geometry demos.</summary>
        protected const string ImageCategory = "Image & 3D";

        protected SimpleDemoPage(string title, string subtitle, string glyph, string category = null, bool showInfo = true)
        {
            Title = title;
            BackgroundColor = DemoTheme.PageBackground;
            Shell.SetNavBarIsVisible(this, false);

            // Same header as the model pages: plain icons, a giant lowercase title, then category.
            var header = DemoTheme.PageHeader(this, title, subtitle, glyph, null, category, showInfo);

            _body = new VerticalStackLayout
            {
                Spacing = 20,
                Padding = new Thickness(20, 16, 20, 28),
                Children = { header }
            };

            _loadingIndicator = new ActivityIndicator { IsRunning = false, Color = DemoTheme.Accent, WidthRequest = 44, HeightRequest = 44, HorizontalOptions = LayoutOptions.Center };
            _loadingLabel = new Label { Text = "Working…", FontFamily = DemoTheme.BodyFont, FontSize = 15, TextColor = DemoTheme.PrimaryText, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            var loadingCard = new Border
            {
                BackgroundColor = DemoTheme.CardBackground,
                Stroke = DemoTheme.Hairline,
                StrokeThickness = 1,
                Padding = new Thickness(28, 22),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = new VerticalStackLayout { Spacing = 14, WidthRequest = 240, Children = { _loadingIndicator, _loadingLabel } }
            };
            _loadingOverlay = new Grid { IsVisible = false, BackgroundColor = DemoTheme.LoadingVeil, Children = { loadingCard } };

            // Bottom-sheet chooser overlay (hidden until ShowChooser is called).
            _sheetList = new VerticalStackLayout { Spacing = 0 };
            _sheetCard = new Border
            {
                BackgroundColor = DemoTheme.CardBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(28, 28, 0, 0) },
                Padding = new Thickness(20, 8, 20, 60),
                Margin = new Thickness(0, 0, 0, -40),
                VerticalOptions = LayoutOptions.End,
                Content = _sheetList
            };
            _sheetScrim = new BoxView { Color = Color.FromArgb("#66000000"), Opacity = 0, Margin = new Thickness(0, -120, 0, -120) };
            var scrimTap = new TapGestureRecognizer();
            scrimTap.Tapped += (s, e) => CloseSheet(null);
            _sheetScrim.GestureRecognizers.Add(scrimTap);
            _sheetOverlay = new Grid { IsVisible = false, Children = { _sheetScrim, _sheetCard } };

            Content = new Grid { Children = { new Microsoft.Maui.Controls.ScrollView { Content = _body }, _loadingOverlay, _sheetOverlay } };
        }

        // ---------- Bottom-sheet chooser ----------

        /// <summary>Show a Mask R-CNN-style bottom sheet and return the chosen value (null = cancelled).</summary>
        protected System.Threading.Tasks.Task<string> ShowChooser(string title, System.Collections.Generic.IList<(string glyph, string label, string value)> options)
        {
            _sheetTcs = new System.Threading.Tasks.TaskCompletionSource<string>();

            _sheetList.Children.Clear();
            _sheetList.Children.Add(new BoxView { WidthRequest = 40, HeightRequest = 5, CornerRadius = 3, Color = DemoTheme.SheetHandle, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 10, 0, 6) });
            _sheetList.Children.Add(new Label { Text = title, FontFamily = DemoTheme.TitleFont, FontSize = 20, TextColor = DemoTheme.PrimaryText, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 0, 6) });
            // An option with a null value is rendered as a section heading.
            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].value == null)
                    _sheetList.Children.Add(SheetSection(options[i].label));
                else
                    _sheetList.Children.Add(SheetRow(options[i].glyph, options[i].label, options[i].value, i + 1 < options.Count && options[i + 1].value != null));
            }

            var cancel = new Border { BackgroundColor = DemoTheme.BannerBackground, Stroke = Colors.Transparent, StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) }, HeightRequest = 52, Margin = new Thickness(0, 14, 0, 0), Content = new Label { Text = "Cancel", FontFamily = DemoTheme.TitleFont, FontSize = 17, TextColor = DemoTheme.Accent, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center } };
            var cancelTap = new TapGestureRecognizer();
            cancelTap.Tapped += (s, e) => CloseSheet(null);
            cancel.GestureRecognizers.Add(cancelTap);
            _sheetList.Children.Add(cancel);

            _sheetOverlay.IsVisible = true;
            _sheetScrim.Opacity = 0;
            _sheetCard.TranslationY = 700;
            _ = System.Threading.Tasks.Task.WhenAll(
                _sheetScrim.FadeToAsync(1, 220, Easing.CubicOut),
                _sheetCard.TranslateToAsync(0, 0, 260, Easing.CubicOut));
            return _sheetTcs.Task;
        }

        private static View SheetSection(string text) => new Label
        {
            Text = text,
            FontFamily = DemoTheme.TitleFont,
            FontSize = 12,
            TextColor = DemoTheme.SecondaryText,
            CharacterSpacing = 1.2,
            Margin = new Thickness(2, 16, 0, 2)
        };

        private View SheetRow(string glyph, string text, string value, bool divider)
        {
            var grid = new Grid { Padding = new Thickness(2, 14), ColumnSpacing = 16, ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) } };
            grid.Add(DemoTheme.MakeIcon(glyph, DemoTheme.Accent, 26), 0, 0);
            grid.Add(new Label { Text = text, FontFamily = DemoTheme.BodyFont, FontSize = 17, TextColor = DemoTheme.PrimaryText, VerticalOptions = LayoutOptions.Center }, 1, 0);
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => CloseSheet(value);
            grid.GestureRecognizers.Add(tap);

            var stack = new VerticalStackLayout();
            stack.Children.Add(grid);
            if (divider)
                stack.Children.Add(new BoxView { HeightRequest = 1, Color = DemoTheme.Hairline, Margin = new Thickness(42, 0, 0, 0) });
            return stack;
        }

        private async void CloseSheet(string value)
        {
            if (_sheetAnimating)
                return;
            _sheetAnimating = true;
            await System.Threading.Tasks.Task.WhenAll(
                _sheetScrim.FadeToAsync(0, 180, Easing.CubicIn),
                _sheetCard.TranslateToAsync(0, 700, 220, Easing.CubicIn));
            _sheetOverlay.IsVisible = false;
            _sheetAnimating = false;
            _sheetTcs?.TrySetResult(value);
        }

        // ---------- Composition API for subclasses ----------

        /// <summary>Append a section to the page body, preceded by a thin divider (like the model pages).</summary>
        protected void AddSection(View section)
        {
            _body.Children.Add(DemoTheme.Divider());
            _body.Children.Add(section);
        }

        /// <summary>Append a view with no divider before it (e.g. the main action button under a section).</summary>
        protected void AddContent(View view) => _body.Children.Add(view);

        /// <summary>Append the collapsible "About this module" section (shared with the detection pages).</summary>
        protected void AddAbout(string about)
        {
            if (!string.IsNullOrWhiteSpace(about))
                AddSection(ModelShowcasePage.BuildAboutCard(about, flush: true));
        }

        protected void SetBusy(bool busy, string message = "Working…")
        {
            _loadingLabel.Text = message;
            _loadingOverlay.IsVisible = busy;
            _loadingIndicator.IsRunning = busy;
        }

        // ---------- Styled builders ----------

        /// <summary>A section title row (small uppercase grey label) with an optional trailing view (pill / stat).</summary>
        protected static View SectionHeader(string title, View trailing = null)
        {
            var label = DemoTheme.SectionLabel(title);
            if (trailing == null)
                return label;
            var grid = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            grid.Add(label, 0, 0);
            grid.Add(trailing, 1, 0);
            return grid;
        }

        protected static Label Caption(string text) => new Label
        {
            Text = text,
            FontFamily = DemoTheme.BodyFont,
            FontSize = 14,
            TextColor = DemoTheme.SecondaryText
        };

        /// <summary>Full-width black pill button (uppercase, letter-spaced) with a leading glyph.</summary>
        protected Button PrimaryButton(string text, string glyph, EventHandler onClick)
        {
            var button = DemoTheme.PrimaryButton(text, glyph);
            button.Clicked += onClick;
            return button;
        }

        protected static ImageSource ToImageSource(Mat m) => MaskRcnnPage.MatToImageSource(m);

        /// <summary>
        /// A row of option pills (e.g. KAZE / SIFT), like the model pills on the model pages:
        /// the selected one is a black pill with white uppercase text, the others grey.
        /// </summary>
        protected static View PillToggle(string[] options, int initial, Action<int> onSelect)
        {
            var labels = new Label[options.Length];
            var pills = new Border[options.Length];
            var row = new HorizontalStackLayout { Spacing = 10 };
            int selected = initial;

            void Refresh()
            {
                for (int i = 0; i < options.Length; i++)
                {
                    bool on = i == selected;
                    pills[i].BackgroundColor = on ? DemoTheme.PrimaryText : DemoTheme.PillBackground;
                    labels[i].TextColor = on ? DemoTheme.OnAccent : DemoTheme.PrimaryText;
                }
            }

            for (int i = 0; i < options.Length; i++)
            {
                labels[i] = new Label { Text = options[i].ToUpperInvariant(), FontFamily = DemoTheme.BodyFont, FontSize = 14, CharacterSpacing = 1, VerticalOptions = LayoutOptions.Center };
                pills[i] = new Border
                {
                    Stroke = Colors.Transparent,
                    StrokeThickness = 0,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                    Padding = new Thickness(20, 11),
                    Content = labels[i]
                };
                int idx = i;
                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) => { selected = idx; Refresh(); onSelect(idx); };
                pills[i].GestureRecognizers.Add(tap);
                row.Children.Add(pills[i]);
            }
            Refresh();
            return row;
        }
    }
}
