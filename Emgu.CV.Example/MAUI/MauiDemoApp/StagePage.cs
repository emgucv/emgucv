//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

using Microsoft.Maui.Controls.Shapes;

namespace MauiDemoApp
{
    /// <summary>A sample input image offered as a thumbnail under the stage.</summary>
    public sealed record StageSample(string File, string Name);

    /// <summary>What a demo shows about itself: titles, samples and panel options.</summary>
    public sealed class StageConfig
    {
        public string Title;                 // lowercase-able demo name, e.g. "yolo"
        public string Tag;                   // e.g. "DNN"; null for none
        public string Category;              // e.g. "Detection"
        public string Module;                // e.g. "dnn module"; null for none
        public string Description;           // one line under the title
        public string About;                 // paragraph(s) for the info sheet
        public StageSample[] Samples = Array.Empty<StageSample>();
        public bool HasCamera;
        public bool HasConfidence;
        public double ConfidenceDefault = 0.25;
        public double ConfidenceMin = 0.05;
        public string RunLabel = "Run";
        public bool AutoRun;                 // run once when the page opens (generated demos)
        public string OptionsTitle = "Model";
        public string EmptyTitle = "no results yet";
        public string EmptyCaption = "run it to see results here";
        public string ItemSingular = "object";
        public string ItemPlural = "objects";
        public string ResultsTitle = "Results";
        public bool ShowResultCount = true;   // "N objects" next to the results title
        public int MaxImageSize = 1280;      // user photos are downscaled to this
    }

    /// <summary>A box drawn over the input image, in image pixels.</summary>
    public sealed record StageBox(RectF Rect, string Label, Color Color, double Score = 1);

    /// <summary>A line in the results list.</summary>
    public sealed record StageRow(string Label, string Detail = null, double? Fraction = null, Color Color = null, double Score = 1);

    /// <summary>The outcome of one run.</summary>
    public sealed class StageResult
    {
        public byte[] OutputPng;             // replaces the input on the stage (compare shows a split)
        public int OutputWidth, OutputHeight;
        public StageBox[] Boxes = Array.Empty<StageBox>();
        public StageRow[] Rows = Array.Empty<StageRow>();
        public string Note;                  // free text under the rows (e.g. a message)
        public string Summary;               // badge text; default "N objects · X ms"
        public double InferenceMs;
        public double PrePostMs;
    }

    /// <summary>
    /// The redesigned demo layout shared by every image demo: a large image stage
    /// (boxes and label chips drawn over the photo, a floating Photo / Camera,
    /// compare and fullscreen toolbar, sample thumbnails) and a settings panel
    /// (options, optional confidence, Run, results, performance and the C# behind
    /// it). Desktop shows them side by side; phones stack them with Run pinned to
    /// the bottom. Each demo supplies a <see cref="StageConfig"/>, its options view
    /// and <see cref="RunAsync"/>.
    /// </summary>
    public abstract class StagePage : ContentPage
    {
        private const double PanelWidth = 380;
        private const double WideBreakpoint = 900;

        protected readonly StageConfig Config;

        // ---------- State ----------
        private Mat _image;                   // current input (BGR); null for generated demos
        private int _sampleIndex = -1;
        private StageResult _result;
        private bool _busy, _wide, _built, _layoutBuilt, _loaded, _comparing;
        private double _split = 0.5;

        // ---------- Views ----------
        private View _headerWide, _headerPhone;
        private Grid _stage;
        private Image _photo, _photoBefore;
        private GraphicsView _overlay;
        private readonly DetectionDrawable _drawable = new DetectionDrawable();
        private Border _resultBadge, _sampleBadge;
        private Label _resultBadgeLabel, _sampleBadgeLabel;
        private ActivityIndicator _stageSpinner;
        private Label _stageEmpty;
        private Grid _splitHandle;
        private Border _beforeBadge, _afterBadge;
        private Label _compareLabel;
        private Border _toolbar, _compareButton;
        private SegmentedControl _sourceToggle;
        private HorizontalStackLayout _thumbs;
        private readonly List<Border> _thumbBorders = new List<Border>();
        private Label _optionsValue, _statusLabel;
        private BoxView _statusDot;
        private Border _downloadChip;
        private Slider _confidence;
        private Label _confidenceValue;
        private Button _runButton;
        private Label _resultsCount, _emptyCaption, _note;
        private VerticalStackLayout _resultsList, _emptyState;
        private Label _perfInference, _perfPrePost, _perfFps;
        private View _optionsSection, _confidenceSection, _resultsSection, _perfSection, _codeLink;
        private Grid _sheetOverlay;
        private Border _sheetCard;
        private BoxView _sheetScrim;
        private readonly Grid _root = new Grid();

        protected StagePage(StageConfig config)
        {
            Config = config;
            Shell.SetNavBarIsVisible(this, false);
            BackgroundColor = DemoTheme.PageBackground;
            Title = config.Title;
            Content = _root;
            SizeChanged += (s, e) => BuildLayout();

            // Cmd+Return runs the demo on the Mac (the menu bar carries the shortcut).
            var runItem = new MenuFlyoutItem { Text = config.RunLabel };
            runItem.KeyboardAccelerators.Add(new KeyboardAccelerator { Modifiers = KeyboardAcceleratorModifiers.Cmd, Key = "\r" });
            runItem.Clicked += async (s, e) => await Run();
            var menu = new MenuBarItem { Text = "Demo" };
            menu.Add(runItem);
            MenuBarItems.Add(menu);
        }

