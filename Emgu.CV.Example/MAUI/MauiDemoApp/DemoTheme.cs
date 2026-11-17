//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

namespace MauiDemoApp
{
    /// <summary>
    /// The demo app's shared look: a plain storefront (white with black type, or the reverse in dark mode), grey
    /// details and pill-shaped tags. Every page takes its colors from here.
    /// </summary>
    internal static class DemoTheme
    {
        /// <summary>True when the phone / computer is in dark mode.</summary>
        internal static bool IsDark => Application.Current?.RequestedTheme == AppTheme.Dark;

        private static Color Pick(string light, string dark) => Color.FromArgb(IsDark ? dark : light);

        internal static Color PageBackground => Pick("#FFFFFF", "#0E0E0E");
        internal static Color CardBackground => Pick("#FFFFFF", "#0E0E0E");
        internal static Color PrimaryText => Pick("#111111", "#F2F2F2");
        internal static Color SecondaryText => Pick("#5E5E5E", "#A3A3A3");
        internal static Color MutedText => Pick("#9A9A9A", "#6E6E6E");
        // Buttons, selections and links use the main text color (black, or white in dark mode).
        internal static Color Accent => PrimaryText;
        // Text and icons drawn on top of an Accent-colored pill.
        internal static Color OnAccent => PageBackground;
        internal static Color Hairline => Pick("#E4E4E4", "#2A2A2A");
        // Soft grey fill for pills, tags and icon tiles.
        internal static Color PillBackground => Pick("#EFEFEC", "#222222");
        // Backdrop behind photos and previews.
        internal static Color ImageBackground => Pick("#F5F5F5", "#1A1A1A");
        internal static Color BannerBackground => Pick("#F3F3F3", "#1A1A1A");
        // Half-see-through page color behind loading spinners.
        internal static Color LoadingVeil => Pick("#B3FFFFFF", "#B30E0E0E");
        internal static Color LoadingVeilStrong => Pick("#F5FFFFFF", "#F50E0E0E");
        // The little grab handle at the top of bottom sheets.
        internal static Color SheetHandle => Pick("#D4D4D4", "#444444");

        internal const string BodyFont = "InterRegular";
        internal const string TitleFont = "InterSemiBold";
        internal const string IconFont = "MaterialSymbols";

        // Only glyphs present in the bundled (subset) Material Symbols font work;
        // anything else renders as "?". Reuse these rather than new codepoints.
        internal const string GlyphBack = MaskRcnnPage.GlyphChevronLeft;
        internal const string GlyphInfo = MaskRcnnPage.GlyphInfo;
        internal const string GlyphPlay = MaskRcnnPage.GlyphPlay;
        internal const string GlyphVideo = MaskRcnnPage.GlyphVideo;

        // Pale grey for oversized empty-state type ("no results yet").
        internal static Color EmptyStateText => Pick("#D9D9D9", "#333333");

        internal static Image MakeIcon(string glyph, Color color, double size) => new Image
        {
            Source = new FontImageSource { FontFamily = IconFont, Glyph = glyph, Color = color, Size = size },
            WidthRequest = size,
            HeightRequest = size,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };

        /// <summary>Small uppercase grey heading, e.g. "INPUT IMAGE".</summary>
        internal static Label SectionLabel(string text) => new Label
        {
            Text = text.ToUpperInvariant(),
            FontFamily = BodyFont,
            FontSize = 12,
            CharacterSpacing = 2,
            TextColor = SecondaryText,
            VerticalOptions = LayoutOptions.Center
        };

        /// <summary>Thin rule between page sections.</summary>
        internal static BoxView Divider() => new BoxView { HeightRequest = 1, Color = Hairline };

        /// <summary>Grey pill holding a short tag such as "DNN".</summary>
        internal static Border TagPill(string text) => new Border
        {
            BackgroundColor = PillBackground,
            Stroke = Colors.Transparent,
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(16) },
            Padding = new Thickness(14, 6),
            VerticalOptions = LayoutOptions.Center,
            Content = new Label { Text = text, FontFamily = TitleFont, FontSize = 14, TextColor = PrimaryText }
        };

