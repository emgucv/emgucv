//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using Microsoft.Maui.Controls.Shapes;

namespace MauiDemoApp
{
    /// <summary>
    /// Pill-shaped filter chip with uppercase, letter-spaced text. The selected chip
    /// swaps to inverted colors (text color fill, page color text).
    /// </summary>
    internal class ChipView : Border
    {
        private readonly Label _label;
        private bool _selected;

        public event EventHandler Tapped;

        public ChipView(string text, bool selected = false, double fontSize = 12)
        {
            _label = new Label
            {
                Text = text.ToUpperInvariant(),
                FontFamily = DemoTheme.TitleFont,
                FontSize = fontSize,
                CharacterSpacing = 1.4,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            Stroke = Colors.Transparent;
            StrokeThickness = 0;
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) };
            Padding = new Thickness(16, 9);
            VerticalOptions = LayoutOptions.Center;
            Content = _label;
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => Tapped?.Invoke(this, EventArgs.Empty);
            GestureRecognizers.Add(tap);
            IsSelected = selected;
            DemoControls.AddHover(this);
        }

        public bool IsSelected
        {
            get => _selected;
            set
            {
                _selected = value;
                BackgroundColor = value ? DemoTheme.Accent : DemoTheme.PillBackground;
                _label.TextColor = value ? DemoTheme.OnAccent : DemoTheme.PrimaryText;
            }
        }
    }

    /// <summary>
    /// Segmented control: a pill-shaped track holding a row of options; the selected
    /// segment is "raised" (page-colored pill on the chip-colored track).
    /// </summary>
    internal class SegmentedControl : Border
    {
        private readonly HorizontalStackLayout _row = new HorizontalStackLayout { Spacing = 2 };
        private readonly List<(Border seg, Label label)> _segments = new List<(Border, Label)>();
        private int _selectedIndex = -1;
        private readonly double _fontSize;

        /// <summary>Raised whenever the user picks a different segment.</summary>
        public event EventHandler SelectionChanged;

        public SegmentedControl(IEnumerable<string> items, int selectedIndex = 0, double fontSize = 13)
        {
            _fontSize = fontSize;
            BackgroundColor = DemoTheme.PillBackground;
            Stroke = Colors.Transparent;
            StrokeThickness = 0;
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) };
            Padding = new Thickness(3);
            HorizontalOptions = LayoutOptions.Start;
            VerticalOptions = LayoutOptions.Center;
            Content = _row;
            SetItems(items, selectedIndex);
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value == _selectedIndex)
                    return;
                _selectedIndex = value;
                Refresh();
            }
        }

        public string SelectedItem => _selectedIndex >= 0 && _selectedIndex < _segments.Count ? _segments[_selectedIndex].label.Text : null;

        /// <summary>Replace the options (e.g. the sizes available for a model version).</summary>
        public void SetItems(IEnumerable<string> items, int selectedIndex = 0)
        {
            _row.Children.Clear();
            _segments.Clear();
            int i = 0;
            foreach (string item in items)
            {
                int idx = i++;
                var label = new Label
                {
                    Text = item,
                    FontFamily = DemoTheme.TitleFont,
                    FontSize = _fontSize,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                var seg = new Border
                {
                    Stroke = Colors.Transparent,
                    StrokeThickness = 0,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) },
                    Padding = new Thickness(14, 7),
                    MinimumWidthRequest = 36,
                    Content = label
                };
                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) =>
                {
                    if (idx == _selectedIndex)
                        return;
                    SelectedIndex = idx;
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                };
                seg.GestureRecognizers.Add(tap);
                _segments.Add((seg, label));
                _row.Children.Add(seg);
            }
            _selectedIndex = Math.Clamp(selectedIndex, 0, Math.Max(0, _segments.Count - 1));
            Refresh();
        }

        private void Refresh()
        {
            for (int i = 0; i < _segments.Count; i++)
            {
                bool on = i == _selectedIndex;
                _segments[i].seg.BackgroundColor = on ? DemoTheme.PageBackground : Colors.Transparent;
                _segments[i].seg.Shadow = on ? new Shadow { Brush = Colors.Black, Opacity = DemoTheme.IsDark ? 0.5f : 0.10f, Radius = 4, Offset = new Point(0, 1) } : null;
                _segments[i].label.TextColor = on ? DemoTheme.PrimaryText : DemoTheme.SecondaryText;
            }
        }
    }

    /// <summary>Small reusable building blocks for the redesigned pages.</summary>
    internal static class DemoControls
    {
        /// <summary>Small pill tag such as "DNN", "LLM", "OpenCV".</summary>
        internal static Border Tag(string text) => new Border
        {
            BackgroundColor = DemoTheme.PillBackground,
            Stroke = Colors.Transparent,
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) },
            Padding = new Thickness(9, 4),
            VerticalOptions = LayoutOptions.Center,
            Content = new Label { Text = text, FontFamily = DemoTheme.TitleFont, FontSize = 11, CharacterSpacing = 0.6, TextColor = DemoTheme.PrimaryText }
        };

        /// <summary>Section label: 11pt, uppercase, letter spaced, faint.</summary>
        internal static Label SectionLabel(string text) => new Label
        {
            Text = text.ToUpperInvariant(),
            FontFamily = DemoTheme.BodyFont,
            FontSize = 11,
            CharacterSpacing = 1.6,
            TextColor = DemoTheme.MutedText,
            VerticalOptions = LayoutOptions.Center
        };

        /// <summary>Round, chip-filled icon button (back, share, info, fullscreen...).</summary>
        internal static Border CircleButton(string glyph, Action onTap, double size = 38, Color fill = null, Color iconColor = null)
        {
            var b = new Border
            {
                WidthRequest = size,
                HeightRequest = size,
                BackgroundColor = fill ?? DemoTheme.PillBackground,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(size / 2) },
                VerticalOptions = LayoutOptions.Center,
                Content = DemoTheme.MakeIcon(glyph, iconColor ?? DemoTheme.PrimaryText, size * 0.5)
            };
            var t = new TapGestureRecognizer();
            t.Tapped += (s, e) => onTap();
            b.GestureRecognizers.Add(t);
            AddHover(b);
            return b;
        }

        /// <summary>
        /// Result badge for the corner of a photo: green dot + text such as
        /// "3 objects · 18 ms". Dark glass so it reads on any image.
        /// </summary>
        internal static (Border badge, Label label, BoxView dot) Badge(string text, Color dotColor = null)
        {
            var dot = new BoxView { WidthRequest = 7, HeightRequest = 7, CornerRadius = 3.5, Color = dotColor ?? DemoTheme.StatusGreen, VerticalOptions = LayoutOptions.Center };
            var label = new Label { Text = text, FontFamily = DemoTheme.TitleFont, FontSize = 12, TextColor = Colors.White, VerticalOptions = LayoutOptions.Center };
            var badge = new Border
            {
                BackgroundColor = DemoTheme.PhotoBadgeBackground,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) },
                Padding = new Thickness(11, 6),
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Content = new HorizontalStackLayout { Spacing = 7, Children = { dot, label } }
            };
            return (badge, label, dot);
        }

        /// <summary>Rounded "well" (image area / panel) with no border.</summary>
        internal static Border Well(View content, double radius = 22, Thickness? padding = null) => new Border
        {
            BackgroundColor = DemoTheme.ImageBackground,
            Stroke = Colors.Transparent,
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(radius) },
            Padding = padding ?? new Thickness(0),
            Content = content
        };

        /// <summary>Filled primary pill button (text color fill). Fully round.</summary>
        internal static Button PrimaryButton(string text, string glyph = null, double height = 50)
        {
            var b = new Button
            {
                Text = text,
                TextTransform = TextTransform.Uppercase,
                CharacterSpacing = 1.6,
                FontFamily = DemoTheme.TitleFont,
                FontSize = 14,
                BackgroundColor = DemoTheme.Accent,
                TextColor = DemoTheme.OnAccent,
                CornerRadius = (int)(height / 2),
                HeightRequest = height,
                BorderWidth = 0,
                ImageSource = glyph == null ? null : new FontImageSource { FontFamily = DemoTheme.IconFont, Glyph = glyph, Color = DemoTheme.OnAccent, Size = 18 },
                ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 8)
            };
            return b;
        }

        /// <summary>Thin rule between panel sections.</summary>
        internal static BoxView Hairline() => new BoxView { HeightRequest = 1, Color = DemoTheme.Hairline };

        /// <summary>Desktop hover: dim slightly while the pointer is over the view.</summary>
        internal static void AddHover(View view)
        {
            var hover = new PointerGestureRecognizer();
            hover.PointerEntered += (s, e) => view.Opacity = 0.82;
            hover.PointerExited += (s, e) => view.Opacity = 1;
            view.GestureRecognizers.Add(hover);
        }
    }
}