        // ================= What a demo provides =================

        /// <summary>Options shown at the top of the panel (model pickers, toggles). Null for none.</summary>
        protected virtual View BuildOptions() => null;

        /// <summary>Run the demo on <paramref name="input"/> (null for demos without samples). Called off the UI thread.</summary>
        protected abstract StageResult RunCore(Mat input);

        /// <summary>Prepare before running (e.g. download / load a model) on the UI thread. Return false to cancel.</summary>
        protected virtual Task<bool> PrepareAsync() => Task.FromResult(true);

        /// <summary>The short C# snippet behind this demo. Null hides the link.</summary>
        protected virtual string CodeSnippet => null;

        /// <summary>Open the live camera page for this demo.</summary>
        protected virtual Task OpenLiveCameraAsync() => Task.CompletedTask;

        /// <summary>"CPU · OpenCV DNN" etc., shown next to Performance.</summary>
        protected virtual string BackendName => "CPU · OpenCV";

        /// <summary>The input, for demos that read it on the UI thread.</summary>
        protected Mat CurrentImage => _image;

        protected double Confidence => _confidence?.Value ?? 0;

        protected static bool IsDesktop => DeviceInfo.Idiom == DeviceIdiom.Desktop;

        /// <summary>Text on the right of the options header (e.g. "YOLO12n").</summary>
        protected void SetOptionsValue(string text)
        {
            if (_optionsValue != null)
                _optionsValue.Text = text;
        }