        /// <summary>Tappable grey pill with uppercase text, e.g. "CHANGE".</summary>
        internal static Border TextPill(string text, Action onTap)
        {
            var pill = new Border
            {
                BackgroundColor = PillBackground,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(20) },
                Padding = new Thickness(20, 9),
                VerticalOptions = LayoutOptions.Center,
                Content = new Label { Text = text.ToUpperInvariant(), FontFamily = BodyFont, FontSize = 13, CharacterSpacing = 1.5, TextColor = PrimaryText }
            };
            var t = new TapGestureRecognizer();
            t.Tapped += (s, e) => onTap();
            pill.GestureRecognizers.Add(t);
            return pill;
        }

        /// <summary>Bare tappable icon (no circle or outline), for the back / info buttons.</summary>
        internal static View IconButton(string glyph, Action onTap)
        {
            var icon = MakeIcon(glyph, PrimaryText, 22);
            var holder = new Grid { WidthRequest = 44, HeightRequest = 44, Children = { icon } };
            var t = new TapGestureRecognizer();
            t.Tapped += (s, e) => onTap();
            holder.GestureRecognizers.Add(t);
            return holder;
        }

        /// <summary>
        /// Standard page header: back / info icons, a giant lowercase title sized to its
        /// length, an icon + tag pill + category row, the bold one-line description and a
        /// small grey module line. Pass null for anything a page doesn't have.
        /// </summary>
        internal static View PageHeader(Page page, string title, string subtitle, string glyph, string tag, string category, bool showInfo = true, View trailing = null)
        {
            var topRow = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            topRow.Add(IconButton(GlyphBack, async () => await page.Navigation.PopAsync()), 0, 0);
            if (trailing != null)
                topRow.Add(trailing, 2, 0);
            else if (showInfo)
                topRow.Add(IconButton(GlyphInfo, async () => await page.Navigation.PushAsync(new AboutPage())), 2, 0);

            // Short names get the biggest type; longer ones step down so they still fit a phone.
            double titleSize = title.Length <= 6 ? 64 : title.Length <= 12 ? 52 : 42;
            var header = new VerticalStackLayout
            {
                Children =
                {
                    topRow,
                    new Label
                    {
                        Text = title.ToLowerInvariant(),
                        FontFamily = TitleFont,
                        FontSize = titleSize,
                        CharacterSpacing = -titleSize * 0.05,
                        LineHeight = 0.95,
                        TextColor = PrimaryText,
                        Margin = new Thickness(-2, 18, 0, 6)
                    }
                }
            };

            var metaRow = new HorizontalStackLayout { Spacing = 12 };
            if (!string.IsNullOrEmpty(glyph))
                metaRow.Children.Add(MakeIcon(glyph, PrimaryText, 24));
            if (!string.IsNullOrEmpty(tag))
                metaRow.Children.Add(TagPill(tag));
            if (!string.IsNullOrEmpty(category))
                metaRow.Children.Add(new Label { Text = category, FontFamily = BodyFont, FontSize = 15, TextColor = SecondaryText, VerticalOptions = LayoutOptions.Center });
            if (metaRow.Children.Count > 0)
                header.Children.Add(metaRow);

            if (!string.IsNullOrEmpty(subtitle))
                header.Children.Add(new Label { Text = subtitle, FontFamily = TitleFont, FontSize = 17, TextColor = PrimaryText, Margin = new Thickness(0, 12, 0, 0) });
            // The module line only makes sense on demo pages (they have a tag or a group).
            if (!string.IsNullOrEmpty(tag) || !string.IsNullOrEmpty(category))
                header.Children.Add(new Label
            {
                Text = !string.IsNullOrEmpty(tag) ? tag.ToLowerInvariant() + " module" : "opencv",
                FontFamily = BodyFont,
                FontSize = 13,
                TextColor = MutedText
            });
            return header;
        }

        /// <summary>Full-width black pill button with spaced uppercase text.</summary>
        internal static Button PrimaryButton(string text, string glyph) => new Button
        {
            Text = text,
            TextTransform = TextTransform.Uppercase,
            CharacterSpacing = 2,
            FontFamily = TitleFont,
            FontSize = 15,
            BackgroundColor = Accent,
            TextColor = OnAccent,
            CornerRadius = 28,
            HeightRequest = 56,
            ImageSource = glyph == null ? null : new FontImageSource { FontFamily = IconFont, Glyph = glyph, Color = OnAccent, Size = 20 },
            ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 10)
        };

        /// <summary>Full-width grey pill button for the secondary action.</summary>
        internal static Button SecondaryButton(string text, string glyph) => new Button
        {
            Text = text,
            TextTransform = TextTransform.Uppercase,
            CharacterSpacing = 2,
            FontFamily = TitleFont,
            FontSize = 15,
            BackgroundColor = PillBackground,
            TextColor = PrimaryText,
            BorderWidth = 0,
            CornerRadius = 28,
            HeightRequest = 56,
            ImageSource = glyph == null ? null : new FontImageSource { FontFamily = IconFont, Glyph = glyph, Color = PrimaryText, Size = 18 },
            ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 10)
        };

        /// <summary>Oversized pale title for an empty state, e.g. "no results yet".</summary>
        internal static Label EmptyStateTitle(string text) => new Label
        {
            Text = text,
            FontFamily = TitleFont,
            FontSize = 44,
            CharacterSpacing = -2.2,
            LineHeight = 0.95,
            TextColor = EmptyStateText
        };

        /// <summary>Flat light-grey backdrop for photos and previews (no border, square corners).</summary>
        internal static Border ImageFrame(View content) => new Border
        {
            BackgroundColor = ImageBackground,
            Stroke = Colors.Transparent,
            StrokeThickness = 0,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(2) },
            Content = content
        };
    }
}
