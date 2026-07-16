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
    /// Rendering) so they match the detection pages' look. It provides the themed
    /// header, a scrolling body that subclasses fill with cards, a loading
    /// overlay, and styled builders (cards, section headers, image frame, primary
    /// button) plus the same collapsible "About this module" card.
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

        protected SimpleDemoPage(string title, string subtitle, string glyph)
        {
            Title = title;
            BackgroundColor = MaskRcnnPage.PageBackground;
            Shell.SetNavBarIsVisible(this, false);

            var headerTile = new Border
            {
                WidthRequest = 64,
                HeightRequest = 64,
                BackgroundColor = MaskRcnnPage.CardBackground,
                Stroke = MaskRcnnPage.RowBorder,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(18) },
                VerticalOptions = LayoutOptions.Start,
                Content = MaskRcnnPage.MakeIcon(glyph, MaskRcnnPage.Accent, 32)
            };
            var titleLabel = new Label { Text = title, FontFamily = MaskRcnnPage.TitleFont, FontSize = 30, TextColor = MaskRcnnPage.PrimaryText };
            var subtitleLabel = new Label { Text = subtitle, FontFamily = MaskRcnnPage.BodyFont, FontSize = 15, TextColor = MaskRcnnPage.SecondaryText, Margin = new Thickness(0, 2, 0, 0) };
            var titleStack = new VerticalStackLayout { VerticalOptions = LayoutOptions.Center, Children = { titleLabel, subtitleLabel } };

            var backButton = CircleButton(MaskRcnnPage.GlyphChevronLeft, async () => await Navigation.PopAsync());
            var infoButton = CircleButton(MaskRcnnPage.GlyphInfo, async () => await Navigation.PushAsync(new AboutPage()));

            var topRow = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            topRow.Add(backButton, 0, 0);
            topRow.Add(infoButton, 2, 0);

            var headerBody = new Grid
            {
                ColumnSpacing = 16,
                Margin = new Thickness(0, 12, 0, 0),
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) }
            };
            headerBody.Add(headerTile, 0, 0);
            headerBody.Add(titleStack, 1, 0);

            var header = new VerticalStackLayout { Children = { topRow, headerBody } };

            _body = new VerticalStackLayout
            {
                Spacing = 18,
                Padding = new Thickness(20, 16, 20, 28),
                Children = { header }
            };

            _loadingIndicator = new ActivityIndicator { IsRunning = false, Color = MaskRcnnPage.Accent, WidthRequest = 44, HeightRequest = 44, HorizontalOptions = LayoutOptions.Center };
            _loadingLabel = new Label { Text = "Working…", FontFamily = MaskRcnnPage.BodyFont, FontSize = 15, TextColor = MaskRcnnPage.PrimaryText, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            var loadingCard = new Border
            {
                BackgroundColor = MaskRcnnPage.CardBackground,
                Stroke = MaskRcnnPage.RowBorder,
                StrokeThickness = 1,
                Padding = new Thickness(28, 22),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = new VerticalStackLayout { Spacing = 14, WidthRequest = 240, Children = { _loadingIndicator, _loadingLabel } }
            };
            _loadingOverlay = new Grid { IsVisible = false, BackgroundColor = Color.FromArgb("#B3EEF1F8"), Children = { loadingCard } };

            // Bottom-sheet chooser overlay (hidden until ShowChooser is called).
            _sheetList = new VerticalStackLayout { Spacing = 0 };
            _sheetCard = new Border
            {
                BackgroundColor = MaskRcnnPage.CardBackground,
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
            _sheetList.Children.Add(new BoxView { WidthRequest = 40, HeightRequest = 5, CornerRadius = 3, Color = Color.FromArgb("#D5D8E0"), HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 10, 0, 6) });
            _sheetList.Children.Add(new Label { Text = title, FontFamily = MaskRcnnPage.TitleFont, FontSize = 20, TextColor = MaskRcnnPage.PrimaryText, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 0, 6) });
            // An option with a null value is rendered as a section heading.
            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].value == null)
                    _sheetList.Children.Add(SheetSection(options[i].label));
                else
                    _sheetList.Children.Add(SheetRow(options[i].glyph, options[i].label, options[i].value, i + 1 < options.Count && options[i + 1].value != null));
            }

            var cancel = new Border { BackgroundColor = Color.FromArgb("#F2F2F7"), Stroke = Colors.Transparent, StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) }, HeightRequest = 52, Margin = new Thickness(0, 14, 0, 0), Content = new Label { Text = "Cancel", FontFamily = MaskRcnnPage.TitleFont, FontSize = 17, TextColor = MaskRcnnPage.Accent, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center } };
            var cancelTap = new TapGestureRecognizer();
            cancelTap.Tapped += (s, e) => CloseSheet(null);
            cancel.GestureRecognizers.Add(cancelTap);
            _sheetList.Children.Add(cancel);

            _sheetOverlay.IsVisible = true;
            _sheetScrim.Opacity = 0;
            _sheetCard.TranslationY = 700;
            _ = System.Threading.Tasks.Task.WhenAll(
                _sheetScrim.FadeTo(1, 220, Easing.CubicOut),
                _sheetCard.TranslateTo(0, 0, 260, Easing.CubicOut));
            return _sheetTcs.Task;
        }

        private static View SheetSection(string text) => new Label
        {
            Text = text,
            FontFamily = MaskRcnnPage.TitleFont,
            FontSize = 12,
            TextColor = MaskRcnnPage.SecondaryText,
            CharacterSpacing = 1.2,
            Margin = new Thickness(2, 16, 0, 2)
        };

        /// <summary>A small rounded accent pill button (glyph + text), like the Change Photo pill.</summary>
        protected View PillButton(string glyph, string text, Action onTap)
        {
            var pill = new Border
            {
                BackgroundColor = MaskRcnnPage.TileBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(13) },
                Padding = new Thickness(14, 9),
                VerticalOptions = LayoutOptions.Center,
                Content = new HorizontalStackLayout
                {
                    Spacing = 7,
                    Children =
                    {
                        MaskRcnnPage.MakeIcon(glyph, MaskRcnnPage.Accent, 18),
                        new Label { Text = text, FontFamily = MaskRcnnPage.TitleFont, FontSize = 14, TextColor = MaskRcnnPage.Accent, VerticalOptions = LayoutOptions.Center }
                    }
                }
            };
            var t = new TapGestureRecognizer();
            t.Tapped += (s, e) => onTap();
            pill.GestureRecognizers.Add(t);
            return pill;
        }

        private View SheetRow(string glyph, string text, string value, bool divider)
        {
            var grid = new Grid { Padding = new Thickness(2, 14), ColumnSpacing = 16, ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) } };
            grid.Add(MaskRcnnPage.MakeIcon(glyph, MaskRcnnPage.Accent, 26), 0, 0);
            grid.Add(new Label { Text = text, FontFamily = MaskRcnnPage.BodyFont, FontSize = 17, TextColor = MaskRcnnPage.PrimaryText, VerticalOptions = LayoutOptions.Center }, 1, 0);
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => CloseSheet(value);
            grid.GestureRecognizers.Add(tap);

            var stack = new VerticalStackLayout();
            stack.Children.Add(grid);
            if (divider)
                stack.Children.Add(new BoxView { HeightRequest = 1, Color = MaskRcnnPage.RowBorder, Margin = new Thickness(42, 0, 0, 0) });
            return stack;
        }

        private async void CloseSheet(string value)
        {
            if (_sheetAnimating)
                return;
            _sheetAnimating = true;
            await System.Threading.Tasks.Task.WhenAll(
                _sheetScrim.FadeTo(0, 180, Easing.CubicIn),
                _sheetCard.TranslateTo(0, 700, 220, Easing.CubicIn));
            _sheetOverlay.IsVisible = false;
            _sheetAnimating = false;
            _sheetTcs?.TrySetResult(value);
        }

        // ---------- Composition API for subclasses ----------

        /// <summary>Append a card/section to the page body (in order).</summary>
        protected void AddCard(View card) => _body.Children.Add(card);

        /// <summary>Append the collapsible "About this module" card (shared with the detection pages).</summary>
        protected void AddAbout(string about)
        {
            if (!string.IsNullOrWhiteSpace(about))
                _body.Children.Add(ModelShowcasePage.BuildAboutCard(about));
        }

        protected void SetBusy(bool busy, string message = "Working…")
        {
            _loadingLabel.Text = message;
            _loadingOverlay.IsVisible = busy;
            _loadingIndicator.IsRunning = busy;
        }

        // ---------- Styled builders ----------

        /// <summary>A white rounded content card.</summary>
        protected static Border Card(View content) => new Border
        {
            BackgroundColor = MaskRcnnPage.CardBackground,
            Stroke = Colors.Transparent,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
            Padding = new Thickness(16),
            Content = content
        };

        /// <summary>A card title row with an optional trailing view (badge / stat).</summary>
        protected static View SectionHeader(string title, View trailing = null)
        {
            var label = new Label { Text = title, FontFamily = MaskRcnnPage.TitleFont, FontSize = 19, TextColor = MaskRcnnPage.PrimaryText, VerticalOptions = LayoutOptions.Center };
            if (trailing == null)
                return label;
            var grid = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            grid.Add(label, 0, 0);
            grid.Add(trailing, 1, 0);
            return grid;
        }

        /// <summary>The light rounded frame the result image sits in.</summary>
        protected static Border ImageFrame(Image img) => new Border
        {
            BackgroundColor = MaskRcnnPage.ImageBackground,
            Stroke = MaskRcnnPage.RowBorder,
            StrokeThickness = 1,
            Padding = new Thickness(10),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
            Content = img
        };

        protected static Label Caption(string text) => new Label
        {
            Text = text,
            FontFamily = MaskRcnnPage.BodyFont,
            FontSize = 14,
            TextColor = MaskRcnnPage.SecondaryText
        };

        /// <summary>Full-width accent primary button with a leading glyph.</summary>
        protected Button PrimaryButton(string text, string glyph, EventHandler onClick)
        {
            var button = new Button
            {
                Text = text,
                FontFamily = MaskRcnnPage.TitleFont,
                FontSize = 17,
                BackgroundColor = MaskRcnnPage.Accent,
                TextColor = Colors.White,
                CornerRadius = 16,
                HeightRequest = 56,
                ImageSource = new FontImageSource { FontFamily = MaskRcnnPage.IconFont, Glyph = glyph, Color = Colors.White, Size = 20 },
                ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 8)
            };
            button.Clicked += onClick;
            return button;
        }

        protected static ImageSource ToImageSource(Mat m) => MaskRcnnPage.MatToImageSource(m);

        /// <summary>A 2-plus option segmented pill toggle (e.g. KAZE / SIFT). Manages its own selection visuals.</summary>
        protected static View SegmentedToggle(string[] options, int initial, Action<int> onSelect)
        {
            var labels = new Label[options.Length];
            var cells = new Border[options.Length];
            var inner = new Grid { ColumnSpacing = 4 };
            int selected = initial;

            void Refresh()
            {
                for (int i = 0; i < options.Length; i++)
                {
                    bool on = i == selected;
                    cells[i].BackgroundColor = on ? MaskRcnnPage.Accent : Colors.Transparent;
                    labels[i].TextColor = on ? Colors.White : MaskRcnnPage.PrimaryText;
                }
            }

            for (int i = 0; i < options.Length; i++)
            {
                inner.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                labels[i] = new Label { Text = options[i], FontFamily = MaskRcnnPage.TitleFont, FontSize = 15, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
                cells[i] = new Border { Stroke = Colors.Transparent, StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(11) }, HeightRequest = 40, Content = labels[i] };
                int idx = i;
                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) => { selected = idx; Refresh(); onSelect(idx); };
                cells[i].GestureRecognizers.Add(tap);
                inner.Add(cells[i], i, 0);
            }
            Refresh();

            return new Border
            {
                BackgroundColor = MaskRcnnPage.ImageBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(13) },
                Padding = new Thickness(3),
                Content = inner
            };
        }

        private Border CircleButton(string glyph, Action onTap)
        {
            var cb = new Border
            {
                WidthRequest = 44,
                HeightRequest = 44,
                BackgroundColor = MaskRcnnPage.CardBackground,
                Stroke = MaskRcnnPage.RowBorder,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                VerticalOptions = LayoutOptions.Center,
                Content = MaskRcnnPage.MakeIcon(glyph, MaskRcnnPage.PrimaryText, 22)
            };
            var t = new TapGestureRecognizer();
            t.Tapped += (s, e) => onTap();
            cb.GestureRecognizers.Add(t);
            return cb;
        }
    }
}
