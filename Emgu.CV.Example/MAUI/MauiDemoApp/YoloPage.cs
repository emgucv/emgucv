//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Models;
using Emgu.CV.Structure;

using Microsoft.Maui.Controls.Shapes;

namespace MauiDemoApp
{
    /// <summary>
    /// YOLO object detection, in the redesigned two-pane layout: a large image stage
    /// (boxes and label chips drawn over the photo) and a settings panel (model,
    /// confidence, run, results, performance, the C# behind it). On a phone the panel
    /// stacks under the image and the Detect button is pinned to the bottom.
    /// </summary>
    public class YoloPage : ContentPage
    {
        private const string Title_ = "yolo";
        private const string Category = "Detection";
        private const string Description = "Detect and label objects in a photo or live camera feed.";
        private const double PanelWidth = 380;
        private const double WideBreakpoint = 900;

        private sealed record SampleImage(string File, string Name);

        private static readonly SampleImage[] Samples =
        {
            new SampleImage("dog416.png", "dog.png"),
            new SampleImage("pedestrian.png", "street.png"),
            new SampleImage("cars_license_plate.png", "cars.png"),
        };

        // Version label -> enum prefix and the sizes that version ships in.
        private static readonly (string label, string prefix, string[] sizes)[] Versions =
        {
            ("v12", "Yolo12", new[] { "n", "s", "m", "l", "x" }),
            ("v11", "Yolo11", new[] { "n", "s", "m", "l", "x" }),
            ("v10", "YoloV10", new[] { "n", "s", "m", "b", "l", "x" }),
            ("v8", "YoloV8", new[] { "n" }),
        };

        // ---------- State ----------
        private Yolo _model = new Yolo();
        private string _modelLoaded;          // selection the model was initialized with
        private Mat _image;                   // current input (BGR)
        private string _imageName;
        private int _sampleIndex;
        private DetectedObject[] _all = Array.Empty<DetectedObject>();  // every detection >= MinConfidence
        private bool _busy;
        private bool _wide;
        private bool _layoutBuilt;
        private bool _loaded;
        private double _lastInferenceMs, _lastPrePostMs;

        // Ask the network for everything above this; the slider then filters
        // instantly without running the model again.
        private const double MinConfidence = 0.05;

        // ---------- Views (built once, arranged by BuildLayout) ----------
        private readonly View _headerWide, _headerPhone;
        private readonly Grid _stage;
        private readonly Image _photo;
        private readonly GraphicsView _overlay;
        private readonly DetectionDrawable _drawable = new DetectionDrawable();
        private readonly Border _resultBadge;
        private readonly Label _resultBadgeLabel;
        private readonly Border _sampleBadge;
        private readonly Label _sampleBadgeLabel;
        private readonly ActivityIndicator _stageSpinner;
        private readonly View _toolbar;
        private readonly SegmentedControl _sourceToggle;
        private readonly Border _compareButton;
        private bool _comparing;
        private readonly HorizontalStackLayout _thumbs;
        private readonly List<Border> _thumbBorders = new List<Border>();

        private readonly SegmentedControl _versionControl;
        private readonly SegmentedControl _sizeControl;
        private readonly Label _modelNameLabel;
        private readonly Label _modelStatusLabel;
        private readonly BoxView _modelStatusDot;
        private readonly Border _downloadChip;
        private readonly Slider _confidence;
        private readonly Label _confidenceValue;
        private readonly Button _detectButton;
        private readonly Label _resultsCount;
        private readonly VerticalStackLayout _resultsList;
        private readonly VerticalStackLayout _emptyState;
        private readonly Label _emptyCaption;
        private readonly Label _perfInference, _perfPrePost, _perfFps, _perfBackend;
        private readonly View _codeLink;
        private readonly View _modelSection, _confidenceSection, _resultsSection, _perfSection;
        private readonly Border _fullscreenButton;

        private readonly Grid _sheetOverlay;
        private readonly Border _sheetCard;
        private readonly BoxView _sheetScrim;

        private readonly Grid _root = new Grid();

        public YoloPage()
        {
            Shell.SetNavBarIsVisible(this, false);
            BackgroundColor = DemoTheme.PageBackground;
            Title = "YOLO";

            // ---------- Header ----------
            _headerWide = BuildHeader(wide: true);
            _headerPhone = BuildHeader(wide: false);

            // ---------- Image stage ----------
            _photo = new Image { Aspect = Aspect.AspectFit, Margin = new Thickness(16, 16, 16, 70) };
            _overlay = new GraphicsView { Drawable = _drawable, InputTransparent = true, Margin = new Thickness(16, 16, 16, 70) };
            (_resultBadge, _resultBadgeLabel, _) = DemoControls.Badge("");
            _resultBadge.Margin = new Thickness(14);
            _resultBadge.IsVisible = false;
            (_sampleBadge, _sampleBadgeLabel, var sampleDot) = DemoControls.Badge("");
            sampleDot.IsVisible = false;
            _sampleBadge.HorizontalOptions = LayoutOptions.End;
            _sampleBadge.Margin = new Thickness(14);
            _stageSpinner = new ActivityIndicator { Color = DemoTheme.Lime, IsRunning = false, IsVisible = false, WidthRequest = 36, HeightRequest = 36, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            _stage = new Grid { Children = { _photo, _overlay, _resultBadge, _sampleBadge, _stageSpinner } };
            // (the floating toolbar is added to the stage below, once built)
            _overlay.SizeChanged += (s, e) => PlaceBadges();
            var drop = new DropGestureRecognizer { AllowDrop = true };
            drop.DragOver += (s, e) => e.AcceptedOperation = DataPackageOperation.Copy;
            drop.Drop += OnDrop;
            _stage.GestureRecognizers.Add(drop);

            // Floating toolbar: Photo | Camera, compare (before / after), fullscreen.
            _sourceToggle = new SegmentedControl(new[] { "Photo", "Camera" }, 0, 13);
            _sourceToggle.SelectionChanged += async (s, e) =>
            {
                if (_sourceToggle.SelectedIndex == 1)
                {
                    await OpenLiveCamera();
                    _sourceToggle.SelectedIndex = 0;
                }
            };
            _compareButton = DemoControls.CircleButton(DemoTheme.GlyphCompare, ToggleCompare, 34, Colors.Transparent);
            _fullscreenButton = DemoControls.CircleButton(DemoTheme.GlyphFullscreen, OpenFullscreen, 34, Colors.Transparent);
            _toolbar = new Border
            {
                BackgroundColor = DemoTheme.PageBackground,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) },
                Padding = new Thickness(4),
                HorizontalOptions = LayoutOptions.Center,
                Shadow = new Shadow { Brush = Colors.Black, Opacity = DemoTheme.IsDark ? 0.6f : 0.08f, Radius = 10, Offset = new Point(0, 2) },
                Content = new HorizontalStackLayout
                {
                    Spacing = 4,
                    Children = { _sourceToggle, new BoxView { WidthRequest = 1, HeightRequest = 20, Color = DemoTheme.Hairline, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(4, 0) }, _compareButton, _fullscreenButton }
                }
            };

