//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using Microsoft.Maui.Controls.Shapes;

namespace MauiDemoApp
{
    /// <summary>
    /// The home screen: a slim top bar (name, OpenCV version, search, settings), the
    /// big "emgu cv" hero with the platform tags, category chips with a grid / list
    /// switch, a featured YOLO card, and one preview card per demo showing a real
    /// sample result. Phones stack it in one column and get a bottom tab bar.
    /// </summary>
    public partial class MainPage
    {
        // Filter chips. Group 0 is "All"; the rest match Categorize().
        private static readonly string[] CategoryNames = { "All", "Detection", "Image & 3D", "Video", "LLM" };

        private const double WideBreakpoint = 700;

        private sealed class DemoCard
        {
            public string Name;        // "scene text"
            public string Tag;         // "DNN"
            public string Category;    // "Detection" (label on the card)
            public int Group;          // index into CategoryNames
            public string Preview;     // "previews/scene-text.jpg", or null for a drawn preview
            public Button Source;      // the original button; its Clicked opens the demo
        }

        private static int Categorize(string text)
        {
            string t = (text ?? "").ToLowerInvariant();
            if (t.Contains("qwen") || t.Contains("smolvlm") || t.Contains("chat with")) return 4;   // LLM
            if (t.Contains("video")) return 3;
            if (t.Contains("hello") || t.Contains("planar") || t.Contains("feature matching")
                || t.Contains("super resolution") || t.Contains("3d") || t.Contains("unicode"))
                return 2;
            return 1;
        }

        // Card details for a demo button such as "Yolo (DNN module)".
        private static DemoCard Describe(Button b)
        {
            string text = b.Text ?? "";
            string t = text.ToLowerInvariant();
            string name = text;
            int open = text.IndexOf('(');
            if (open > 0)
                name = text.Substring(0, open).Trim();
            name = name.ToLowerInvariant();

            (string key, string display, string tag) info =
                t.Contains("yolo") ? ("yolo", "yolo", "DNN")
                : t.Contains("mask rcnn") ? ("mask-r-cnn", "mask r-cnn", "DNN")
                : t.Contains("scene text") ? ("scene-text", "scene text", "DNN")
                : t.Contains("feature matching") ? ("feature-matching", "feature matching", "KAZE")
                : t.Contains("planar") ? ("planar", "planar subdivision", "OpenCV")
                : t.Contains("shape") ? ("shape-detection", "shape detection", "OpenCV")
                : t.Contains("super resolution") ? ("super-resolution", "super resolution", "DNN")
                : t.Contains("face") ? ("face-detection", "face detection", "DNN")
                : t.Contains("pedestrian") ? ("pedestrian-detection", "pedestrian detection", "HOG")
                : t.Contains("barcode") ? ("barcode-and-qrcode", "barcode & qr code", "OpenCV")
                : t.Contains("tesseract") ? ("tesseract-ocr", "tesseract ocr", "Tesseract")
                : t.Contains("paddle") ? ("paddleocr", "paddleocr", "DNN")
                : t.Contains("hello") ? ("hello-world", "hello world", "OpenCV")
                : t.Contains("unicode") ? ("unicode", "unicode rendering", "OpenCV")
                : t.Contains("qwen") ? (null, "qwen chat", "LLM")
                : t.Contains("smolvlm") || t.Contains("chat with") ? (null, "chat with image", "VLM")
                : t.Contains("video") ? (null, "video surveillance", "OpenCV")
                : (null, name, "OpenCV");

            int group = Categorize(text);
            return new DemoCard
            {
                Name = info.display,
                Tag = info.tag,
                Category = group == 4 ? "Chat" : CategoryNames[group],
                Group = group,
                Preview = info.key == null ? null : "previews/" + info.key + ".jpg",
                Source = b,
            };
        }