        /// <summary>Status line under the options: dot, text, and an optional Download chip.</summary>
        protected void SetStatus(string text, Color dot = null, bool showDownload = false)
        {
            if (_statusLabel == null)
                return;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _statusLabel.Text = text ?? "";
                _statusLabel.IsVisible = !string.IsNullOrEmpty(text);
                _statusDot.IsVisible = _statusLabel.IsVisible;
                _statusDot.Color = dot ?? DemoTheme.StatusGreen;
                _downloadChip.IsVisible = showDownload;
            });
        }

        /// <summary>Forget the last result (e.g. the model changed).</summary>
        protected void ClearResult()
        {
            _result = null;
            ShowResult(animate: false);
        }

        /// <summary>Redraw the results (e.g. a filter changed) without running again.</summary>
        protected void RefreshResult() => ShowResult(animate: false);

        /// <summary>Called after options are built, to fill the status line.</summary>
        protected virtual void OnStatusInit() { }

        /// <summary>Progress callback for model downloads: shows "Downloading · x of y MB".</summary>
        protected void OnDownloadProgress(long? total, long received, double? percent)
        {
            string msg = total != null
                ? $"Downloading · {received / (1024.0 * 1024.0):0.0} of {total.Value / (1024.0 * 1024.0):0.0} MB"
                : $"Downloading · {received / (1024.0 * 1024.0):0.0} MB";
            SetStatus(msg, DemoTheme.Lime);
        }

        // ================= Build (lazily, once the subclass is constructed) =================

        private void EnsureBuilt()
        {
            if (_built)
                return;
            _built = true;
            var c = Config;

            _headerWide = BuildHeader(wide: true);
            _headerPhone = BuildHeader(wide: false);

            // ---------- Stage ----------
            var photoMargin = new Thickness(16, 16, 16, 70);
            _photoBefore = new Image { Aspect = Aspect.AspectFit, Margin = photoMargin, IsVisible = false };
            _photo = new Image { Aspect = Aspect.AspectFit, Margin = photoMargin };
            _overlay = new GraphicsView { Drawable = _drawable, InputTransparent = true, Margin = photoMargin };
            (_resultBadge, _resultBadgeLabel, _) = DemoControls.Badge("");
            _resultBadge.IsVisible = false;
            (_sampleBadge, _sampleBadgeLabel, var sampleDot) = DemoControls.Badge("");
            sampleDot.IsVisible = false;
            _sampleBadge.HorizontalOptions = LayoutOptions.End;
            _sampleBadge.IsVisible = false;
            _stageSpinner = new ActivityIndicator { Color = DemoTheme.Lime, IsRunning = false, IsVisible = false, WidthRequest = 36, HeightRequest = 36, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            _stageEmpty = new Label { Text = "press " + c.RunLabel.ToLowerInvariant(), FontFamily = DemoTheme.HeroFont, FontSize = 30, CharacterSpacing = -1, TextColor = DemoTheme.EmptyStateText, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, IsVisible = c.Samples.Length == 0 };
            // Before / after divider: a white line with a round knob (◂ ▸) that you drag.
            var knob = new Border
            {
                WidthRequest = 38,
                HeightRequest = 38,
                BackgroundColor = Colors.White,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(19) },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Shadow = new Shadow { Brush = Colors.Black, Opacity = 0.35f, Radius = 8, Offset = new Point(0, 2) },
                Content = new HorizontalStackLayout
                {
                    Spacing = -4,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        DemoTheme.MakeIcon(MaskRcnnPage.GlyphChevronLeft, Color.FromArgb("#0E0E0E"), 18),
                        DemoTheme.MakeIcon(MaskRcnnPage.GlyphChevronRight, Color.FromArgb("#0E0E0E"), 18)
                    }
                }
            };
            _splitHandle = new Grid
            {
                WidthRequest = 38,
                IsVisible = false,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 16, 0, 70),
                InputTransparent = true,
                Children = { new BoxView { WidthRequest = 2, Color = Colors.White, HorizontalOptions = LayoutOptions.Center }, knob }
            };
            (_beforeBadge, var beforeLabel, var beforeDot) = DemoControls.Badge("BEFORE");
            (_afterBadge, var afterLabel, var afterDot) = DemoControls.Badge("AFTER");
            beforeDot.IsVisible = afterDot.IsVisible = false;
            beforeLabel.CharacterSpacing = afterLabel.CharacterSpacing = 1.2;
            _afterBadge.HorizontalOptions = LayoutOptions.End;
            _beforeBadge.IsVisible = _afterBadge.IsVisible = false;
            _beforeBadge.InputTransparent = _afterBadge.InputTransparent = true;
            _stage = new Grid { Children = { _photoBefore, _photo, _overlay, _splitHandle, _stageEmpty, _resultBadge, _sampleBadge, _beforeBadge, _afterBadge, _stageSpinner } };
            _overlay.SizeChanged += (s, e) => { PlaceBadges(); UpdateSplit(); };
            var pan = new PanGestureRecognizer();
            pan.PanUpdated += OnSplitPan;
            _stage.GestureRecognizers.Add(pan);
            var drop = new DropGestureRecognizer { AllowDrop = c.Samples.Length > 0 };
            drop.DragOver += (s, e) => e.AcceptedOperation = DataPackageOperation.Copy;
            drop.Drop += OnDrop;
            _stage.GestureRecognizers.Add(drop);

            // Floating toolbar: Photo | Camera, compare, fullscreen.
            var toolbarRow = new HorizontalStackLayout { Spacing = 4 };
            if (c.HasCamera)
            {
                _sourceToggle = new SegmentedControl(new[] { "Photo", "Camera" }, 0, 13);
                _sourceToggle.SelectionChanged += async (s, e) =>
                {
                    if (_sourceToggle.SelectedIndex == 1)
                    {
                        await OpenLiveCameraAsync();
                        _sourceToggle.SelectedIndex = 0;
                    }
                };
                toolbarRow.Children.Add(_sourceToggle);
                toolbarRow.Children.Add(new BoxView { WidthRequest = 1, HeightRequest = 20, Color = DemoTheme.Hairline, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(4, 0) });
            }
            // A labelled pill, so it's obvious what it does.
            _compareLabel = new Label { Text = "Compare", FontFamily = DemoTheme.TitleFont, FontSize = 13, TextColor = DemoTheme.PrimaryText, VerticalOptions = LayoutOptions.Center };
            _compareButton = new Border
            {
                BackgroundColor = Colors.Transparent,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) },
                Padding = new Thickness(12, 7),
                VerticalOptions = LayoutOptions.Center,
                Content = new HorizontalStackLayout { Spacing = 6, Children = { DemoTheme.MakeIcon(DemoTheme.GlyphCompare, DemoTheme.PrimaryText, 17), _compareLabel } }
            };
            var compareTap = new TapGestureRecognizer();
            compareTap.Tapped += (s, e) => ToggleCompare();
            _compareButton.GestureRecognizers.Add(compareTap);
            DemoControls.AddHover(_compareButton);
            toolbarRow.Children.Add(_compareButton);
            toolbarRow.Children.Add(DemoControls.CircleButton(DemoTheme.GlyphFullscreen, OpenFullscreen, 34, Colors.Transparent));
            _toolbar = new Border
            {
                BackgroundColor = DemoTheme.PageBackground,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) },
                Padding = new Thickness(4),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                Margin = new Thickness(0, 0, 0, 14),
                Shadow = new Shadow { Brush = Colors.Black, Opacity = DemoTheme.IsDark ? 0.6f : 0.08f, Radius = 10, Offset = new Point(0, 2) },
                Content = toolbarRow
            };
            _stage.Children.Add(_toolbar);

            // Sample thumbnails + "+" for your own photo.
            _thumbs = new HorizontalStackLayout { Spacing = 8 };
            for (int i = 0; i < c.Samples.Length; i++)
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
                    Content = new Image { Aspect = Aspect.AspectFill, Source = ImageSource.FromStream(ct => FileSystem.OpenAppPackageFileAsync(c.Samples[idx].File)) }
                };
                var t = new TapGestureRecognizer();
                t.Tapped += async (s, e) => await LoadSample(idx);
                thumb.GestureRecognizers.Add(t);
                DemoControls.AddHover(thumb);
                _thumbBorders.Add(thumb);
                _thumbs.Children.Add(thumb);
            }
            if (c.Samples.Length > 0)
            {
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
            }

            // ---------- Panel: options + status ----------
            _optionsValue = new Label { FontFamily = DemoTheme.TitleFont, FontSize = 12, TextColor = DemoTheme.SecondaryText, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center };
            _statusDot = new BoxView { WidthRequest = 6, HeightRequest = 6, CornerRadius = 3, Color = DemoTheme.StatusGreen, VerticalOptions = LayoutOptions.Center, IsVisible = false };
            _statusLabel = new Label { FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center, IsVisible = false };
            _downloadChip = new Border
            {
                BackgroundColor = DemoTheme.PillBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) },
                Padding = new Thickness(10, 4),
                VerticalOptions = LayoutOptions.Center,
                IsVisible = false,
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
            dlTap.Tapped += async (s, e) => await PrepareAsync();
            _downloadChip.GestureRecognizers.Add(dlTap);
            DemoControls.AddHover(_downloadChip);

            View options = BuildOptions();
            var optionsHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            optionsHeader.Add(DemoControls.SectionLabel(c.OptionsTitle), 0, 0);
            optionsHeader.Add(_optionsValue, 1, 0);
            var optionsStack = new VerticalStackLayout { Spacing = 12, Children = { optionsHeader } };
            if (options != null)
                optionsStack.Children.Add(options);
            optionsStack.Children.Add(new HorizontalStackLayout { Spacing = 7, Children = { _statusDot, _statusLabel, _downloadChip } });
            _optionsSection = optionsStack;

            // ---------- Panel: confidence ----------
            if (c.HasConfidence)
            {
                _confidenceValue = new Label { Text = c.ConfidenceDefault.ToString("0.00"), FontFamily = DemoTheme.TitleFont, FontSize = 12, TextColor = DemoTheme.SecondaryText, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center };
                _confidence = new Slider
                {
                    Minimum = c.ConfidenceMin,
                    Maximum = 0.95,
                    Value = c.ConfidenceDefault,
                    MinimumTrackColor = DemoTheme.PrimaryText,
                    MaximumTrackColor = DemoTheme.Hairline,
                    ThumbColor = DemoTheme.PrimaryText
                };
                _confidence.ValueChanged += (s, e) =>
                {
                    _confidenceValue.Text = _confidence.Value.ToString("0.00");
                    ShowResult(animate: false);
                };
                var confHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
                confHeader.Add(DemoControls.SectionLabel("Confidence"), 0, 0);
                confHeader.Add(_confidenceValue, 1, 0);
                _confidenceSection = new VerticalStackLayout { Spacing = 8, Children = { confHeader, _confidence } };
            }

            // ---------- Panel: run ----------
            _runButton = DemoControls.PrimaryButton(RunText, DemoTheme.GlyphPlay);
            _runButton.Clicked += async (s, e) => await Run();

            // ---------- Panel: results ----------
            _resultsCount = new Label { FontFamily = DemoTheme.TitleFont, FontSize = 12, TextColor = DemoTheme.SecondaryText, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center };
            var resultsHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            resultsHeader.Add(DemoControls.SectionLabel(c.ResultsTitle), 0, 0);
            resultsHeader.Add(_resultsCount, 1, 0);
            _resultsList = new VerticalStackLayout { Spacing = 10 };
            _note = new Label { FontFamily = DemoTheme.BodyFont, FontSize = 14, TextColor = DemoTheme.PrimaryText, IsVisible = false };
            _emptyCaption = new Label { Text = c.EmptyCaption, FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.MutedText };
            _emptyState = new VerticalStackLayout
            {
                Spacing = 6,
                Children =
                {
                    new Label { Text = c.EmptyTitle, FontFamily = DemoTheme.HeroFont, FontSize = 32, CharacterSpacing = -1.2, LineHeight = 0.95, TextColor = DemoTheme.EmptyStateText },
                    _emptyCaption
                }
            };
            _resultsSection = new VerticalStackLayout { Spacing = 12, Children = { resultsHeader, _emptyState, _resultsList, _note } };

            // ---------- Panel: performance ----------
            var perfHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            perfHeader.Add(DemoControls.SectionLabel("Performance"), 0, 0);
            perfHeader.Add(new Label { Text = BackendName, FontFamily = DemoTheme.TitleFont, FontSize = 12, TextColor = DemoTheme.SecondaryText, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center }, 1, 0);
            var perfRow = new Grid { ColumnSpacing = 8, ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) } };
            (var t1, _perfInference) = PerfTile("inference");
            (var t2, _perfPrePost) = PerfTile("pre + post");
            (var t3, _perfFps) = PerfTile("live est.");
            perfRow.Add(t1, 0, 0);
            perfRow.Add(t2, 1, 0);
            perfRow.Add(t3, 2, 0);
            _perfSection = new VerticalStackLayout { Spacing = 12, Children = { perfHeader, perfRow } };

            // ---------- Panel: the C# behind this ----------
            if (CodeSnippet != null)
            {
                var codeRow = new Grid { ColumnSpacing = 10, Padding = new Thickness(0, 6), ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
                codeRow.Add(DemoTheme.MakeIcon(DemoTheme.GlyphCode, DemoTheme.PrimaryText, 18), 0, 0);
                codeRow.Add(new Label { Text = "View the C# behind this", FontFamily = DemoTheme.TitleFont, FontSize = 14, TextColor = DemoTheme.PrimaryText, VerticalOptions = LayoutOptions.Center }, 1, 0);
                codeRow.Add(DemoTheme.MakeIcon(MaskRcnnPage.GlyphChevronRight, DemoTheme.MutedText, 18), 2, 0);
                var codeTap = new TapGestureRecognizer();
                codeTap.Tapped += (s, e) => ShowCodeSheet();
                codeRow.GestureRecognizers.Add(codeTap);
                DemoControls.AddHover(codeRow);
                _codeLink = codeRow;
            }

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

            OnStatusInit();
            ShowResult(animate: false);
        }

        private string RunText => IsDesktop ? Config.RunLabel + "   ⌘↵" : Config.RunLabel;

        // ================= Layout =================

        private void BuildLayout()
        {
            if (Width <= 0)
                return;
            EnsureBuilt();
            bool wide = Width >= WideBreakpoint;
            if (_layoutBuilt && wide == _wide)
            {
                SizeStage();
                return;
            }
            _wide = wide;
            _layoutBuilt = true;

            foreach (var v in new View[] { _headerWide, _headerPhone, _stage, _thumbs, _optionsSection, _confidenceSection, _runButton, _resultsSection, _perfSection, _codeLink, _sheetOverlay })
                Detach(v);
            _root.Children.Clear();
            _root.RowDefinitions.Clear();

            var panelItems = new List<View> { _optionsSection, DemoControls.Hairline() };
            if (_confidenceSection != null)
                panelItems.Add(_confidenceSection);

            if (wide)
            {
                var stageCard = new Grid { RowDefinitions = { new RowDefinition(GridLength.Star), new RowDefinition(GridLength.Auto) } };
                stageCard.Add(DemoControls.Well(_stage), 0, 0);
                var under = new Grid { Margin = new Thickness(0, 14, 0, 0), ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
                if (Config.Samples.Length > 0)
                {
                    under.Add(new Label { Text = "Drop an image here, or pick a sample", FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = DemoTheme.MutedText, VerticalOptions = LayoutOptions.Center }, 0, 0);
                    under.Add(_thumbs, 1, 0);
                    stageCard.Add(under, 0, 1);
                }

                panelItems.Add(_runButton);
                panelItems.AddRange(new View[] { DemoControls.Hairline(), _resultsSection, DemoControls.Hairline(), _perfSection });
                if (_codeLink != null)
                    panelItems.AddRange(new View[] { DemoControls.Hairline(), _codeLink });
                var panelStack = new VerticalStackLayout { Spacing = 20 };
                foreach (var v in panelItems)
                    panelStack.Children.Add(v);
                var panel = DemoControls.Well(panelStack, 22, new Thickness(20));

                var body = new Grid { ColumnSpacing = 20, ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(new GridLength(PanelWidth)) } };
                body.Add(stageCard, 0, 0);
                body.Add(panel, 1, 0);

                var page = new VerticalStackLayout { Padding = new Thickness(32, 22, 32, 32), Spacing = 22, Children = { _headerWide, body } };
                _root.Add(new ScrollView { Content = page });
            }
            else
            {
                var column = new VerticalStackLayout { Padding = new Thickness(16, 10, 16, 24), Spacing = 18, Children = { _headerPhone, DemoControls.Well(_stage) } };
                if (Config.Samples.Length > 0)
                    column.Children.Add(new ScrollView { Orientation = ScrollOrientation.Horizontal, HorizontalScrollBarVisibility = ScrollBarVisibility.Never, Content = _thumbs });
                foreach (var v in panelItems)
                    column.Children.Add(v);
                foreach (var v in new View[] { DemoControls.Hairline(), _resultsSection, DemoControls.Hairline(), _perfSection })
                    column.Children.Add(v);
                if (_codeLink != null)
                {
                    column.Children.Add(DemoControls.Hairline());
                    column.Children.Add(_codeLink);
                }
                var bottomBar = new Grid { Padding = new Thickness(16, 10), BackgroundColor = DemoTheme.PageBackground };
                bottomBar.Add(_runButton);
                _root.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                _root.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                _root.Add(new ScrollView { Content = column }, 0, 0);
                _root.Add(bottomBar, 0, 1);
            }
            _root.Add(_sheetOverlay);
            if (_root.RowDefinitions.Count > 1)
                Grid.SetRowSpan(_sheetOverlay, 2);
            _sampleBadge.IsVisible = wide && _sampleBadgeLabel.Text?.Length > 0;
            SizeStage();
        }

        private void SizeStage()
        {
            if (_stage == null)
                return;
            if (_wide)
            {
                _stage.HeightRequest = Math.Clamp(Height - 430, 360, 720);
                return;
            }
            // Phone: fit the box to the image's shape so a wide picture doesn't
            // sit in a tall box with empty bands above and below it.
            var sz = StageImageSize;
            double photoWidth = Width - 32 - 32;          // page padding + photo margins
            double photoHeight = sz.Width > 0 ? photoWidth * sz.Height / sz.Width : photoWidth * 0.75;
            _stage.HeightRequest = Math.Clamp(photoHeight + 16 + 70, 220, 560);
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

        // The size of what's on the stage now (output if any, else input).
        private Size StageImageSize =>
            _result?.OutputPng != null && _result.OutputWidth > 0 ? new Size(_result.OutputWidth, _result.OutputHeight)
            : _image != null ? new Size(_image.Width, _image.Height) : Size.Zero;

        // Fitted rectangle of the photo inside the overlay view.
        private Rect FitRect()
        {
            double w = _overlay.Width, h = _overlay.Height;
            var sz = StageImageSize;
            if (w <= 0 || h <= 0 || sz.Width <= 0)
                return Rect.Zero;
            double scale = Math.Min(w / sz.Width, h / sz.Height);
            return new Rect((w - sz.Width * scale) / 2, (h - sz.Height * scale) / 2, sz.Width * scale, sz.Height * scale);
        }

        private void PlaceBadges()
        {
            var r = FitRect();
            if (r.Width <= 0)
                return;
            double ox = r.X + 16 + 10, oy = r.Y + 16 + 10;
            _resultBadge.Margin = new Thickness(ox, oy, 0, 0);
            _sampleBadge.Margin = new Thickness(0, oy, ox, 0);
            _beforeBadge.Margin = new Thickness(ox, oy, 0, 0);
            _afterBadge.Margin = new Thickness(0, oy, ox, 0);
        }

        private View BuildHeader(bool wide)
        {
            var c = Config;
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
                if (!string.IsNullOrEmpty(c.Category))
                {
                    crumbs.Children.Add(Crumb("/", false, null));
                    crumbs.Children.Add(Crumb(c.Category, false, async () => await Navigation.PopAsync()));
                }
                crumbs.Children.Add(Crumb("/", false, null));
                crumbs.Children.Add(Crumb(c.Title.ToLowerInvariant(), true, null));
                topRow.Add(crumbs, 1, 0);
            }
            topRow.Add(right, 2, 0);

            string titleText = c.Title.ToLowerInvariant();
            // Long names step down so they still fit.
            double titleSize = wide ? (titleText.Length <= 8 ? 120 : titleText.Length <= 14 ? 96 : 76)
                                    : (titleText.Length <= 6 ? 84 : titleText.Length <= 10 ? 64 : 48);
            var title = new Label
            {
                Text = titleText,
                FontFamily = DemoTheme.HeroFont,
                FontSize = titleSize,
                CharacterSpacing = -titleSize * 0.05,
                LineHeight = 0.9,
                TextColor = DemoTheme.PrimaryText,
                Margin = new Thickness(-3, 0, 0, 0),
                // Tight negative spacing makes the label measure narrower than the
                // glyphs it draws; pad the right so the last letter isn't clipped.
                Padding = new Thickness(0, 0, titleSize * 0.12, 0),
                VerticalOptions = LayoutOptions.End
            };
            var metaRow = new HorizontalStackLayout { Spacing = 8 };
            if (!string.IsNullOrEmpty(c.Tag))
                metaRow.Children.Add(DemoControls.Tag(c.Tag));
            if (!string.IsNullOrEmpty(c.Category))
                metaRow.Children.Add(new Label { Text = c.Category, FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center });
            if (wide && !string.IsNullOrEmpty(c.Module))
                metaRow.Children.Add(new Label { Text = "· " + c.Module, FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.MutedText, VerticalOptions = LayoutOptions.Center });
            var desc = new Label { Text = c.Description, FontFamily = DemoTheme.BodyFont, FontSize = wide ? 16 : 15, TextColor = DemoTheme.SecondaryText };

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
            EnsureBuilt();
            BuildLayout();
            if (!_loaded)
            {
                _loaded = true;
                if (Config.Samples.Length > 0)
                    await LoadSample(0);
                if (Config.AutoRun)
                    await Run();
                //TEMP-AUTOOPEN-BEGIN
                if (!Config.AutoRun && Environment.GetEnvironmentVariable("EMGU_DEMO_AUTORUN") != null) await Run();
                if (Environment.GetEnvironmentVariable("EMGU_DEMO_SAVEPREVIEW") != null && RenderAnnotated() is byte[] previewPng)
                    System.IO.File.WriteAllBytes(System.IO.Path.Combine(FileSystem.CacheDirectory, "preview-" + Config.Title.ToLowerInvariant().Replace(' ', '-') + ".png"), previewPng);
                //TEMP-AUTOOPEN-END
            }
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            base.OnNavigatedFrom(args);
            if (!Navigation.NavigationStack.Contains(this) && !Navigation.ModalStack.Contains(this))
            {
                _image?.Dispose();
                _image = null;
                OnClosed();
            }
        }

        /// <summary>The page was closed for good: release models etc.</summary>
        protected virtual void OnClosed() { }

        // ================= Images =================

        private async Task LoadSample(int idx)
        {
            try
            {
                byte[] bytes = await MaskRcnnPage.ReadAppFileAsync(Config.Samples[idx].File);
                var m = new Mat();
                CvInvoke.Imdecode(bytes, ImreadModes.ColorBgr, m);
                if (m.IsEmpty) { m.Dispose(); return; }
                _sampleIndex = idx;
                SetImage(m, "Sample · " + Config.Samples[idx].Name);
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
            SetImage(MaskRcnnPage.Downscale(m, Config.MaxImageSize), name ?? "Your photo");
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
            _photo.Source = MaskRcnnPage.MatToImageSource(m);
            _sampleBadgeLabel.Text = badge;
            _sampleBadge.IsVisible = _wide;
            for (int i = 0; i < _thumbBorders.Count; i++)
                _thumbBorders[i].Stroke = i == _sampleIndex ? DemoTheme.PrimaryText : Colors.Transparent;
            _result = null;
            SetCompare(false);
            ShowResult(animate: false);
            PlaceBadges();
            SizeStage();
        }

        // ================= Run =================

        /// <summary>Run again (e.g. when an option changes on a generated demo).</summary>
        protected Task RunAsync() => Run();

        /// <summary>True while a run is in progress.</summary>
        protected bool IsBusy => _busy;

        private async Task Run()
        {
            if (_busy || (Config.Samples.Length > 0 && _image == null))
                return;
            _busy = true;
            _runButton.IsEnabled = false;
            _runButton.Text = "Working…";
            _stageSpinner.IsVisible = _stageSpinner.IsRunning = true;
            _stageEmpty.IsVisible = false;
            try
            {
                if (!await PrepareAsync())
                {
                    _emptyCaption.Text = "the model could not be downloaded or loaded";
                    return;
                }
                Mat input = _image;
                StageResult result = await Task.Run(() => RunCore(input));
                _result = result;
                SetCompare(false);
                if (result?.OutputPng != null)
                    _photo.Source = MaskRcnnPage.MatBytesToImageSource(result.OutputPng);
                else if (_image != null)
                    _photo.Source = MaskRcnnPage.MatToImageSource(_image);
                ShowResult(animate: true);
                PlaceBadges();
                SizeStage();
            }
            catch (Exception ex)
            {
                _result = null;
                ShowResult(animate: false);
                _emptyCaption.Text = "something went wrong: " + ex.Message;
            }
            finally
            {
                _busy = false;
                _runButton.IsEnabled = true;
                _runButton.Text = RunText;
                _stageSpinner.IsVisible = _stageSpinner.IsRunning = false;
                _stageEmpty.IsVisible = Config.Samples.Length == 0 && _result == null;
            }
        }

        private bool Passes(double score) => !Config.HasConfidence || score >= Confidence;

        private void ShowResult(bool animate)
        {
            if (!_built)
                return;
            var r = _result;
            bool ran = r != null;
            var boxes = ran ? r.Boxes.Where(b => Passes(b.Score)).ToArray() : Array.Empty<StageBox>();
            var rows = ran ? r.Rows.Where(x => Passes(x.Score)).ToArray() : Array.Empty<StageRow>();

            _drawable.ImageSize = StageImageSize;
            _drawable.Boxes = boxes.Select(b => new DetectionDrawable.Box(b.Rect, b.Label, b.Color)).ToArray();
            _overlay.IsVisible = !_comparing;
            if (animate && boxes.Length > 0)
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

            int count = Math.Max(boxes.Length, rows.Length);
            string countText = $"{count} {(count == 1 ? Config.ItemSingular : Config.ItemPlural)}";
            _resultBadge.IsVisible = ran;
            _resultBadgeLabel.Text = ran ? (r.Summary ?? $"{countText} · {r.InferenceMs:0} ms") : "";
            _resultsCount.Text = ran && count > 0 && Config.ShowResultCount ? countText : "";

            _resultsList.Children.Clear();
            foreach (var row in rows.Take(14))
                _resultsList.Children.Add(ResultRow(row));
            if (rows.Length > 14)
                _resultsList.Children.Add(new Label { Text = $"+ {rows.Length - 14} more", FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = DemoTheme.MutedText });
            _note.Text = r?.Note ?? "";
            _note.IsVisible = ran && !string.IsNullOrEmpty(r.Note);

            _emptyState.IsVisible = !ran || (rows.Length == 0 && !_note.IsVisible);
            _emptyCaption.Text = !ran ? Config.EmptyCaption
                : Config.HasConfidence ? "nothing above this confidence; try lowering it"
                : "nothing found in this image";

            if (ran)
            {
                _perfInference.Text = $"{r.InferenceMs:0} ms";
                _perfPrePost.Text = $"{r.PrePostMs:0} ms";
                _perfFps.Text = $"{1000.0 / Math.Max(1, r.InferenceMs + r.PrePostMs):0} fps";
            }
            else
            {
                _perfInference.Text = _perfPrePost.Text = _perfFps.Text = "—";
            }
            _compareButton.IsVisible = ran && (r.OutputPng != null || r.Boxes.Length > 0) && _image != null;
        }

        private static View ResultRow(StageRow row)
        {
            var grid = new Grid { ColumnSpacing = 10 };
            int col = 0;
            if (row.Color != null)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
                grid.Add(new BoxView { WidthRequest = 9, HeightRequest = 9, CornerRadius = 2.5, Color = row.Color, VerticalOptions = LayoutOptions.Center }, col++, 0);
            }
            bool bar = row.Fraction.HasValue;
            grid.ColumnDefinitions.Add(bar ? new ColumnDefinition(new GridLength(96)) : new ColumnDefinition(GridLength.Star));
            grid.Add(new Label { Text = row.Label, FontFamily = DemoTheme.TitleFont, FontSize = 14, TextColor = DemoTheme.PrimaryText, LineBreakMode = bar ? LineBreakMode.TailTruncation : LineBreakMode.WordWrap, VerticalOptions = LayoutOptions.Center }, col++, 0);
            if (bar)
            {
                double f = Math.Clamp(row.Fraction.Value, 0, 1);
                var track = new Grid
                {
                    HeightRequest = 4,
                    VerticalOptions = LayoutOptions.Center,
                    ColumnDefinitions = { new ColumnDefinition(new GridLength(Math.Max(f, 0.001), GridUnitType.Star)), new ColumnDefinition(new GridLength(Math.Max(1 - f, 0.001), GridUnitType.Star)) }
                };
                track.Add(new BoxView { Color = row.Color ?? DemoTheme.PrimaryText, CornerRadius = 2 }, 0, 0);
                grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                grid.Add(track, col++, 0);
            }
            if (row.Detail != null)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
                grid.Add(new Label { Text = row.Detail, FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center }, col++, 0);
            }
            return grid;
        }

        /// <summary>Score row helper: swatch, label, confidence bar and score.</summary>
        protected static StageRow ScoreRow(string label, double score, Color color) =>
            new StageRow(label, score.ToString("0.00"), score, color, score);

        /// <summary>Per-class box color (lime, sky, pink, …).</summary>
        protected static Color ClassColor(int classId) => DemoTheme.BoxColors[Math.Abs(classId) % DemoTheme.BoxColors.Length];

        // ================= Toolbar actions =================

        private void ToggleCompare() => SetCompare(!_comparing);

        // Compare: for boxes, hide them; for an output image, show a draggable
        // before / after split (input on the left, output on the right).
        private void SetCompare(bool on)
        {
            _comparing = on && _result != null;
            _compareButton.BackgroundColor = _comparing ? DemoTheme.PillBackground : Colors.Transparent;
            _overlay.IsVisible = !_comparing;
            bool split = _comparing && _result?.OutputPng != null && _image != null;
            _compareLabel.Text = _comparing ? (split ? "Drag to compare" : "Show results") : "Compare";
            _photoBefore.IsVisible = split;
            _splitHandle.IsVisible = split;
            _beforeBadge.IsVisible = _afterBadge.IsVisible = split;
            _resultBadge.IsVisible = !split && _result != null;
            _sampleBadge.IsVisible = !split && _wide && _sampleBadgeLabel.Text?.Length > 0;
            if (split)
            {
                _photoBefore.Source = MaskRcnnPage.MatToImageSource(_image);
                _split = 0.5;
            }
            UpdateSplit();
        }

        private void UpdateSplit()
        {
            if (_photo == null)
                return;
            if (!_photoBefore.IsVisible)
            {
                _photo.Clip = null;
                return;
            }
            var fit = FitRect();
            double x = fit.X + fit.Width * _split;
            _photo.Clip = new RectangleGeometry(new Rect(x, 0, Math.Max(0, _photo.Width - x), Math.Max(1, _photo.Height)));
            _splitHandle.TranslationX = 16 + x - 19;
        }

        private double _panStartX;
        private void OnSplitPan(object sender, PanUpdatedEventArgs e)
        {
            if (!_photoBefore.IsVisible)
                return;
            var fit = FitRect();
            if (fit.Width <= 0)
                return;
            if (e.StatusType == GestureStatus.Started)
                _panStartX = fit.X + fit.Width * _split;
            else if (e.StatusType == GestureStatus.Running)
            {
                _split = Math.Clamp((_panStartX + e.TotalX - fit.X) / fit.Width, 0, 1);
                UpdateSplit();
            }
        }

        // What's on the stage, with the visible boxes drawn in, as PNG bytes.
        private byte[] RenderAnnotated()
        {
            if (_result?.OutputPng != null && _result.Boxes.Length == 0)
                return _result.OutputPng;
            if (_image == null && _result?.OutputPng == null)
                return null;
            // Start from what's on the stage: the output image if there is one
            // (e.g. masks), otherwise the input photo.
            using var copy = new Mat();
            if (_result?.OutputPng != null)
                CvInvoke.Imdecode(_result.OutputPng, ImreadModes.ColorBgr, copy);
            else
                _image.CopyTo(copy);
            if (_result != null)
            {
                foreach (var b in _result.Boxes.Where(x => Passes(x.Score)))
                {
                    var c = b.Color;
                    var bgr = new MCvScalar(c.Blue * 255, c.Green * 255, c.Red * 255);
                    var rect = new System.Drawing.Rectangle((int)b.Rect.X, (int)b.Rect.Y, (int)b.Rect.Width, (int)b.Rect.Height);
                    CvInvoke.Rectangle(copy, rect, bgr, 2);
                    CvInvoke.PutText(copy, b.Label, new System.Drawing.Point(rect.X, Math.Max(12, rect.Y - 4)), HersheyFonts.Simplex, 0.5, bgr, 1);
                }
            }
            return MaskRcnnPage.Encode(copy);
        }

        private async void OpenFullscreen()
        {
            byte[] png = RenderAnnotated();
            if (png != null)
                await Navigation.PushModalAsync(new FullScreenImagePage(png, Config.Title));
        }

        private async Task ShareResult()
        {
            byte[] png = RenderAnnotated();
            if (png == null)
                return;
            string path = System.IO.Path.Combine(FileSystem.CacheDirectory, "emgu-result.png");
            await System.IO.File.WriteAllBytesAsync(path, png);
            await Share.Default.RequestAsync(new ShareFileRequest { Title = Config.Title + " result", File = new ShareFile(path) });
        }

        // ================= Sheets =================

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
                    new Label { Text = "The core of what this page runs, with your current settings.", FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText },
                    codeBox
                }
            });
        }

        private void ShowInfoSheet()
        {
            var head = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            head.Add(new Label { Text = "about " + Config.Title.ToLowerInvariant(), FontFamily = DemoTheme.HeroFont, FontSize = 26, CharacterSpacing = -0.8, TextColor = DemoTheme.PrimaryText, VerticalOptions = LayoutOptions.Center }, 0, 0);
            head.Add(DemoControls.CircleButton(DemoTheme.GlyphClose, CloseSheet, 34), 1, 0);
            var stack = new VerticalStackLayout { Spacing = 12, Children = { head } };
            foreach (string para in (Config.About ?? Config.Description ?? "").Split("\n\n"))
                stack.Children.Add(new Label { Text = para, FontFamily = DemoTheme.BodyFont, FontSize = 15, TextColor = DemoTheme.SecondaryText });
            stack.Children.Add(new Label { Text = "Everything runs on this device; models download once, the first time you use them.", FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.MutedText });
            OpenSheet(stack);
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
                // No room above (top of the photo, where the result badge sits too):
                // put the chip at the bottom of the box instead.
                if (cy < oy + 34) cy = r.Bottom - ch - 3;
                canvas.FillColor = b.Color;
                canvas.FillRoundedRectangle(cx, cy, cw, ch, 5);
                canvas.FontColor = Color.FromArgb("#0E0E0E");
                canvas.DrawString(b.Label, cx, cy, cw, ch, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
        }
    }
}