            _toolbar.VerticalOptions = LayoutOptions.End;
            _toolbar.Margin = new Thickness(0, 0, 0, 14);
            _stage.Children.Add(_toolbar);

            // Sample thumbnails + "+" for your own photo.
            _thumbs = new HorizontalStackLayout { Spacing = 8 };
            for (int i = 0; i < Samples.Length; i++)
            {
                int idx = i;
                var thumb = new Border
                {
                    WidthRequest = 46,
                    HeightRequest = 46,
                    Stroke = Colors.Transparent,
                    StrokeThickness = 2,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                    BackgroundColor = DemoTheme.PillBackground,
                    Content = new Image { Aspect = Aspect.AspectFill, Source = ImageSource.FromStream(ct => FileSystem.OpenAppPackageFileAsync(Samples[idx].File)) }
                };
                var t = new TapGestureRecognizer();
                t.Tapped += async (s, e) => await LoadSample(idx);
                thumb.GestureRecognizers.Add(t);
                DemoControls.AddHover(thumb);
                _thumbBorders.Add(thumb);
                _thumbs.Children.Add(thumb);
            }
            var addThumb = new Border
            {
                WidthRequest = 46,
                HeightRequest = 46,
                Stroke = DemoTheme.Hairline,
                StrokeThickness = 1.5,
                StrokeDashArray = new DoubleCollection { 3, 3 },
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                BackgroundColor = Colors.Transparent,
                Content = DemoTheme.MakeIcon(DemoTheme.GlyphAdd, DemoTheme.SecondaryText, 20)
            };
            var addTap = new TapGestureRecognizer();
            addTap.Tapped += async (s, e) => await PickPhoto();
            addThumb.GestureRecognizers.Add(addTap);
            DemoControls.AddHover(addThumb);
            _thumbs.Children.Add(addThumb);