        private void BuildHome(List<View> buttonList)
        {
            var cards = buttonList.OfType<Button>().Where(b => b.IsVisible).Select(Describe).ToList();
            var yolo = cards.FirstOrDefault(c => c.Name == "yolo");

            int currentGroup = 0;
            bool listMode = false;
            bool wide = true;
            double lastWidth = -1;

            Color text = DemoTheme.PrimaryText, muted = DemoTheme.SecondaryText, faint = DemoTheme.MutedText;

            // ---------- Search field (shared by both layouts) ----------
            var searchEntry = new Entry
            {
                Placeholder = $"Search {cards.Count} demos",
                FontFamily = DemoTheme.BodyFont,
                FontSize = 14,
                TextColor = text,
                PlaceholderColor = faint,
                BackgroundColor = Colors.Transparent,
                VerticalOptions = LayoutOptions.Center,
                ClassId = DemoTheme.BorderlessEntry
            };
            Func<bool, Border> searchField = (showShortcut) =>
            {
                var row = new Grid
                {
                    ColumnSpacing = 8,
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
                };
                row.Add(DemoTheme.MakeIcon(DemoTheme.GlyphSearch, faint, 18), 0, 0);
                if (showShortcut)
                    row.Add(new Border
                    {
                        BackgroundColor = DemoTheme.PageBackground,
                        Stroke = Colors.Transparent,
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(6) },
                        Padding = new Thickness(7, 2),
                        VerticalOptions = LayoutOptions.Center,
                        Content = new Label { Text = "⌘K", FontFamily = DemoTheme.TitleFont, FontSize = 11, TextColor = faint }
                    }, 2, 0);
                return new Border
                {
                    BackgroundColor = DemoTheme.PillBackground,
                    Stroke = Colors.Transparent,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) },
                    Padding = new Thickness(16, 2, 10, 2),
                    HeightRequest = 44,
                    Content = row
                };
            };
            var wideSearch = searchField(DeviceInfo.Idiom == DeviceIdiom.Desktop);
            var phoneSearch = searchField(false);
            // The Entry lives in whichever field is on screen.
            Action placeSearch = () =>
            {
                var target = (Grid)(wide ? wideSearch : phoneSearch).Content;
                if (searchEntry.Parent is Grid g && g != target)
                    g.Children.Remove(searchEntry);
                if (searchEntry.Parent == null)
                    target.Add(searchEntry, 1, 0);
            };

            // ---------- Top bar ----------
            var gear = DemoControls.CircleButton(SettingsPage.GlyphSettings, () => Navigation.PushAsync(new SettingsPage()), 40);
            var logo = new Border
            {
                WidthRequest = 26,
                HeightRequest = 26,
                BackgroundColor = Color.FromArgb("#0E0E0E"),
                Stroke = DemoTheme.IsDark ? DemoTheme.Hairline : Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                VerticalOptions = LayoutOptions.Center,
                Content = new BoxView { WidthRequest = 9, HeightRequest = 9, CornerRadius = 4.5, Color = DemoTheme.Lime, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }
            };
            var wideTop = new Grid { ColumnSpacing = 12, ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(new GridLength(380)), new ColumnDefinition(GridLength.Auto) } };
            wideTop.Add(new HorizontalStackLayout
            {
                Spacing = 10,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    logo,
                    new Label { Text = "emgu cv", FontFamily = DemoTheme.TitleFont, FontSize = 17, TextColor = text, VerticalOptions = LayoutOptions.Center },
                    DemoControls.Tag("OpenCV 5.1")
                }
            }, 0, 0);
            wideTop.Add(wideSearch, 1, 0);
            wideTop.Add(gear, 2, 0);

            Func<View> onDeviceTag = () =>
            {
                var tag = DemoControls.Tag("On device");
                var label = (Label)tag.Content;
                tag.Content = null;
                tag.Content = new HorizontalStackLayout
                {
                    Spacing = 6,
                    Children = { new BoxView { WidthRequest = 7, HeightRequest = 7, CornerRadius = 3.5, Color = DemoTheme.StatusGreen, VerticalOptions = LayoutOptions.Center }, label }
                };
                return tag;
            };
            var phoneGear = DemoControls.CircleButton(SettingsPage.GlyphSettings, () => Navigation.PushAsync(new SettingsPage()), 38);
            var phoneTop = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            phoneTop.Add(new HorizontalStackLayout { Spacing = 8, VerticalOptions = LayoutOptions.Center, Children = { DemoControls.Tag("OpenCV 5.1"), onDeviceTag() } }, 0, 0);
            phoneTop.Add(phoneGear, 1, 0);

            // ---------- Hero ----------
            Func<double, Label> wordmark = (size) => new Label
            {
                Text = "emgu cv",
                FontFamily = DemoTheme.HeroFont,
                FontSize = size,
                CharacterSpacing = -size * 0.05,
                LineHeight = 0.9,
                TextColor = text,
                LineBreakMode = LineBreakMode.NoWrap,
                Padding = new Thickness(0, 0, size * 0.1, 0),
                Margin = new Thickness(-size * 0.03, 0, 0, 0),
                VerticalOptions = LayoutOptions.Center
            };
            var wideWordmark = wordmark(160);
            var phoneWordmark = wordmark(84);
            var platformTags = new HorizontalStackLayout { Spacing = 8 };
            foreach (var v in new[] { onDeviceTag() }.Concat(new[] { "Windows", "macOS", "Linux", "iOS", "Android" }.Select(p => (View)DemoControls.Tag(p))))
                platformTags.Children.Add(v);
            Func<double, string, Label> pitch = (size, rest) => new Label
            {
                FontSize = size,
                LineHeight = 1.3,
                FormattedText = new FormattedString
                {
                    Spans =
                    {
                        new Span { Text = "OpenCV for .NET. ", FontFamily = DemoTheme.TitleFont, TextColor = text },
                        new Span { Text = rest, FontFamily = DemoTheme.BodyFont, TextColor = muted }
                    }
                }
            };
            var wideHero = new Grid { ColumnSpacing = 40, ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) } };
            wideHero.Add(wideWordmark, 0, 0);
            wideHero.Add(new VerticalStackLayout
            {
                Spacing = 18,
                VerticalOptions = LayoutOptions.Center,
                MaximumWidthRequest = 480,
                HorizontalOptions = LayoutOptions.End,
                Children = { pitch(17, "Real-time computer vision and on-device AI: every demo below runs locally, nothing leaves this machine."), platformTags }
            }, 1, 0);
            var phoneHero = new VerticalStackLayout
            {
                Spacing = 10,
                Children = { phoneWordmark, pitch(15, $"{cards.Count} vision & AI demos that run entirely on your phone.") }
            };

            // ---------- Filter chips + grid / list switch ----------
            var tilesHost = new VerticalStackLayout();
            var sectionTitle = new Label { Text = "All demos", FontFamily = DemoTheme.HeroFont, FontSize = 26, CharacterSpacing = -0.6, TextColor = text, VerticalOptions = LayoutOptions.Center };
            var sectionNote = new Label { Text = "Sorted by category", FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = faint, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.End };
            var chips = new List<(Border chip, Label label, int group)>();
            var chipRow = new HorizontalStackLayout { Spacing = 8 };
            Action layoutCards = null;
            Action refreshChips = () =>
            {
                bool searching = (searchEntry.Text ?? "").Trim().Length > 0;
                foreach (var (chip, label, group) in chips)
                {
                    bool on = !searching && group == currentGroup;
                    chip.BackgroundColor = on ? DemoTheme.Accent : DemoTheme.PillBackground;
                    label.TextColor = on ? DemoTheme.OnAccent : text;
                }
            };
            for (int i = 0; i < CategoryNames.Length; i++)
            {
                int count = i == 0 ? cards.Count : cards.Count(c => c.Group == i);
                if (count == 0)
                    continue;      // hide a group this build has no demos for
                int idx = i;
                var label = new Label
                {
                    FontFamily = DemoTheme.TitleFont,
                    FontSize = 12,
                    CharacterSpacing = 1.3,
                    VerticalOptions = LayoutOptions.Center,
                    FormattedText = new FormattedString
                    {
                        Spans =
                        {
                            new Span { Text = CategoryNames[i].ToUpperInvariant() },
                            new Span { Text = "  " + count, FontSize = 10, FontFamily = DemoTheme.BodyFont }
                        }
                    }
                };
                var chip = new Border
                {
                    Stroke = Colors.Transparent,
                    StrokeThickness = 0,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) },
                    Padding = new Thickness(16, 10),
                    Content = label
                };
                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) =>
                {
                    currentGroup = idx;
                    searchEntry.Text = "";
                    refreshChips();
                    layoutCards();
                };
                chip.GestureRecognizers.Add(tap);
                DemoControls.AddHover(chip);
                chips.Add((chip, label, i));
                chipRow.Children.Add(chip);
            }
            var viewSwitch = new SegmentedControl(new[] { "Grid", "List" }, 0, 12);
            viewSwitch.SelectionChanged += (s, e) => { listMode = viewSwitch.SelectedIndex == 1; layoutCards(); };
            var chipScroll = new ScrollView { Orientation = ScrollOrientation.Horizontal, HorizontalScrollBarVisibility = ScrollBarVisibility.Never, Content = chipRow };
            var filterRow = new Grid { ColumnSpacing = 12, ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            filterRow.Add(chipScroll, 0, 0);
            filterRow.Add(viewSwitch, 1, 0);

            // ---------- Featured card (YOLO) ----------
            View wideFeatured = yolo == null ? null : FeaturedCard(yolo, wide: true);
            View phoneFeatured = yolo == null ? null : FeaturedCard(yolo, wide: false);

            // ---------- Demo cards ----------
            var grid = new Grid { ColumnSpacing = 24, RowSpacing = 30 };
            var cardViews = cards.ToDictionary(c => c, c => (grid: GridCard(c), list: ListRow(c)));
            var emptyLabel = new Label { Text = "no matching demos", FontFamily = DemoTheme.HeroFont, FontSize = 28, TextColor = DemoTheme.EmptyStateText, IsVisible = false };

            layoutCards = () =>
            {
                double width = Width > 0 ? Width : 400;
                double pad = wide ? 40 : 16;
                string q = (searchEntry.Text ?? "").Trim().ToLowerInvariant();
                var shown = cards.Where(c => q.Length > 0
                    ? c.Name.Contains(q) || c.Category.ToLowerInvariant().Contains(q) || c.Tag.ToLowerInvariant().Contains(q)
                    : currentGroup == 0 || c.Group == currentGroup).ToList();

                sectionTitle.Text = q.Length > 0 ? $"results for “{q}”" : currentGroup == 0 ? "All demos" : CategoryNames[currentGroup];
                sectionNote.Text = q.Length > 0 || currentGroup != 0 ? $"{shown.Count} demo{(shown.Count == 1 ? "" : "s")}" : "Sorted by category";
                emptyLabel.IsVisible = shown.Count == 0;

                grid.Children.Clear();
                grid.ColumnDefinitions.Clear();
                grid.RowDefinitions.Clear();
                if (listMode)
                {
                    grid.RowSpacing = 0;
                    grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                    for (int i = 0; i < shown.Count; i++)
                    {
                        grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                        grid.Add(cardViews[shown[i]].list, 0, i);
                    }
                    return;
                }
                int columns = !wide ? 2 : width >= 1150 ? 4 : width >= 860 ? 3 : 2;
                double cardWidth = (width - 2 * pad - (columns - 1) * grid.ColumnSpacing) / columns;
                grid.RowSpacing = wide ? 34 : 24;
                for (int c = 0; c < columns; c++)
                    grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                for (int r = 0; r < (shown.Count + columns - 1) / columns; r++)
                    grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                for (int i = 0; i < shown.Count; i++)
                {
                    var view = cardViews[shown[i]].grid;
                    ((View)((VerticalStackLayout)view).Children[0]).HeightRequest = cardWidth * 0.75;
                    grid.Add(view, i % columns, i / columns);
                }
            };

            searchEntry.TextChanged += (s, e) => { refreshChips(); layoutCards(); };

            // ---------- Phone tab bar ----------
            var tabBar = new Grid
            {
                BackgroundColor = DemoTheme.PageBackground,
                Padding = new Thickness(8, 8, 8, 4),
                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) }
            };
            Func<string, string, bool, Action, View> tab = (glyph, label, active, onTap) =>
            {
                var c = active ? text : faint;
                var v = new VerticalStackLayout
                {
                    Spacing = 3,
                    HorizontalOptions = LayoutOptions.Center,
                    Children =
                    {
                        DemoTheme.MakeIcon(glyph, c, 22),
                        new Label { Text = label, FontFamily = active ? DemoTheme.TitleFont : DemoTheme.BodyFont, FontSize = 11, TextColor = c, HorizontalOptions = LayoutOptions.Center }
                    }
                };
                if (onTap != null)
                {
                    var t = new TapGestureRecognizer();
                    t.Tapped += (s, e) => onTap();
                    v.GestureRecognizers.Add(t);
                }
                return v;
            };
            tabBar.Add(tab(DemoTheme.GlyphGrid, "Demos", true, null), 0, 0);
            tabBar.Add(tab(DemoTheme.GlyphVideo, "Camera", false, yolo == null ? null : () => Navigation.PushAsync(new YoloPage { StartWithCamera = true })), 1, 0);
            tabBar.Add(tab(DemoTheme.GlyphDownload, "Models", false, () => Navigation.PushAsync(new SettingsPage())), 2, 0);
            tabBar.Add(tab(SettingsPage.GlyphSettings, "Settings", false, () => Navigation.PushAsync(new SettingsPage())), 3, 0);
            var tabHost = new VerticalStackLayout { Children = { DemoControls.Hairline(), tabBar } };

            // ---------- Arrange for the current width ----------
            var root = new Grid { RowDefinitions = { new RowDefinition(GridLength.Star), new RowDefinition(GridLength.Auto) } };
            var scroll = new ScrollView();
            root.Add(scroll, 0, 0);
            root.Add(tabHost, 0, 1);

            Action arrange = () =>
            {
                double width = Width > 0 ? Width : 400;
                wide = width >= WideBreakpoint;
                foreach (var v in new View[] { wideTop, phoneTop, wideHero, phoneHero, filterRow, wideFeatured, phoneFeatured, grid, emptyLabel })
                    if (v?.Parent is Layout l)
                        l.Children.Remove(v);
                placeSearch();

                double pad = wide ? 40 : 16;
                var header = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
                header.Add(sectionTitle, 0, 0);
                header.Add(sectionNote, 1, 0);

                var page = new VerticalStackLayout { Padding = new Thickness(pad, wide ? 26 : 12, pad, 36), Spacing = wide ? 26 : 16 };
                if (wide)
                {
                    // Size the wordmark to the space left of the pitch column.
                    double size = Math.Clamp((width - 2 * pad - 440) / 4.1, 80, 170);
                    wideWordmark.FontSize = size;
                    wideWordmark.CharacterSpacing = -size * 0.05;
                    wideWordmark.Padding = new Thickness(0, 0, size * 0.1, 0);
                    filterRow.ColumnDefinitions[1].Width = GridLength.Auto;
                    viewSwitch.IsVisible = true;
                    page.Children.Add(wideTop);
                    page.Children.Add(new ContentView { Margin = new Thickness(0, 14, 0, 0), Content = wideHero });
                    page.Children.Add(filterRow);
                    if (wideFeatured != null)
                        page.Children.Add(wideFeatured);
                }
                else
                {
                    double size = Math.Clamp((width - 2 * pad) / 4.1, 56, 110);
                    phoneWordmark.FontSize = size;
                    phoneWordmark.CharacterSpacing = -size * 0.05;
                    phoneWordmark.Padding = new Thickness(0, 0, size * 0.1, 0);
                    viewSwitch.IsVisible = false;
                    page.Children.Add(phoneTop);
                    page.Children.Add(phoneHero);
                    page.Children.Add(phoneSearch);
                    page.Children.Add(filterRow);
                    if (phoneFeatured != null)
                        page.Children.Add(phoneFeatured);
                }
                page.Children.Add(new ContentView { Margin = new Thickness(0, wide ? 14 : 6, 0, 0), Content = header });
                page.Children.Add(grid);
                page.Children.Add(emptyLabel);
                scroll.Content = page;
                tabHost.IsVisible = !wide;
                layoutCards();
            };

            // Cmd+K focuses search on the Mac.
            var searchItem = new MenuFlyoutItem { Text = "Search Demos" };
            searchItem.KeyboardAccelerators.Add(new KeyboardAccelerator { Modifiers = KeyboardAcceleratorModifiers.Cmd, Key = "K" });
            searchItem.Clicked += (s, e) => searchEntry.Focus();
            var menu = new MenuBarItem { Text = "Demo" };
            menu.Add(searchItem);
            MenuBarItems.Add(menu);

            SizeChanged += (s, e) =>
            {
                if (Math.Abs(Width - lastWidth) < 1)
                    return;
                bool wasWide = wide;
                lastWidth = Width;
                bool nowWide = Width >= WideBreakpoint;
                if (scroll.Content == null || nowWide != wasWide || nowWide)
                    arrange();
                else
                    layoutCards();
            };

            //TEMP-AUTOOPEN-BEGIN
            if (Environment.GetEnvironmentVariable("EMGU_DEMO_SCROLL") is string sc)
                Dispatcher.DispatchDelayed(TimeSpan.FromSeconds(3), async () => { if (sc == "list") viewSwitch.SelectedIndex = 1; if (sc == "list") { listMode = true; layoutCards(); } await scroll.ScrollToAsync(grid, ScrollToPosition.Start, false); });
            //TEMP-AUTOOPEN-END
            refreshChips();
            BackgroundColor = DemoTheme.PageBackground;
            Content = root;
            arrange();
        }

        // ---------- Cards ----------

        private View GridCard(DemoCard c)
        {
            var preview = Preview(c, 18);
            // Long names wrap to a second line rather than being cut off on phones.
            var title = new Label { Text = c.Name, FontFamily = DemoTheme.TitleFont, FontSize = 17, LineHeight = 1.05, TextColor = DemoTheme.PrimaryText, LineBreakMode = LineBreakMode.WordWrap, MaxLines = 2, VerticalOptions = LayoutOptions.Center };
            var titleRow = new Grid { ColumnSpacing = 8, Margin = new Thickness(0, 12, 0, 0), ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            titleRow.Add(title, 0, 0);
            titleRow.Add(ArrowCircle(), 1, 0);
            var meta = new HorizontalStackLayout
            {
                Spacing = 8,
                Margin = new Thickness(0, 6, 0, 0),
                Children = { DemoControls.Tag(c.Tag), new Label { Text = c.Category, FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center } }
            };
            var card = new VerticalStackLayout { Children = { preview, titleRow, meta } };
            Tappable(card, c);
            return card;
        }

        private View ListRow(DemoCard c)
        {
            var thumb = Preview(c, 10, small: true);
            thumb.WidthRequest = 72;
            thumb.HeightRequest = 54;
            var row = new Grid
            {
                ColumnSpacing = 16,
                Padding = new Thickness(0, 12),
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
            };
            row.Add(thumb, 0, 0);
            row.Add(new VerticalStackLayout
            {
                Spacing = 4,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label { Text = c.Name, FontFamily = DemoTheme.TitleFont, FontSize = 17, TextColor = DemoTheme.PrimaryText },
                    new HorizontalStackLayout { Spacing = 8, Children = { DemoControls.Tag(c.Tag), new Label { Text = c.Category, FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center } } }
                }
            }, 1, 0);
            row.Add(ArrowCircle(), 2, 0);
            var stack = new VerticalStackLayout { Children = { row, DemoControls.Hairline() } };
            Tappable(stack, c);
            return stack;
        }

        private static void Tappable(View view, DemoCard c)
        {
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => ((Microsoft.Maui.IButton)c.Source).Clicked();
            view.GestureRecognizers.Add(tap);
            DemoControls.AddHover(view);
        }

        private static View ArrowCircle() => new Border
        {
            WidthRequest = 30,
            HeightRequest = 30,
            BackgroundColor = Colors.Transparent,
            Stroke = DemoTheme.Hairline,
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
            VerticalOptions = LayoutOptions.Center,
            Content = DemoTheme.MakeIcon(DemoTheme.GlyphArrowForward, DemoTheme.PrimaryText, 15)
        };

        // A real sample result, or a drawn preview for demos without one (chat, video).
        private static Border Preview(DemoCard c, double radius, bool small = false)
        {
            View content;
            Color bg = DemoTheme.ImageBackground;
            if (small && c.Preview == null)
            {
                // Too small for the drawn previews: an icon on dark.
                bg = Color.FromArgb("#111111");
                content = DemoTheme.MakeIcon(c.Name == "video surveillance" ? DemoTheme.GlyphVideo : "\ue262", Color.FromArgb("#F4F4F2"), 22);
            }
            else if (c.Preview != null)
            {
                content = new Image { Aspect = Aspect.AspectFill, Source = ImageSource.FromStream(ct => FileSystem.OpenAppPackageFileAsync(c.Preview)) };
            }
            else if (c.Name == "video surveillance")
            {
                bg = Color.FromArgb("#111111");
                var (badge, _, dot) = DemoControls.Badge("LIVE");
                dot.Color = DemoTheme.Lime;
                badge.Margin = new Thickness(10);
                content = new Grid
                {
                    Children =
                    {
                        new Image { Aspect = Aspect.AspectFill, Opacity = 0.55, Source = ImageSource.FromStream(ct => FileSystem.OpenAppPackageFileAsync("previews/pedestrian-detection.jpg")) },
                        badge
                    }
                };
            }
            else
            {
                // Chat: a tiny conversation on dark.
                bg = Color.FromArgb("#111111");
                bool vlm = c.Tag == "VLM";
                Func<string, bool, View> bubble = (t, user) => new Border
                {
                    BackgroundColor = user ? Color.FromArgb("#2A2A2A") : Color.FromArgb("#F4F4F2"),
                    Stroke = Colors.Transparent,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                    Padding = new Thickness(11, 7),
                    HorizontalOptions = user ? LayoutOptions.End : LayoutOptions.Start,
                    MaximumWidthRequest = 210,
                    Content = new Label { Text = t, FontFamily = DemoTheme.BodyFont, FontSize = 11, TextColor = user ? Colors.White : Color.FromArgb("#0E0E0E") }
                };
                var stack = new VerticalStackLayout { Spacing = 8, Padding = new Thickness(14), VerticalOptions = LayoutOptions.Center };
                if (vlm)
                {
                    stack.Children.Add(new Border
                    {
                        WidthRequest = 70,
                        HeightRequest = 52,
                        HorizontalOptions = LayoutOptions.End,
                        Stroke = Colors.Transparent,
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                        Content = new Image { Aspect = Aspect.AspectFill, Source = ImageSource.FromStream(ct => FileSystem.OpenAppPackageFileAsync("dog416.png")) }
                    });
                    stack.Children.Add(bubble("What's in this photo?", true));
                    stack.Children.Add(bubble("A dog sitting on a porch next to a red bicycle.", false));
                }
                else
                {
                    stack.Children.Add(bubble("What can you run offline?", true));
                    stack.Children.Add(bubble("Everything here: I'm a 0.6B model running on your device.", false));
                    stack.Children.Add(bubble("• • •", false));
                }
                content = stack;
            }
            return new Border
            {
                BackgroundColor = bg,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(radius) },
                HeightRequest = 150,
                Content = content
            };
        }

        // ---------- Featured card ----------

        // Real numbers only: the last YOLO timing measured on this device and the
        // downloaded model's size; dashes until there is one.
        private static (string ms, string mb) YoloStats()
        {
            double ms = Preferences.Default.Get(YoloPage.LastInferenceKey, 0.0);
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "emgu", "yolo", "yolo12n.onnx");
            var fi = new System.IO.FileInfo(path);
            return (ms > 0 ? $"{ms:0} ms" : "—", fi.Exists ? $"{fi.Length / (1024.0 * 1024.0):0.0} MB" : "—");
        }

        private View FeaturedCard(DemoCard yolo, bool wide)
        {
            var dark = Color.FromArgb("#0E0E0E");
            var white = Color.FromArgb("#F4F4F2");
            var grey = Color.FromArgb("#A3A3A3");
            var (ms, mb) = YoloStats();
            var photo = new Image { Aspect = Aspect.AspectFill, Source = ImageSource.FromStream(ct => FileSystem.OpenAppPackageFileAsync("previews/yolo.jpg")) };
            var (badge, _, _) = DemoControls.Badge("Sample result");
            badge.Margin = new Thickness(14);
            var kicker = new Label
            {
                FontSize = 11,
                CharacterSpacing = 1.8,
                FormattedText = new FormattedString
                {
                    Spans =
                    {
                        new Span { Text = "FEATURED", FontFamily = DemoTheme.TitleFont, TextColor = DemoTheme.Lime },
                        new Span { Text = "  ·  DETECTION", FontFamily = DemoTheme.BodyFont, TextColor = grey }
                    }
                }
            };

            if (!wide)
            {
                var arrow = new Border
                {
                    WidthRequest = 42,
                    HeightRequest = 42,
                    BackgroundColor = white,
                    Stroke = Colors.Transparent,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(21) },
                    VerticalOptions = LayoutOptions.Center,
                    Content = DemoTheme.MakeIcon(DemoTheme.GlyphArrowForward, dark, 20)
                };
                var info = new Grid { Padding = new Thickness(16, 14), ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
                info.Add(new VerticalStackLayout
                {
                    Spacing = 2,
                    Children =
                    {
                        kicker,
                        new Label { Text = "yolo", FontFamily = DemoTheme.HeroFont, FontSize = 44, CharacterSpacing = -2, LineHeight = 0.95, TextColor = white, Padding = new Thickness(0, 0, 6, 0) },
                        new Label { Text = $"{ms} · {mb} · on device", FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = grey }
                    }
                }, 0, 0);
                info.Add(arrow, 1, 0);
                var phoneCard = new Border
                {
                    BackgroundColor = dark,
                    Stroke = Colors.Transparent,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                    Content = new VerticalStackLayout { Children = { new Grid { HeightRequest = 180, Children = { photo, badge } }, info } }
                };
                Tappable(phoneCard, yolo);
                return phoneCard;
            }

            var open = new Button
            {
                Text = "Open demo  →",
                TextTransform = TextTransform.Uppercase,
                CharacterSpacing = 1.6,
                FontFamily = DemoTheme.TitleFont,
                FontSize = 13,
                BackgroundColor = white,
                TextColor = dark,
                CornerRadius = 24,
                HeightRequest = 48,
                Padding = new Thickness(24, 0)
            };
            open.Clicked += (s, e) => ((Microsoft.Maui.IButton)yolo.Source).Clicked();
            var live = new Button
            {
                Text = "Live camera",
                TextTransform = TextTransform.Uppercase,
                CharacterSpacing = 1.6,
                FontFamily = DemoTheme.TitleFont,
                FontSize = 13,
                BackgroundColor = Color.FromArgb("#262626"),
                TextColor = white,
                CornerRadius = 24,
                HeightRequest = 48,
                Padding = new Thickness(24, 0)
            };
            live.Clicked += (s, e) => Navigation.PushAsync(new YoloPage { StartWithCamera = true });
            Func<string, string, View> stat = (v, caption) => new VerticalStackLayout
            {
                Spacing = 2,
                Children =
                {
                    new Label { Text = v, FontFamily = DemoTheme.TitleFont, FontSize = 22, TextColor = white },
                    new Label { Text = caption, FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = grey }
                }
            };
            var side = new VerticalStackLayout
            {
                Spacing = 14,
                Padding = new Thickness(34, 30),
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    kicker,
                    new Label { Text = "yolo", FontFamily = DemoTheme.HeroFont, FontSize = 96, CharacterSpacing = -4.5, LineHeight = 0.9, TextColor = white, Padding = new Thickness(0, 0, 12, 0) },
                    new Label { Text = "Detect and label 80 kinds of objects in a photo or live camera feed with YOLO12.", FontFamily = DemoTheme.BodyFont, FontSize = 17, LineHeight = 1.3, TextColor = Color.FromArgb("#C9C9C6") },
                    new HorizontalStackLayout { Spacing = 10, Margin = new Thickness(0, 6, 0, 0), Children = { open, live } },
                    new BoxView { HeightRequest = 1, Color = Color.FromArgb("#2A2A2A"), Margin = new Thickness(0, 8, 0, 0) },
                    new HorizontalStackLayout { Spacing = 34, Children = { stat(ms, "per frame, last run"), stat(mb, "model"), stat("CPU", "backend") } }
                }
            };
            var body = new Grid { ColumnDefinitions = { new ColumnDefinition(new GridLength(1.1, GridUnitType.Star)), new ColumnDefinition(new GridLength(1, GridUnitType.Star)) } };
            body.Add(new Grid { Children = { photo, badge } }, 0, 0);
            body.Add(side, 1, 0);
            return new Border
            {
                BackgroundColor = dark,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(24) },
                HeightRequest = 440,
                Content = body
            };
        }
    }
}