            // ---------- Panel: model ----------
            _modelNameLabel = new Label { FontFamily = DemoTheme.TitleFont, FontSize = 12, TextColor = DemoTheme.SecondaryText, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center };
            _versionControl = new SegmentedControl(Versions.Select(v => v.label), 0);
            _sizeControl = new SegmentedControl(Versions[0].sizes, 0);
            _versionControl.SelectionChanged += (s, e) =>
            {
                var sizes = Versions[_versionControl.SelectedIndex].sizes;
                string current = _sizeControl.SelectedItem;
                int keep = Array.IndexOf(sizes, current);
                _sizeControl.SetItems(sizes, keep < 0 ? 0 : keep);
                OnModelChanged();
            };
            _sizeControl.SelectionChanged += (s, e) => OnModelChanged();
            _modelStatusDot = new BoxView { WidthRequest = 6, HeightRequest = 6, CornerRadius = 3, Color = DemoTheme.StatusGreen, VerticalOptions = LayoutOptions.Center };
            _modelStatusLabel = new Label { FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center };
            _downloadChip = new Border
            {
                BackgroundColor = DemoTheme.PillBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) },
                Padding = new Thickness(10, 4),
                VerticalOptions = LayoutOptions.Center,
                Content = new HorizontalStackLayout
                {
                    Spacing = 4,
                    Children =
                    {
                        DemoTheme.MakeIcon(DemoTheme.GlyphDownload, DemoTheme.PrimaryText, 14),
                        new Label { Text = "DOWNLOAD", FontFamily = DemoTheme.TitleFont, FontSize = 10, CharacterSpacing = 1, TextColor = DemoTheme.PrimaryText, VerticalOptions = LayoutOptions.Center }
                    }
                }
            };
            var dlTap = new TapGestureRecognizer();
            dlTap.Tapped += async (s, e) => await EnsureModel();
            _downloadChip.GestureRecognizers.Add(dlTap);
            DemoControls.AddHover(_downloadChip);

            var modelHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            modelHeader.Add(DemoControls.SectionLabel("Model"), 0, 0);
            modelHeader.Add(_modelNameLabel, 1, 0);
            var modelControls = new FlexLayout
            {
                Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap,
                JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceBetween,
                Children = { _versionControl, _sizeControl }
            };
            _versionControl.Margin = new Thickness(0, 0, 8, 8);
            _sizeControl.Margin = new Thickness(0, 0, 0, 8);
            _modelSection = new VerticalStackLayout
            {
                Spacing = 12,
                Children =
                {
                    modelHeader,
                    modelControls,
                    new HorizontalStackLayout { Spacing = 7, Children = { _modelStatusDot, _modelStatusLabel, _downloadChip } }
                }
            };

            // ---------- Panel: confidence ----------
            _confidenceValue = new Label { Text = "0.25", FontFamily = DemoTheme.TitleFont, FontSize = 12, TextColor = DemoTheme.SecondaryText, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center };
            _confidence = new Slider
            {
                Minimum = MinConfidence,
                Maximum = 0.95,
                Value = 0.25,
                MinimumTrackColor = DemoTheme.PrimaryText,
                MaximumTrackColor = DemoTheme.Hairline,
                ThumbColor = DemoTheme.PrimaryText
            };
            _confidence.ValueChanged += (s, e) =>
            {
                _confidenceValue.Text = _confidence.Value.ToString("0.00");
                ShowResults(animate: false);
            };
            var confHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            confHeader.Add(DemoControls.SectionLabel("Confidence"), 0, 0);
            confHeader.Add(_confidenceValue, 1, 0);
            _confidenceSection = new VerticalStackLayout { Spacing = 8, Children = { confHeader, _confidence } };

            // ---------- Panel: run ----------
            _detectButton = DemoControls.PrimaryButton(IsDesktop ? "Detect   ⌘↵" : "Detect", DemoTheme.GlyphPlay);
            _detectButton.Clicked += async (s, e) => await RunDetection();

            // ---------- Panel: results ----------
            _resultsCount = new Label { FontFamily = DemoTheme.TitleFont, FontSize = 12, TextColor = DemoTheme.SecondaryText, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center };
            var resultsHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            resultsHeader.Add(DemoControls.SectionLabel("Results"), 0, 0);
            resultsHeader.Add(_resultsCount, 1, 0);
            _resultsList = new VerticalStackLayout { Spacing = 10 };
            _emptyCaption = new Label { Text = "run a detection to see labels here", FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.MutedText };
            _emptyState = new VerticalStackLayout
            {
                Spacing = 6,
                Children =
                {
                    new Label { Text = "no results yet", FontFamily = DemoTheme.HeroFont, FontSize = 32, CharacterSpacing = -1.2, LineHeight = 0.95, TextColor = DemoTheme.EmptyStateText },
                    _emptyCaption
                }
            };
            _resultsSection = new VerticalStackLayout { Spacing = 12, Children = { resultsHeader, _emptyState, _resultsList } };

            // ---------- Panel: performance ----------
            _perfBackend = new Label { Text = BackendName, FontFamily = DemoTheme.TitleFont, FontSize = 12, TextColor = DemoTheme.SecondaryText, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center };
            var perfHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            perfHeader.Add(DemoControls.SectionLabel("Performance"), 0, 0);
            perfHeader.Add(_perfBackend, 1, 0);
            var perfRow = new Grid { ColumnSpacing = 8, ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) } };
            (var inferenceTile, _perfInference) = PerfTile("inference");
            (var prePostTile, _perfPrePost) = PerfTile("pre + post");
            (var fpsTile, _perfFps) = PerfTile("live est.");
            perfRow.Add(inferenceTile, 0, 0);
            perfRow.Add(prePostTile, 1, 0);
            perfRow.Add(fpsTile, 2, 0);
            _perfSection = new VerticalStackLayout { Spacing = 12, Children = { perfHeader, perfRow } };

            // ---------- Panel: "View the C# behind this" ----------
            var codeRow = new Grid { ColumnSpacing = 10, Padding = new Thickness(0, 6), ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            codeRow.Add(DemoTheme.MakeIcon(DemoTheme.GlyphCode, DemoTheme.PrimaryText, 18), 0, 0);
            codeRow.Add(new Label { Text = "View the C# behind this", FontFamily = DemoTheme.TitleFont, FontSize = 14, TextColor = DemoTheme.PrimaryText, VerticalOptions = LayoutOptions.Center }, 1, 0);
            codeRow.Add(DemoTheme.MakeIcon(MaskRcnnPage.GlyphChevronRight, DemoTheme.MutedText, 18), 2, 0);
            var codeTap = new TapGestureRecognizer();
            codeTap.Tapped += (s, e) => ShowCodeSheet();
            codeRow.GestureRecognizers.Add(codeTap);
            DemoControls.AddHover(codeRow);
            _codeLink = codeRow;

            // ---------- Sheet (code / info) ----------
            _sheetScrim = new BoxView { Color = Color.FromArgb("#80000000"), Opacity = 0 };
            var scrimTap = new TapGestureRecognizer();
            scrimTap.Tapped += (s, e) => CloseSheet();
            _sheetScrim.GestureRecognizers.Add(scrimTap);
            _sheetCard = new Border
            {
                BackgroundColor = DemoTheme.PageBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                Padding = new Thickness(22),
                Margin = new Thickness(16),
                MaximumWidthRequest = 640,
                VerticalOptions = LayoutOptions.Center
            };
            _sheetOverlay = new Grid { IsVisible = false, Children = { _sheetScrim, _sheetCard } };

            UpdateModelLabels();
            ShowResults(animate: false);

            // ⌘↵ runs detection on the Mac (the menu bar carries the shortcut).
            var detectItem = new MenuFlyoutItem { Text = "Detect" };
            detectItem.KeyboardAccelerators.Add(new KeyboardAccelerator { Modifiers = KeyboardAcceleratorModifiers.Cmd, Key = "\r" });
            detectItem.Clicked += async (s, e) => await RunDetection();
            var demoMenu = new MenuBarItem { Text = "Demo" };
            demoMenu.Add(detectItem);
            MenuBarItems.Add(demoMenu);

            Content = _root;
            SizeChanged += (s, e) => BuildLayout();
        }

        private static bool IsDesktop => DeviceInfo.Idiom == DeviceIdiom.Desktop;

        private static string BackendName
        {
            get
            {
#if __IOS__ || __MACCATALYST__ || __ANDROID__
                return "CPU · OpenCV DNN";
#else
                bool gpu = CvInvoke.HaveOnnxRuntime && Emgu.CV.Cuda.CudaInvoke.HasCuda;
                return gpu ? "GPU · ONNX Runtime" : "CPU · OpenCV DNN";
#endif
            }
        }

        private string Selection
        {
            get
            {
                var v = Versions[Math.Max(0, _versionControl.SelectedIndex)];
                string size = _sizeControl.SelectedItem ?? v.sizes[0];
                return v.prefix + size.ToUpperInvariant();
            }
        }

        // e.g. "YOLO12n", "YOLOv10b"
        private string DisplayModelName
        {
            get
            {
                var v = Versions[Math.Max(0, _versionControl.SelectedIndex)];
                string num = v.label.Substring(1);
                return "YOLO" + (v.prefix.Contains("V") ? "v" : "") + num + (_sizeControl.SelectedItem ?? v.sizes[0]);
            }
        }

        // Local file the model downloads to (same folder / name Yolo.Init uses).
        private string ModelFilePath
        {
            get
            {
                string s = Selection.ToLowerInvariant();
                string file = s.StartsWith("yolov10") ? s + "_no_post_process.onnx" : s + ".onnx";
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "emgu", "yolo", file);
            }
        }

        // ================= Layout =================

        private void BuildLayout()
        {
            if (Width <= 0)
                return;
            bool wide = Width >= WideBreakpoint;
            if (_layoutBuilt && wide == _wide)
            {
                SizeStage();
                return;
            }
            _wide = wide;
            _layoutBuilt = true;

            foreach (var v in new View[] { _headerWide, _headerPhone, _stage, _thumbs, _modelSection, _confidenceSection, _detectButton, _resultsSection, _perfSection, _codeLink, _sheetOverlay })
                Detach(v);
            _root.Children.Clear();
            _root.RowDefinitions.Clear();

            if (wide)
            {
                // Desktop: stage card on the left, fixed-width settings panel on the right.
                var stageCard = new Grid
                {
                    RowSpacing = 0,
                    RowDefinitions = { new RowDefinition(GridLength.Star), new RowDefinition(GridLength.Auto) },
                };
                var well = DemoControls.Well(_stage);
                stageCard.Add(well, 0, 0);
                var under = new Grid { Margin = new Thickness(0, 14, 0, 0), ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
                under.Add(new Label { Text = "Drop an image here, or pick a sample", FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = DemoTheme.MutedText, VerticalOptions = LayoutOptions.Center }, 0, 0);
                under.Add(_thumbs, 1, 0);
                stageCard.Add(under, 0, 1);

                _detectButton.Margin = new Thickness(0);
                var panel = DemoControls.Well(new VerticalStackLayout
                {
                    Spacing = 20,
                    Children =
                    {
                        _modelSection, DemoControls.Hairline(),
                        _confidenceSection, _detectButton, DemoControls.Hairline(),
                        _resultsSection, DemoControls.Hairline(),
                        _perfSection, DemoControls.Hairline(),
                        _codeLink
                    }
                }, 22, new Thickness(20));

                var body = new Grid
                {
                    ColumnSpacing = 20,
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(new GridLength(PanelWidth)) }
                };
                body.Add(stageCard, 0, 0);
                body.Add(panel, 1, 0);

                var page = new VerticalStackLayout { Padding = new Thickness(32, 22, 32, 32), Spacing = 22, Children = { _headerWide, body } };
                _root.Add(new ScrollView { Content = page });
            }
            else
            {
                // Phone: one column; Detect pinned at the bottom.
                var column = new VerticalStackLayout
                {
                    Padding = new Thickness(16, 10, 16, 24),
                    Spacing = 18,
                    Children =
                    {
                        _headerPhone,
                        DemoControls.Well(_stage),
                        new ScrollView { Orientation = ScrollOrientation.Horizontal, HorizontalScrollBarVisibility = ScrollBarVisibility.Never, Content = _thumbs },
                        DemoControls.Hairline(), _modelSection,
                        DemoControls.Hairline(), _confidenceSection,
                        DemoControls.Hairline(), _resultsSection,
                        DemoControls.Hairline(), _perfSection,
                        DemoControls.Hairline(), _codeLink
                    }
                };
                _detectButton.Margin = new Thickness(0);
                var bottomBar = new Grid
                {
                    Padding = new Thickness(16, 10, 16, 10),
                    ColumnSpacing = 10,
                    BackgroundColor = DemoTheme.PageBackground,
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
                };
                bottomBar.Add(_detectButton, 0, 0);
                _root.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                _root.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                _root.Add(new ScrollView { Content = column }, 0, 0);
                _root.Add(bottomBar, 0, 1);
            }
            _root.Add(_sheetOverlay);
            if (_root.RowDefinitions.Count > 1)
                Grid.SetRowSpan(_sheetOverlay, 2);
            _sampleBadge.IsVisible = wide;
            SizeStage();
        }

        private void SizeStage()
        {
            double h = _wide ? Math.Clamp(Height - 430, 360, 720) : Math.Clamp(Width * 0.95, 260, 520);
            _stage.HeightRequest = h;
        }

        // Pin the two badges to the photo's own corners (the photo is aspect-fit
        // inside the stage, so it may not reach the stage edges).
        private void PlaceBadges()
        {
            double w = _overlay.Width, h = _overlay.Height;
            if (w <= 0 || h <= 0 || _drawable.ImageSize.Width <= 0)
                return;
            double scale = Math.Min(w / _drawable.ImageSize.Width, h / _drawable.ImageSize.Height);
            double ox = (w - _drawable.ImageSize.Width * scale) / 2 + 16 + 10;
            double oy = (h - _drawable.ImageSize.Height * scale) / 2 + 16 + 10;
            _resultBadge.Margin = new Thickness(ox, oy, 0, 0);
            _sampleBadge.Margin = new Thickness(0, oy, ox, 0);
        }

        private static void Detach(View v)
        {
            if (v?.Parent is Layout l)
                l.Children.Remove(v);
            else if (v?.Parent is Border b)
                b.Content = null;
            else if (v?.Parent is ScrollView sv)
                sv.Content = null;
        }

        private View BuildHeader(bool wide)
        {
            var back = DemoControls.CircleButton(DemoTheme.GlyphArrowBack, async () => await Navigation.PopAsync());
            var share = DemoControls.CircleButton(DemoTheme.GlyphShare, async () => await ShareResult());
            var info = DemoControls.CircleButton(DemoTheme.GlyphInfo, ShowInfoSheet);
            var right = new HorizontalStackLayout { Spacing = 8, HorizontalOptions = LayoutOptions.End, Children = { share, info } };

            var topRow = new Grid { ColumnSpacing = 12, ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            topRow.Add(back, 0, 0);
            if (wide)
            {
                var crumbs = new HorizontalStackLayout { Spacing = 8, VerticalOptions = LayoutOptions.Center };
                crumbs.Children.Add(Crumb("Demos", false, async () => await Navigation.PopAsync()));
                crumbs.Children.Add(Crumb("/", false, null));
                crumbs.Children.Add(Crumb(Category, false, async () => await Navigation.PopAsync()));
                crumbs.Children.Add(Crumb("/", false, null));
                crumbs.Children.Add(Crumb(Title_, true, null));
                topRow.Add(crumbs, 1, 0);
            }
            topRow.Add(right, 2, 0);

            double titleSize = wide ? 120 : 84;
            var title = new Label
            {
                Text = Title_,
                FontFamily = DemoTheme.HeroFont,
                FontSize = titleSize,
                CharacterSpacing = -titleSize * 0.05,
                LineHeight = 0.9,
                TextColor = DemoTheme.PrimaryText,
                Margin = new Thickness(-3, 0, 0, 0),
                VerticalOptions = LayoutOptions.End
            };
            var metaRow = new HorizontalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    DemoControls.Tag("DNN"),
                    new Label { Text = Category, FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center },
                    new Label { Text = "· dnn module", FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.MutedText, VerticalOptions = LayoutOptions.Center, IsVisible = wide }
                }
            };
            var desc = new Label { Text = Description, FontFamily = DemoTheme.BodyFont, FontSize = wide ? 16 : 15, TextColor = DemoTheme.SecondaryText };

            if (wide)
            {
                var titleRow = new Grid { ColumnSpacing = 24, Margin = new Thickness(0, 14, 0, 0), ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) } };
                titleRow.Add(title, 0, 0);
                titleRow.Add(new VerticalStackLayout { Spacing = 10, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(0, 18, 0, 0), Children = { metaRow, desc } }, 1, 0);
                return new VerticalStackLayout { Children = { topRow, titleRow } };
            }
            return new VerticalStackLayout
            {
                Spacing = 8,
                Children = { topRow, new VerticalStackLayout { Margin = new Thickness(0, 10, 0, 0), Children = { title } }, metaRow, desc }
            };
        }

        private static View Crumb(string text, bool current, Action onTap)
        {
            var l = new Label
            {
                Text = text,
                FontFamily = current ? DemoTheme.TitleFont : DemoTheme.BodyFont,
                FontSize = 13,
                TextColor = current ? DemoTheme.PrimaryText : DemoTheme.SecondaryText,
                VerticalOptions = LayoutOptions.Center
            };
            if (onTap != null)
            {
                var t = new TapGestureRecognizer();
                t.Tapped += (s, e) => onTap();
                l.GestureRecognizers.Add(t);
                DemoControls.AddHover(l);
            }
            return l;
        }

        private static (View tile, Label value) PerfTile(string caption)
        {
            var value = new Label { Text = "—", FontFamily = DemoTheme.TitleFont, FontSize = 17, TextColor = DemoTheme.PrimaryText };
            var tile = new Border
            {
                BackgroundColor = DemoTheme.PageBackground,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) },
                Padding = new Thickness(12, 10),
                Content = new VerticalStackLayout
                {
                    Spacing = 2,
                    Children = { value, new Label { Text = caption, FontFamily = DemoTheme.BodyFont, FontSize = 11, TextColor = DemoTheme.MutedText } }
                }
            };
            return (tile, value);
        }

        // ================= Lifecycle =================

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BuildLayout();
            if (!_loaded)
            {
                _loaded = true;
                await LoadSample(0);
            }
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            base.OnNavigatedFrom(args);
            if (!Navigation.NavigationStack.Contains(this) && !Navigation.ModalStack.Contains(this))
            {
                _image?.Dispose();
                _image = null;
                _model?.Dispose();
                _model = null;
            }
        }

        // ================= Images =================

        private async Task LoadSample(int idx)
        {
            try
            {
                byte[] bytes = await MaskRcnnPage.ReadAppFileAsync(Samples[idx].File);
                var m = new Mat();
                CvInvoke.Imdecode(bytes, ImreadModes.ColorBgr, m);
                if (m.IsEmpty) { m.Dispose(); return; }
                _sampleIndex = idx;
                SetImage(m, "Sample · " + Samples[idx].Name);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        private async Task PickPhoto()
        {
            try
            {
                FileResult file = (await MediaPicker.Default.PickPhotosAsync())?.FirstOrDefault();
                if (file == null)
                    return;
                using var stream = await file.OpenReadAsync();
                await LoadFromStream(stream, file.FileName);
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Could not load photo", ex.Message, "OK");
            }
        }

        private async Task LoadFromStream(System.IO.Stream stream, string name)
        {
            using var ms = new System.IO.MemoryStream();
            await stream.CopyToAsync(ms);
            var m = new Mat();
            CvInvoke.Imdecode(ms.ToArray(), ImreadModes.ColorBgr, m);
            if (m.IsEmpty)
            {
                m.Dispose();
                await DisplayAlertAsync("Photo", "That file could not be read as an image.", "OK");
                return;
            }
            _sampleIndex = -1;
            SetImage(MaskRcnnPage.Downscale(m, 1280), name ?? "Your photo");
        }

        // Desktop drag-and-drop of an image file onto the stage.
        private async void OnDrop(object sender, DropEventArgs e)
        {
#if __MACCATALYST__ || __IOS__
            var session = e.PlatformArgs?.DropSession;
            if (session == null || session.Items.Length == 0)
                return;
            var provider = session.Items[0].ItemProvider;
            if (!provider.HasItemConformingTo("public.image"))
                return;
            var data = await provider.LoadDataRepresentationAsync("public.image");
            if (data == null)
                return;
            using var stream = data.AsStream();
            await LoadFromStream(stream, provider.SuggestedName ?? "Dropped image");
#else
            await Task.CompletedTask;
#endif
        }

        private void SetImage(Mat m, string badge)
        {
            _image?.Dispose();
            _image = m;
            _imageName = badge;
            _photo.Source = MaskRcnnPage.MatToImageSource(m);
            _drawable.ImageSize = new Size(m.Width, m.Height);
            _all = Array.Empty<DetectedObject>();
            _sampleBadgeLabel.Text = badge;
            PlaceBadges();
            for (int i = 0; i < _thumbBorders.Count; i++)
                _thumbBorders[i].Stroke = i == _sampleIndex ? DemoTheme.PrimaryText : Colors.Transparent;
            _lastInferenceMs = _lastPrePostMs = 0;
            ShowResults(animate: false);
        }

        // ================= Model =================

        private void OnModelChanged()
        {
            if (_modelLoaded != null && _modelLoaded != Selection)
            {
                _model?.Clear();
                _modelLoaded = null;
            }
            UpdateModelLabels();
        }

        private void UpdateModelLabels()
        {
            _modelNameLabel.Text = DisplayModelName;
            var fi = new System.IO.FileInfo(ModelFilePath);
            if (fi.Exists)
            {
                _modelStatusDot.Color = DemoTheme.StatusGreen;
                _modelStatusLabel.Text = $"Downloaded · {fi.Length / (1024.0 * 1024.0):0.0} MB";
                _downloadChip.IsVisible = false;
            }
            else
            {
                _modelStatusDot.Color = DemoTheme.MutedText;
                _modelStatusLabel.Text = "Not downloaded";
                _downloadChip.IsVisible = true;
            }
        }

        // Download (if needed) and load the selected model. Returns false on failure.
        private async Task<bool> EnsureModel()
        {
            if (_model == null)
                return false;
            string sel = Selection;
            if (_modelLoaded == sel && _model.Initialized)
                return true;
            _model.Clear();
            _downloadChip.IsVisible = false;
            _modelStatusDot.Color = DemoTheme.Lime;
            _modelStatusLabel.Text = "Loading model…";
            await Task.Run(() => _model.Init(OnDownloadProgress, sel));
            if (!_model.Initialized)
            {
                UpdateModelLabels();
                _modelStatusLabel.Text = "Download failed · check your connection";
                return false;
            }
            _modelLoaded = sel;
            UpdateModelLabels();
            return true;
        }

        private void OnDownloadProgress(long? total, long received, double? percent)
        {
            string msg = total != null
                ? $"Downloading · {received / (1024.0 * 1024.0):0.0} of {total.Value / (1024.0 * 1024.0):0.0} MB"
                : $"Downloading · {received / (1024.0 * 1024.0):0.0} MB";
            MainThread.BeginInvokeOnMainThread(() => _modelStatusLabel.Text = msg);
        }

        // ================= Detection =================

        private async Task RunDetection()
        {
            if (_busy || _image == null || _model == null)
                return;
            _busy = true;
            _detectButton.IsEnabled = false;
            _detectButton.Text = "Detecting…";
            _stageSpinner.IsVisible = _stageSpinner.IsRunning = true;
            try
            {
                if (!await EnsureModel())
                {
                    _emptyCaption.Text = "the model could not be downloaded or loaded";
                    return;
                }
                Mat input = _image;
                var (objects, inferMs, prePostMs) = await Task.Run(() =>
                {
                    var total = Stopwatch.StartNew();
                    // "pre": make sure the input is a 3-channel BGR image.
                    using var bgr = new Mat();
                    if (input.NumberOfChannels == 4)
                        CvInvoke.CvtColor(input, bgr, ColorConversion.Bgra2Bgr);
                    else
                        input.CopyTo(bgr);
                    var infer = Stopwatch.StartNew();
                    DetectedObject[] found = _model.Detect(bgr, MinConfidence, 0.5);
                    infer.Stop();
                    // "post": sort the detections for display.
                    found = found.OrderByDescending(o => o.Confident).ToArray();
                    total.Stop();
                    return (found, infer.Elapsed.TotalMilliseconds, total.Elapsed.TotalMilliseconds - infer.Elapsed.TotalMilliseconds);
                });
                _all = objects;
                _lastInferenceMs = inferMs;
                _lastPrePostMs = prePostMs;
                _comparing = false;
                ShowResults(animate: true);
            }
            catch (Exception ex)
            {
                _emptyCaption.Text = "detection failed: " + ex.Message;
            }
            finally
            {
                _busy = false;
                _detectButton.IsEnabled = true;
                _detectButton.Text = IsDesktop ? "Detect   ⌘↵" : "Detect";
                _stageSpinner.IsVisible = _stageSpinner.IsRunning = false;
            }
        }

        private DetectedObject[] Visible => _all.Where(o => o.Confident >= _confidence.Value).ToArray();

        private static Color ColorFor(DetectedObject o) => DemoTheme.BoxColors[Math.Abs(o.ClassId) % DemoTheme.BoxColors.Length];

        private void ShowResults(bool animate)
        {
            var shown = Visible;
            bool ran = _lastInferenceMs > 0;

            _drawable.Boxes = shown.Select(o => new DetectionDrawable.Box(new RectF(o.Region.X, o.Region.Y, o.Region.Width, o.Region.Height), $"{o.Label} {o.Confident:0.00}", ColorFor(o))).ToArray();
            _overlay.IsVisible = !_comparing;
            if (animate)
            {
                _drawable.Alpha = 0;
                new Microsoft.Maui.Controls.Animation(v => { _drawable.Alpha = (float)v; _overlay.Invalidate(); }, 0, 1, Easing.CubicOut)
                    .Commit(this, "boxes", length: 300);
            }
            else
            {
                _drawable.Alpha = 1;
                _overlay.Invalidate();
            }

            _resultBadge.IsVisible = ran;
            _resultBadgeLabel.Text = $"{shown.Length} object{(shown.Length == 1 ? "" : "s")} · {_lastInferenceMs:0} ms";
            _resultsCount.Text = ran ? $"{shown.Length} object{(shown.Length == 1 ? "" : "s")}" : "";

            _resultsList.Children.Clear();
            _emptyState.IsVisible = !ran || shown.Length == 0;
            if (ran && shown.Length == 0)
                _emptyCaption.Text = "nothing above this confidence; try lowering it";
            else if (!ran)
                _emptyCaption.Text = "run a detection to see labels here";
            foreach (var o in shown.Take(12))
                _resultsList.Children.Add(ResultRow(o));
            if (shown.Length > 12)
                _resultsList.Children.Add(new Label { Text = $"+ {shown.Length - 12} more", FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = DemoTheme.MutedText });

            if (ran)
            {
                _perfInference.Text = $"{_lastInferenceMs:0} ms";
                _perfPrePost.Text = $"{_lastPrePostMs:0} ms";
                _perfFps.Text = $"{1000.0 / Math.Max(1, _lastInferenceMs + _lastPrePostMs):0} fps";
            }
            else
            {
                _perfInference.Text = _perfPrePost.Text = _perfFps.Text = "—";
            }
        }

        private static View ResultRow(DetectedObject o)
        {
            var color = ColorFor(o);
            var bar = new Grid
            {
                HeightRequest = 4,
                VerticalOptions = LayoutOptions.Center,
                ColumnDefinitions = { new ColumnDefinition(new GridLength(o.Confident, GridUnitType.Star)), new ColumnDefinition(new GridLength(1 - o.Confident, GridUnitType.Star)) }
            };
            bar.Add(new BoxView { Color = color, CornerRadius = 2 }, 0, 0);
            var row = new Grid
            {
                ColumnSpacing = 10,
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(new GridLength(84)), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
            };
            row.Add(new BoxView { WidthRequest = 9, HeightRequest = 9, CornerRadius = 2.5, Color = color, VerticalOptions = LayoutOptions.Center }, 0, 0);
            row.Add(new Label { Text = o.Label, FontFamily = DemoTheme.TitleFont, FontSize = 14, TextColor = DemoTheme.PrimaryText, LineBreakMode = LineBreakMode.TailTruncation, VerticalOptions = LayoutOptions.Center }, 1, 0);
            row.Add(bar, 2, 0);
            row.Add(new Label { Text = o.Confident.ToString("0.00"), FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center }, 3, 0);
            return row;
        }

        // ================= Toolbar actions =================

        private void ToggleCompare()
        {
            _comparing = !_comparing;
            _overlay.IsVisible = !_comparing;
            _compareButton.BackgroundColor = _comparing ? DemoTheme.PillBackground : Colors.Transparent;
        }

        // The current photo with the visible boxes drawn in, as PNG bytes.
        private byte[] RenderAnnotated()
        {
            if (_image == null)
                return null;
            using var copy = _image.Clone();
            foreach (var o in Visible)
            {
                var c = ColorFor(o);
                o.Render(copy, new MCvScalar(c.Blue * 255, c.Green * 255, c.Red * 255));
            }
            return MaskRcnnPage.Encode(copy);
        }

        private async void OpenFullscreen()
        {
            byte[] png = RenderAnnotated();
            if (png != null)
                await Navigation.PushModalAsync(new FullScreenImagePage(png, "YOLO"));
        }

        private async Task ShareResult()
        {
            byte[] png = RenderAnnotated();
            if (png == null)
                return;
            string path = System.IO.Path.Combine(FileSystem.CacheDirectory, "yolo-result.png");
            await System.IO.File.WriteAllBytesAsync(path, png);
            await Share.Default.RequestAsync(new ShareFileRequest { Title = "YOLO result", File = new ShareFile(path) });
        }

        private async Task OpenLiveCamera()
        {
#if __IOS__ || __MACCATALYST__
            await Navigation.PushModalAsync(new ModelShowcaseLivePage("YOLO", () => new Yolo(), Selection));
#else
            await Navigation.PushAsync(new Emgu.CV.Platform.Maui.UI.ProcessAndRenderPage(
                new Yolo(), "YOLO (Live)", null, "Real-time detection from the camera."));
#endif
        }

        // ================= Sheets =================

        private string CodeSnippet => $@"using Emgu.CV;
using Emgu.CV.Models;

using var yolo = new Yolo();
await yolo.Init(Yolo.YoloVersion.{Selection});

using Mat image = CvInvoke.Imread(""dog.png"");
DetectedObject[] objects =
    yolo.Detect(image, confThreshold: {_confidence.Value:0.00});

foreach (DetectedObject o in objects)
    Console.WriteLine(
        $""{{o.Label}} {{o.Confident:0.00}} at {{o.Region}}"");";

        private void ShowCodeSheet()
        {
            string code = CodeSnippet;
            var copyLabel = new Label { Text = "COPY", FontFamily = DemoTheme.TitleFont, FontSize = 11, CharacterSpacing = 1.2, TextColor = DemoTheme.PrimaryText, VerticalOptions = LayoutOptions.Center };
            var copy = new Border
            {
                BackgroundColor = DemoTheme.PillBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) },
                Padding = new Thickness(12, 7),
                VerticalOptions = LayoutOptions.Center,
                Content = new HorizontalStackLayout { Spacing = 6, Children = { DemoTheme.MakeIcon(DemoTheme.GlyphCopy, DemoTheme.PrimaryText, 15), copyLabel } }
            };
            var copyTap = new TapGestureRecognizer();
            copyTap.Tapped += async (s, e) =>
            {
                await Clipboard.Default.SetTextAsync(code);
                copyLabel.Text = "COPIED";
            };
            copy.GestureRecognizers.Add(copyTap);
            DemoControls.AddHover(copy);

            var head = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Auto) }, ColumnSpacing = 8 };
            head.Add(new Label { Text = "the C# behind this", FontFamily = DemoTheme.HeroFont, FontSize = 26, CharacterSpacing = -0.8, TextColor = DemoTheme.PrimaryText, VerticalOptions = LayoutOptions.Center }, 0, 0);
            head.Add(copy, 1, 0);
            head.Add(DemoControls.CircleButton(DemoTheme.GlyphClose, CloseSheet, 34), 2, 0);

            var codeBox = DemoControls.Well(new ScrollView
            {
                Orientation = ScrollOrientation.Both,
                Content = new Label { Text = code, FontFamily = "Menlo", FontSize = 13, TextColor = DemoTheme.PrimaryText, LineHeight = 1.25 }
            }, 14, new Thickness(16));

            OpenSheet(new VerticalStackLayout
            {
                Spacing = 14,
                Children =
                {
                    head,
                    new Label { Text = $"What this page runs for {DisplayModelName}, with your current confidence setting.", FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText },
                    codeBox
                }
            });
        }

        private void ShowInfoSheet()
        {
            var head = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            head.Add(new Label { Text = "about yolo", FontFamily = DemoTheme.HeroFont, FontSize = 26, CharacterSpacing = -0.8, TextColor = DemoTheme.PrimaryText, VerticalOptions = LayoutOptions.Center }, 0, 0);
            head.Add(DemoControls.CircleButton(DemoTheme.GlyphClose, CloseSheet, 34), 1, 0);
            OpenSheet(new VerticalStackLayout
            {
                Spacing = 12,
                Children =
                {
                    head,
                    new Label { Text = "Fast, general-purpose object detection: one neural network finds and labels every object in a single pass, hence \"You Only Look Once.\" It recognizes 80 everyday kinds of objects (people, animals, vehicles, household items).", FontFamily = DemoTheme.BodyFont, FontSize = 15, TextColor = DemoTheme.SecondaryText },
                    new Label { Text = "Sizes go from n (nano, fastest) to x (extra large, most accurate). Everything runs on this device; models download once, the first time you use them.", FontFamily = DemoTheme.BodyFont, FontSize = 15, TextColor = DemoTheme.SecondaryText }
                }
            });
        }

        private void OpenSheet(View content)
        {
            _sheetCard.Content = content;
            _sheetOverlay.IsVisible = true;
            _sheetScrim.Opacity = 0;
            _sheetCard.Opacity = 0;
            _sheetCard.Scale = 0.97;
            _ = _sheetScrim.FadeToAsync(1, 180, Easing.CubicOut);
            _ = _sheetCard.FadeToAsync(1, 180, Easing.CubicOut);
            _ = _sheetCard.ScaleToAsync(1, 180, Easing.CubicOut);
        }

        private async void CloseSheet()
        {
            await Task.WhenAll(_sheetScrim.FadeToAsync(0, 140), _sheetCard.FadeToAsync(0, 140));
            _sheetOverlay.IsVisible = false;
        }
    }

    /// <summary>
    /// Draws detection boxes and their label chips over an aspect-fit photo. Boxes
    /// are in image pixels; the drawable maps them to the view the same way
    /// Aspect.AspectFit places the image.
    /// </summary>
    internal class DetectionDrawable : IDrawable
    {
        public sealed record Box(RectF Rect, string Label, Color Color);

        public Box[] Boxes { get; set; } = Array.Empty<Box>();
        public Size ImageSize { get; set; }
        public float Alpha { get; set; } = 1;

        public void Draw(ICanvas canvas, RectF dirty)
        {
            if (ImageSize.Width <= 0 || ImageSize.Height <= 0 || Boxes.Length == 0)
                return;
            float scale = (float)Math.Min(dirty.Width / ImageSize.Width, dirty.Height / ImageSize.Height);
            float ox = (float)(dirty.X + (dirty.Width - ImageSize.Width * scale) / 2);
            float oy = (float)(dirty.Y + (dirty.Height - ImageSize.Height * scale) / 2);
            canvas.Alpha = Alpha;
            canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;
            canvas.FontSize = 11;

            foreach (var b in Boxes)
            {
                var r = new RectF(ox + b.Rect.X * scale, oy + b.Rect.Y * scale, b.Rect.Width * scale, b.Rect.Height * scale);
                canvas.StrokeColor = b.Color;
                canvas.StrokeSize = 2;
                canvas.DrawRoundedRectangle(r, 3);

                SizeF ts = canvas.GetStringSize(b.Label, Microsoft.Maui.Graphics.Font.DefaultBold, 11);
                float cw = ts.Width + 12, ch = 18;
                float cx = r.X - 1;
                float cy = r.Y - ch - 3;
                if (cy < oy) cy = r.Y + 3;     // no room above: put the chip inside the box
                canvas.FillColor = b.Color;
                canvas.FillRoundedRectangle(cx, cy, cw, ch, 5);
                canvas.FontColor = Color.FromArgb("#0E0E0E");
                canvas.DrawString(b.Label, cx, cy, cw, ch, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
        }
    }
}
