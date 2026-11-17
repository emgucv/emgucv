//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Shapes;

namespace MauiDemoApp
{
    /// <summary>
    /// A reusable, on-theme chat page for the local language-model demos. It gives
    /// Qwen Chat and Chat with Image the same two-pane layout the image demos get
    /// from <see cref="StagePage"/>: a header with a giant lowercase title, the
    /// conversation and composer in the main area, and a settings panel (model,
    /// performance, the C# behind it) on the right. Phones get one column with the
    /// composer pinned to the bottom.
    ///
    /// The page owns everything that is identical between the two demos — header,
    /// model section, transcript, composer, busy and download state — and leaves each
    /// module to supply its models and the actual generation call.
    /// </summary>
    public abstract class ChatShowcasePage : ContentPage
    {
        /// <summary>One selectable model in the model card.</summary>
        public sealed class ChatModel
        {
            /// <summary>Display name, e.g. "Qwen3 0.6B".</summary>
            public string Name { get; }

            /// <summary>One-line detail, e.g. "2.8 GB • Fastest (~150 ms/token)".</summary>
            public string Detail { get; }

            /// <summary>Subfolder under the local "emgu" model folder.</summary>
            public string Folder { get; }

            /// <summary>
            /// Files that must be present for the model to count as installed, with
            /// the exact byte size of each. A size of 0 means "unknown, any
            /// non-empty file will do".
            /// </summary>
            public (string Name, long Size)[] Files { get; }

            /// <summary>Whether this is the module's default selection.</summary>
            public bool IsDefault { get; }

            /// <summary>Module-specific payload handed back on selection.</summary>
            public object Tag { get; }

            /// <summary>
            /// Roughly how much has to be resident to run the model — in practice
            /// the total weight size. Used to rule out models this device cannot
            /// hold. 0 means unknown, which is treated as "allowed".
            /// </summary>
            public long WeightBytes { get; }

            public ChatModel(string name, string detail, string folder, (string Name, long Size)[] files, bool isDefault = false, object tag = null, long weightBytes = 0)
            {
                Name = name;
                Detail = detail;
                Folder = folder;
                WeightBytes = weightBytes;
                Files = files ?? Array.Empty<(string, long)>();
                IsDefault = isDefault;
                Tag = tag;
            }
        }

        // Bubble palette. The user's turn is filled with the text color and set in
        // the page color, like the app's primary buttons; the model's turn sits on the soft grey pill fill,
        // so a long answer never becomes a wall of black.
        private static Color UserBubble => DemoTheme.Accent;
        private static Color UserBubbleText => DemoTheme.OnAccent;
        private static Color ModelBubble => DemoTheme.PillBackground;
        private static Color ModelBubbleText => DemoTheme.PrimaryText;
        // Red is kept only where it carries meaning: a model this device can't
        // run, and the destructive delete action.
        private static Color BlockedText => Color.FromArgb(DemoTheme.IsDark ? "#F08A82" : "#C2453D");
        private static Color BlockedFill => Color.FromArgb(DemoTheme.IsDark ? "#3A1E1D" : "#FBEAE9");

        private const int MaxPromptLength = 4000;

        // Both already in the subset icon font; MaskRcnnPage doesn't declare them,
        // so keep local copies rather than widen that page's surface. Written as
        // escapes rather than literal private-use characters, which do not survive
        // every editor and tool that touches this file.
        private const string GlyphExpandMore = ""; // expand_more
        // auto_awesome. Used for the model chip and the model's chat avatar so they
        // read as "the model" rather than repeating the page's own title glyph.
        private const string GlyphModel = "";

        // Same breakpoint and panel width as StagePage, so the chat pages switch
        // between the two-pane and one-column layouts at the same window width as
        // the image demos.
        private const double PanelWidth = 380;
        private const double WideBreakpoint = 900;

        private readonly ChatModel[] _models;
        private readonly string _composerPlaceholder;
        private readonly string _title;
        private readonly string _description;
        private readonly string _tag;
        private readonly string _category;

        private int _selectedIndex;
        private bool _expanded;
        private bool _busy;
        private Task _generation;
        private readonly string _moduleKey;
        private bool _rowsShowDownloading;
        private bool _wide, _layoutBuilt;

        // What was said, in order, for the share button.
        private readonly List<(string Speaker, string Text)> _messages = new List<(string, string)>();

        // Wall-clock seconds of each answer (a Stopwatch around Generate only, not
        // the download or the model load), for the performance tiles.
        private readonly List<double> _replySeconds = new List<double>();

        // ---------- Runtime UI ----------
        // Built once in the constructor, then re-arranged between the wide
        // (two-pane) and phone (one-column) layouts on SizeChanged, the same way
        // StagePage does it.
        private readonly Grid _root = new Grid();
        private readonly View _headerWide, _headerPhone;
        private readonly View _modelSection;
        private readonly View _perfSection;
        private readonly View _codeLink;
        private readonly View _extra;
        private readonly Grid _chatBody;
        private readonly VerticalStackLayout _transcript;
        private readonly VerticalStackLayout _emptyState;
        private readonly View _typingRow;
        private readonly VerticalStackLayout _modelRows;
        private readonly Label _collapsedName;
        private readonly Label _collapsedDetail;
        private readonly Border _collapsedBadge;
        private readonly Label _collapsedBadgeLabel;
        private readonly Image _chevron;
        private readonly Border _chevronDisc;
        private readonly BoxView _modelStateDot;
        private readonly Label _modelStateLabel;
        private readonly Editor _promptEditor;
        private readonly Label _counterLabel;
        private readonly Border _sendButton;
        private readonly Image _sendIcon;
        private readonly ActivityIndicator _sendSpinner;
        private readonly Grid _statusRow;
        private readonly BoxView _statusDot;
        private readonly Label _statusLabel;
        private readonly VerticalStackLayout _composerArea;
        private readonly Label _perfLast, _perfAverage, _perfCount;
        private readonly List<Border> _perfTiles = new List<Border>();
        // The conversation's own scroller on desktop; on the phone the whole
        // column scrolls instead (_pageScroll).
        private readonly ScrollView _chatScroll = new ScrollView();
        private ScrollView _pageScroll;
        private VerticalStackLayout _phoneDetails;
        private readonly Grid _sheetOverlay;
        private readonly Border _sheetCard;
        private readonly BoxView _sheetScrim;

        /// <summary>The model the user currently has selected.</summary>
        protected ChatModel SelectedModel => _models.Length > 0 ? _models[_selectedIndex] : null;

        /// <summary>
        /// Create a chat page.
        /// </summary>
        /// <param name="title">Page title, shown as the giant lowercase header.</param>
        /// <param name="heroSubtitle">One-line description under the title.</param>
        /// <param name="glyph">Material Symbols glyph identifying the module.</param>
        /// <param name="models">Selectable models. Pass a single entry for a module with no choice.</param>
        /// <param name="actionText">Caption for the action pill in the model section (e.g. "New Chat"), or null for none.</param>
        /// <param name="composerPlaceholder">Placeholder text for the message box.</param>
        /// <param name="tag">Short tag pill in the header, e.g. "LLM".</param>
        /// <param name="category">Grey category text beside the tag.</param>
        protected ChatShowcasePage(
            string title,
            string heroSubtitle,
            string glyph,
            ChatModel[] models,
            string actionText,
            string composerPlaceholder,
            string tag = null,
            string category = null)
        {
            _models = models ?? Array.Empty<ChatModel>();
            _composerPlaceholder = composerPlaceholder;
            HeroGlyph = glyph;
            _moduleKey = title;
            _title = title;
            _description = heroSubtitle;
            _tag = tag;
            _category = category;

            // Prefer the module's default, but never start on a model this device
            // cannot load: that state looks completely normal and then kills the
            // app on the first send. Fall back to the first one that does fit, and
            // only keep a blocked default if nothing fits at all (so the page still
            // explains itself rather than showing an empty selection).
            _selectedIndex = -1;
            for (int i = 0; i < _models.Length; i++)
                if (_models[i].IsDefault && !ExceedsDeviceMemory(_models[i]))
                    _selectedIndex = i;
            if (_selectedIndex < 0)
                for (int i = 0; i < _models.Length && _selectedIndex < 0; i++)
                    if (!ExceedsDeviceMemory(_models[i]))
                        _selectedIndex = i;
            if (_selectedIndex < 0)
                _selectedIndex = 0;

            Title = title;
            BackgroundColor = DemoTheme.PageBackground;
            Shell.SetNavBarIsVisible(this, false);
            Content = _root;
            SizeChanged += (s, e) => BuildLayout();

            // Cmd+Return sends on the Mac (the menu bar carries the shortcut), like
            // Cmd+Return runs the image demos.
            var sendItem = new MenuFlyoutItem { Text = "Send Message" };
            sendItem.KeyboardAccelerators.Add(new KeyboardAccelerator { Modifiers = KeyboardAcceleratorModifiers.Cmd, Key = "\r" });
            sendItem.Clicked += async (s, e) => await OnSend();
            var menu = new MenuBarItem { Text = "Demo" };
            menu.Add(sendItem);
            MenuBarItems.Add(menu);

            // ---------- Header ----------
            _headerWide = BuildHeader(wide: true);
            _headerPhone = BuildHeader(wide: false);
            _headerWide.SizeChanged += (s, e) => SizeAreas();

            // ---------- Model ----------
            // Collapsed, this is a single flat summary row; expanded, it reveals the
            // full radio list plus a note about what the choice costs.
            // Truncate rather than wrap: the badge beside these sizes to its own
            // content, so a long one (e.g. "Too big") would otherwise squeeze the
            // name into a one-word-per-line tower.
            _collapsedName = new Label { FontFamily = DemoTheme.TitleFont, FontSize = 16, TextColor = DemoTheme.PrimaryText, LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 1 };
            _collapsedDetail = new Label { FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText, LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 1 };
            _collapsedBadgeLabel = new Label { FontFamily = DemoTheme.BodyFont, FontSize = 12, CharacterSpacing = 0.5, VerticalOptions = LayoutOptions.Center };
            _collapsedBadge = new Border
            {
                BackgroundColor = DemoTheme.PillBackground,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                Padding = new Thickness(12, 5),
                VerticalOptions = LayoutOptions.Center,
                Content = _collapsedBadgeLabel
            };
            _chevron = DemoTheme.MakeIcon(GlyphExpandMore, DemoTheme.PrimaryText, 22);
            _chevron.VerticalOptions = LayoutOptions.Center;

            var summaryText = new VerticalStackLayout
            {
                Spacing = 2,
                VerticalOptions = LayoutOptions.Center,
                Children = { _collapsedName, _collapsedDetail }
            };

            // A soft disc behind the chevron so the row reads as something you can
            // open, rather than a bare status line.
            _chevronDisc = new Border
            {
                WidthRequest = 30,
                HeightRequest = 30,
                BackgroundColor = DemoTheme.PillBackground,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
                VerticalOptions = LayoutOptions.Center,
                Content = _chevron
            };

            // Badge and chevron travel together, hard against the right edge, so
            // the free space collects between the model name and the badge instead
            // of leaving a gap outside them.
            var summaryTrailing = new HorizontalStackLayout
            {
                Spacing = 8,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Children = { _collapsedBadge, _chevronDisc }
            };

            var summaryRow = new Grid
            {
                ColumnSpacing = 12,
                BackgroundColor = Colors.Transparent,
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                }
            };
            summaryRow.Add(summaryText, 0, 0);
            summaryRow.Add(summaryTrailing, 1, 0);

            _modelRows = new VerticalStackLayout { Spacing = 0, IsVisible = false };

            // The expander opens even for a single model. There is nothing to
            // choose between, but the row behind it is where the size, the
            // installed state, the delete action and any "too big for this device"
            // explanation live — hiding all of that just because a module ships one
            // model left the vision page looking like a different, emptier app.
            if (_models.Length > 0)
            {
                var expandTap = new TapGestureRecognizer();
                expandTap.Tapped += async (s, e) => await ToggleModelList();
                summaryRow.GestureRecognizers.Add(expandTap);
            }
            else
            {
                _chevronDisc.IsVisible = false;
                summaryRow.IsVisible = false;
            }

            // Download / loaded state of the selected model: dot + one line.
            _modelStateDot = new BoxView { WidthRequest = 6, HeightRequest = 6, CornerRadius = 3, Color = DemoTheme.StatusGreen, VerticalOptions = LayoutOptions.Start, Margin = new Thickness(0, 5, 0, 0) };
            _modelStateLabel = new Label { FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = DemoTheme.SecondaryText, LineBreakMode = LineBreakMode.WordWrap };
            var modelState = new Grid { ColumnSpacing = 7, IsVisible = _models.Length > 0, ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) } };
            modelState.Add(_modelStateDot, 0, 0);
            modelState.Add(_modelStateLabel, 1, 0);

            // The module's action (e.g. NEW CHAT) sits on the right of the section
            // label. A module whose action belongs next to its own content (e.g.
            // "Change" on the image) passes no caption and places
            // BuildActionPill() itself.
            var modelHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            modelHeader.Add(DemoControls.SectionLabel("Model"), 0, 0);
            if (!string.IsNullOrEmpty(actionText))
                modelHeader.Add(BuildActionPill(actionText), 1, 0);

            _modelSection = new VerticalStackLayout
            {
                Spacing = 12,
                Children = { modelHeader, summaryRow, modelState, _modelRows }
            };

            // ---------- Conversation ----------
            // Empty state: oversized pale type instead of an icon, as on the
            // image demos.
            _emptyState = new VerticalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    new Label
                    {
                        Text = "no messages yet",
                        FontFamily = DemoTheme.HeroFont,
                        FontSize = 40,
                        CharacterSpacing = -1.6,
                        LineHeight = 0.95,
                        TextColor = DemoTheme.EmptyStateText,
                        // Negative spacing measures narrower than it draws.
                        Padding = new Thickness(0, 0, 6, 0)
                    },
                    new Label { Text = EmptyStateHint, FontFamily = DemoTheme.BodyFont, FontSize = 14, TextColor = DemoTheme.SecondaryText }
                }
            };
            _transcript = new VerticalStackLayout { Spacing = 14 };
            // Both share one cell: the empty state shows only while the transcript
            // has nothing in it.
            _chatBody = new Grid { Children = { _emptyState, _transcript } };

            // "Thinking" bubble shown on the model's side while it generates.
            var typingSpinner = new ActivityIndicator { Color = DemoTheme.SecondaryText, IsRunning = true, WidthRequest = 16, HeightRequest = 16, VerticalOptions = LayoutOptions.Center };
            var typingBubble = new Border
            {
                BackgroundColor = ModelBubble,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                Padding = new Thickness(14, 11),
                HorizontalOptions = LayoutOptions.Start,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(18, 18, 6, 18) },
                Content = new HorizontalStackLayout
                {
                    Spacing = 8,
                    Children =
                    {
                        typingSpinner,
                        new Label { Text = "thinking…", FontFamily = DemoTheme.BodyFont, FontSize = 14, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center }
                    }
                }
            };
            var typingGrid = new Grid
            {
                ColumnSpacing = 9,
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) }
            };
            typingGrid.Add(Avatar(), 0, 0);
            typingGrid.Add(typingBubble, 1, 0);
            _typingRow = typingGrid;

            // ---------- Status line (just above the composer) ----------
            _statusDot = new BoxView { WidthRequest = 6, HeightRequest = 6, CornerRadius = 3, Color = DemoTheme.StatusGreen, VerticalOptions = LayoutOptions.Start, Margin = new Thickness(0, 5, 0, 0) };
            _statusLabel = new Label { FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = DemoTheme.SecondaryText, LineBreakMode = LineBreakMode.WordWrap, MaxLines = 2 };
            _statusRow = new Grid
            {
                ColumnSpacing = 7,
                Padding = new Thickness(6, 0),
                IsVisible = false,
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) }
            };
            _statusRow.Add(_statusDot, 0, 0);
            _statusRow.Add(_statusLabel, 1, 0);

            // ---------- Composer ----------
            _promptEditor = new Editor
            {
                Placeholder = _composerPlaceholder,
                PlaceholderColor = DemoTheme.MutedText,
                BackgroundColor = Colors.Transparent,
                TextColor = DemoTheme.PrimaryText,
                FontFamily = DemoTheme.BodyFont,
                FontSize = 15,
                HeightRequest = 76,
                AutoSize = EditorAutoSizeOption.Disabled
            };
            _promptEditor.TextChanged += (s, e) => UpdateCounter();

            // On iOS/MacCatalyst, dismissing the keyboard via the "Done" accessory
            // can leave the page ScrollView with the keyboard inset still applied,
            // which freezes scrolling until the editor is focused again. Clearing
            // the inset and re-enabling scrolling when the editor loses focus works
            // around it.
            _promptEditor.Unfocused += (s, e) => ResetScrollAfterKeyboard();

            _counterLabel = new Label
            {
                Text = SendHint + "0 / " + MaxPromptLength,
                FontFamily = DemoTheme.BodyFont,
                FontSize = 11,
                TextColor = DemoTheme.MutedText,
                Margin = new Thickness(4, 0, 0, 2)
            };

            _sendIcon = DemoTheme.MakeIcon(DemoTheme.GlyphPlay, DemoTheme.OnAccent, 22);
            _sendSpinner = new ActivityIndicator { Color = DemoTheme.OnAccent, WidthRequest = 20, HeightRequest = 20, IsRunning = false, IsVisible = false };
            _sendButton = new Border
            {
                WidthRequest = 44,
                HeightRequest = 44,
                BackgroundColor = DemoTheme.Accent,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                Content = new Grid { Children = { _sendIcon, _sendSpinner } }
            };
            var sendTap = new TapGestureRecognizer();
            sendTap.Tapped += async (s, e) => await OnSend();
            _sendButton.GestureRecognizers.Add(sendTap);
            DemoControls.AddHover(_sendButton);

            var composerGrid = new Grid
            {
                ColumnSpacing = 10,
                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                RowDefinitions = { new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Auto) }
            };
            composerGrid.Add(_promptEditor, 0, 0);
            composerGrid.Add(_counterLabel, 0, 1);
            composerGrid.Add(_sendButton, 1, 0);
            Grid.SetRowSpan(_sendButton, 2);

            // Flat rounded well: no outline, no shadow.
            var composerCard = new Border
            {
                BackgroundColor = DemoTheme.ImageBackground,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                Padding = new Thickness(12, 4, 8, 8),
                Content = composerGrid
            };

            _composerArea = new VerticalStackLayout { Spacing = 8, Children = { _statusRow, composerCard } };
            _composerArea.SizeChanged += (s, e) => SizeAreas();

            // ---------- Performance ----------
            var perfHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            perfHeader.Add(DemoControls.SectionLabel("Performance"), 0, 0);
            perfHeader.Add(new Label { Text = "OpenCV DNN · on device", FontFamily = DemoTheme.TitleFont, FontSize = 12, TextColor = DemoTheme.SecondaryText, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center }, 1, 0);
            var perfRow = new Grid { ColumnSpacing = 8, ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) } };
            (var t1, _perfLast) = PerfTile("last reply");
            (var t2, _perfAverage) = PerfTile("average");
            (var t3, _perfCount) = PerfTile("replies");
            perfRow.Add(t1, 0, 0);
            perfRow.Add(t2, 1, 0);
            perfRow.Add(t3, 2, 0);
            _perfSection = new VerticalStackLayout
            {
                Spacing = 12,
                Children =
                {
                    perfHeader,
                    perfRow,
                    new Label { Text = "Time from send to answer for this model, measured on this device. Download and loading are not counted.", FontFamily = DemoTheme.BodyFont, FontSize = 12, TextColor = DemoTheme.MutedText }
                }
            };

            // ---------- The C# behind this ----------
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

            // Modules with extra content (e.g. the image being discussed) put it in
            // the main area, above the conversation.
            _extra = BuildExtraCard();

            BuildModelRows();
            UpdateModelSummary();
            RefreshPerf();
        }

        // ---------- Hooks for the concrete modules ----------

        /// <summary>Hint shown under "no messages yet".</summary>
        protected virtual string EmptyStateHint => "Ask a question to get started.";

        /// <summary>
        /// Optional view shown in the main area above the conversation (e.g. the
        /// image being discussed). The page sets its height for each layout.
        /// </summary>
        protected virtual View BuildExtraCard() => null;

        /// <summary>Whether the selected model is loaded in memory right now.</summary>
        protected virtual bool IsModelLoaded => false;

        /// <summary>The short C# snippet behind this demo, built on demand. Null hides the link.</summary>
        protected virtual string CodeSnippet => null;

        /// <summary>Paragraph(s) for the info sheet; defaults to the description.</summary>
        protected virtual string AboutText => null;

        /// <summary>
        /// Make sure the selected model is loaded, downloading it if needed. Report
        /// progress with <see cref="SetStatus"/>. Return false to abort the turn.
        /// </summary>
        protected abstract Task<bool> EnsureModelReadyAsync();

        /// <summary>Produce the answer for a prompt. Called on a background thread.</summary>
        protected abstract string Generate(string prompt);

        /// <summary>Called when the user picks a different model.</summary>
        protected virtual void OnModelChanged(ChatModel model) { }

        /// <summary>Called when the action pill is tapped.</summary>
        protected virtual void OnHeaderAction(object sender, EventArgs e) { }

        /// <summary>
        /// Build the module's action button as a small uppercase pill, guarded so
        /// it cannot fire mid-turn: "New Chat" resets the model's conversation
        /// state and "Change" swaps the image out, and the background generation
        /// is still using both. Letting either through corrupts the answer at best
        /// and crashes inside native code at worst.
        /// </summary>
        protected View BuildActionPill(string text)
        {
            // Page-colored with a hairline, so it reads both on the page and on the
            // grey panel well.
            var pill = new Border
            {
                BackgroundColor = DemoTheme.PageBackground,
                Stroke = DemoTheme.Hairline,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(100) },
                Padding = new Thickness(14, 7),
                VerticalOptions = LayoutOptions.Center,
                Content = new Label { Text = text.ToUpperInvariant(), FontFamily = DemoTheme.TitleFont, FontSize = 11, CharacterSpacing = 1.4, TextColor = DemoTheme.PrimaryText }
            };
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) =>
            {
                if (_busy)
                {
                    SetStatus("Hang on — still answering.");
                    return;
                }
                OnHeaderAction(this, EventArgs.Empty);
            };
            pill.GestureRecognizers.Add(tap);
            DemoControls.AddHover(pill);
            return pill;
        }

        /// <summary>
        /// Whether a turn is currently being generated. Deliberately not named
        /// IsBusy — that is an existing Page property driving the platform activity
        /// indicator, and shadowing it invites setting the wrong one.
        /// </summary>
        protected bool IsGenerating => _busy;

        /// <summary>
        /// Dispose native state once any in-flight generation has finished.
        ///
        /// <see cref="Generate"/> runs on a background thread and holds the model
        /// (and, for the vision module, the image) for its whole run. Freeing those
        /// while it is still reading them crashes the process inside native code,
        /// so teardown has to wait rather than race. The continuation runs off the
        /// UI thread, which is fine — these are plain native handles.
        /// </summary>
        protected void CleanupWhenIdle(Action cleanup)
        {
            Task pending = _generation;
            if (pending == null || pending.IsCompleted)
            {
                cleanup();
                return;
            }
            pending.ContinueWith(_ => cleanup());
        }

        /// <summary>Whether the page is ready to accept a prompt (e.g. an image is loaded).</summary>
        protected virtual bool CanSend(out string reason)
        {
            reason = null;
            return true;
        }

        private static bool IsDesktop => DeviceInfo.Idiom == DeviceIdiom.Desktop;

        private static string SendHint => IsDesktop ? "⌘↵ to send · " : string.Empty;

        // ================= Layout =================

        private void BuildLayout()
        {
            if (Width <= 0)
                return;
            bool wide = Width >= WideBreakpoint;
            if (_layoutBuilt && wide == _wide)
            {
                SizeAreas();
                return;
            }

            // The panel has room for the whole model list, so open it the first
            // time the page comes up wide. On the phone it stays collapsed.
            if (!_layoutBuilt && wide && _models.Length > 0)
            {
                _expanded = true;
                _modelRows.IsVisible = true;
                _chevron.Rotation = 180;
            }

            _wide = wide;
            _layoutBuilt = true;

            foreach (var v in new View[] { _headerWide, _headerPhone, _modelSection, _perfSection, _codeLink, _extra, _chatBody, _chatScroll, _composerArea, _sheetOverlay })
                Detach(v);
            _root.Children.Clear();
            _root.RowDefinitions.Clear();

            // Perf tiles sit on the grey well on desktop and on the page on phones.
            foreach (Border tile in _perfTiles)
                tile.BackgroundColor = wide ? DemoTheme.PageBackground : DemoTheme.ImageBackground;

            if (wide)
            {
                // ---------- Panel (right) ----------
                var panelStack = new VerticalStackLayout { Spacing = 20, Children = { _modelSection, DemoControls.Hairline(), _perfSection } };
                if (_codeLink != null)
                {
                    panelStack.Children.Add(DemoControls.Hairline());
                    panelStack.Children.Add(_codeLink);
                }
                var panel = DemoControls.Well(panelStack, 22, new Thickness(20));
                panel.VerticalOptions = LayoutOptions.Start;

                // ---------- Main area (left): the conversation card ----------
                // A page-colored card with a hairline rather than a grey well, so the
                // model's grey bubbles keep their contrast.
                _chatBody.Padding = new Thickness(20, 8, 20, 20);
                _emptyState.VerticalOptions = LayoutOptions.Center;
                _chatScroll.Content = _chatBody;
                _promptEditor.HeightRequest = 76;

                var cardHead = new Grid { Padding = new Thickness(20, 16, 20, 4) };
                cardHead.Add(DemoControls.SectionLabel("Conversation"));
                var composerZone = new Grid { Padding = new Thickness(16) };
                composerZone.Add(_composerArea);

                var cardGrid = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition(GridLength.Auto),
                        new RowDefinition(GridLength.Auto),
                        new RowDefinition(GridLength.Auto),
                        new RowDefinition(GridLength.Auto)
                    }
                };
                cardGrid.Add(cardHead, 0, 0);
                cardGrid.Add(_chatScroll, 0, 1);
                cardGrid.Add(DemoControls.Hairline(), 0, 2);
                cardGrid.Add(composerZone, 0, 3);

                var chatCard = new Border
                {
                    BackgroundColor = DemoTheme.PageBackground,
                    Stroke = DemoTheme.Hairline,
                    StrokeThickness = 1,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                    Content = cardGrid
                };

                var main = new VerticalStackLayout { Spacing = 14 };
                if (_extra != null)
                    main.Children.Add(_extra);
                main.Children.Add(chatCard);

                var body = new Grid { ColumnSpacing = 20, ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(new GridLength(PanelWidth)) } };
                body.Add(main, 0, 0);
                body.Add(panel, 1, 0);

                var page = new VerticalStackLayout { Padding = new Thickness(32, 22, 32, 32), Spacing = 22, Children = { _headerWide, body } };
                _pageScroll = new ScrollView { Content = page };
                _root.Add(_pageScroll);
            }
            else
            {
                // ---------- Phone: one column, composer pinned to the bottom ----------
                _chatBody.Padding = new Thickness(0);
                _emptyState.VerticalOptions = LayoutOptions.Start;
                _promptEditor.HeightRequest = 56;

                var column = new VerticalStackLayout { Padding = new Thickness(16, 10, 16, 24), Spacing = 18, Children = { _headerPhone, _modelSection } };

                // Performance and the code link fold away with the model list, so
                // the conversation stays near the top on a small screen.
                _phoneDetails = new VerticalStackLayout { Spacing = 18, IsVisible = _expanded, Children = { DemoControls.Hairline(), _perfSection } };
                if (_codeLink != null)
                {
                    _phoneDetails.Children.Add(DemoControls.Hairline());
                    _phoneDetails.Children.Add(_codeLink);
                }
                column.Children.Add(_phoneDetails);
                column.Children.Add(DemoControls.Hairline());
                if (_extra != null)
                    column.Children.Add(_extra);
                column.Children.Add(new VerticalStackLayout { Spacing = 14, Children = { DemoControls.SectionLabel("Conversation"), _chatBody } });

                var composerZone = new Grid { Padding = new Thickness(16, 10, 16, 10) };
                composerZone.Add(_composerArea);
                var bottomBar = new VerticalStackLayout { BackgroundColor = DemoTheme.PageBackground, Children = { DemoControls.Hairline(), composerZone } };

                _pageScroll = new ScrollView { Content = column };
                _root.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                _root.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                _root.Add(_pageScroll, 0, 0);
                _root.Add(bottomBar, 0, 1);
            }

            _root.Add(_sheetOverlay);
            if (_root.RowDefinitions.Count > 1)
                Grid.SetRowSpan(_sheetOverlay, 2);
            SizeAreas();
        }

        /// <summary>
        /// Size the parts that depend on the window: the image above the
        /// conversation, and on desktop the conversation's own scroll area, which
        /// fills what is left of the window under the header and over the composer.
        /// </summary>
        private void SizeAreas()
        {
            if (!_layoutBuilt || Height <= 0)
                return;
            if (_wide)
            {
                double extraH = 0;
                if (_extra != null)
                {
                    extraH = Math.Clamp(Height * 0.28, 150, 320);
                    _extra.HeightRequest = extraH;
                }
                double headerH = _headerWide.Height > 0 ? _headerWide.Height : 170;
                double composerH = _composerArea.Height > 0 ? _composerArea.Height : 110;
                // Page padding (22 + 32) and spacing (22), the gap under the image
                // (14), the card's head row (~36), hairline and composer padding
                // (1 + 32), and the card's outline.
                double chrome = 22 + headerH + 22 + (_extra != null ? extraH + 14 : 0) + 36 + 1 + composerH + 32 + 32 + 2;
                double h = Math.Max(220, Height - chrome);
                if (Math.Abs(_chatScroll.HeightRequest - h) > 0.5)
                    _chatScroll.HeightRequest = h;
                _chatBody.MinimumHeightRequest = Math.Max(0, h - 28);
            }
            else
            {
                if (_extra != null)
                    _extra.HeightRequest = Math.Clamp(Width * 0.62, 200, 360);
                _chatBody.MinimumHeightRequest = -1;
            }
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
            var share = DemoControls.CircleButton(DemoTheme.GlyphShare, async () => await ShareConversation());
            var info = DemoControls.CircleButton(DemoTheme.GlyphInfo, ShowInfoSheet);
            var right = new HorizontalStackLayout { Spacing = 8, HorizontalOptions = LayoutOptions.End, Children = { share, info } };

            var topRow = new Grid { ColumnSpacing = 12, ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            topRow.Add(back, 0, 0);
            if (wide)
            {
                var crumbs = new HorizontalStackLayout { Spacing = 8, VerticalOptions = LayoutOptions.Center };
                crumbs.Children.Add(Crumb("Demos", false, async () => await Navigation.PopAsync()));
                if (!string.IsNullOrEmpty(_category))
                {
                    crumbs.Children.Add(Crumb("/", false, null));
                    crumbs.Children.Add(Crumb(_category, false, async () => await Navigation.PopAsync()));
                }
                crumbs.Children.Add(Crumb("/", false, null));
                crumbs.Children.Add(Crumb(_title.ToLowerInvariant(), true, null));
                topRow.Add(crumbs, 1, 0);
            }
            topRow.Add(right, 2, 0);

            string titleText = _title.ToLowerInvariant();
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
            if (!string.IsNullOrEmpty(_tag))
                metaRow.Children.Add(DemoControls.Tag(_tag));
            if (!string.IsNullOrEmpty(_category))
                metaRow.Children.Add(new Label { Text = _category, FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center });
            if (wide)
                metaRow.Children.Add(new Label { Text = "· dnn module", FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.MutedText, VerticalOptions = LayoutOptions.Center });
            var desc = new Label { Text = _description, FontFamily = DemoTheme.BodyFont, FontSize = wide ? 16 : 15, TextColor = DemoTheme.SecondaryText };

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

        private (View tile, Label value) PerfTile(string caption)
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
            _perfTiles.Add(tile);
            return (tile, value);
        }

        // ---------- Performance ----------

        private void RecordReply(double seconds)
        {
            _replySeconds.Add(seconds);
            RefreshPerf();
        }

        private void RefreshPerf()
        {
            if (_replySeconds.Count == 0)
            {
                _perfLast.Text = "—";
                _perfAverage.Text = "—";
                _perfCount.Text = "0";
                return;
            }
            double total = 0;
            foreach (double s in _replySeconds)
                total += s;
            _perfLast.Text = FormatSeconds(_replySeconds[_replySeconds.Count - 1]);
            _perfAverage.Text = FormatSeconds(total / _replySeconds.Count);
            _perfCount.Text = _replySeconds.Count.ToString();
        }

        private static string FormatSeconds(double s) => s < 10 ? $"{s:0.0} s" : $"{s:0} s";

        // ---------- Share / sheets ----------

        private async Task ShareConversation()
        {
            if (_messages.Count == 0)
            {
                SetStatus("Nothing to share yet — send a message first.");
                return;
            }
            try
            {
                var sb = new System.Text.StringBuilder();
                foreach ((string speaker, string text) in _messages)
                {
                    sb.Append(speaker).Append(": ").AppendLine(text);
                    sb.AppendLine();
                }
                await Share.Default.RequestAsync(new ShareTextRequest { Title = _title, Text = sb.ToString().TrimEnd() });
            }
            catch (Exception ex)
            {
                SetStatus("Could not share: " + ex.Message);
            }
        }

        private void ShowCodeSheet()
        {
            string code = CodeSnippet;
            if (code == null)
                return;
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
                    new Label { Text = "The core of what this page runs, for the model selected now.", FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText },
                    codeBox
                }
            });
        }

        private void ShowInfoSheet()
        {
            var head = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            head.Add(new Label { Text = "about " + _title.ToLowerInvariant(), FontFamily = DemoTheme.HeroFont, FontSize = 26, CharacterSpacing = -0.8, TextColor = DemoTheme.PrimaryText, VerticalOptions = LayoutOptions.Center }, 0, 0);
            head.Add(DemoControls.CircleButton(DemoTheme.GlyphClose, CloseSheet, 34), 1, 0);
            var stack = new VerticalStackLayout { Spacing = 12, Children = { head } };
            foreach (string para in (AboutText ?? _description ?? "").Split("\n\n"))
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

        // ---------- Transcript ----------

        /// <summary>Remove every message from the transcript and show the empty state.</summary>
        protected void ClearTranscript()
        {
            _transcript.Children.Clear();
            _messages.Clear();
            _emptyState.IsVisible = true;
        }

        /// <summary>
        /// Append one turn to the transcript: the user's on the right in a bubble
        /// filled with the text color, the model's on the left in grey behind its
        /// avatar.
        /// </summary>
        protected void AddMessage(string text, bool isUser)
        {
            _emptyState.IsVisible = false;
            _messages.Add((isUser ? "You" : (SelectedModel?.Name ?? "Model"), text));

            var bubble = new Border
            {
                BackgroundColor = isUser ? UserBubble : ModelBubble,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                Padding = new Thickness(14, 11),
                StrokeShape = new RoundRectangle
                {
                    // Square off the corner nearest the speaker so the turns read
                    // as a conversation rather than two stacks of pills.
                    CornerRadius = isUser
                        ? new CornerRadius(18, 18, 18, 6)
                        : new CornerRadius(18, 18, 6, 18)
                },
                Content = new Label
                {
                    Text = text,
                    TextColor = isUser ? UserBubbleText : ModelBubbleText,
                    FontFamily = DemoTheme.BodyFont,
                    FontSize = 15,
                    LineBreakMode = LineBreakMode.WordWrap
                }
            };

            var timestamp = new Label
            {
                Text = DateTime.Now.ToString("t"),
                FontFamily = DemoTheme.BodyFont,
                FontSize = 11,
                TextColor = DemoTheme.MutedText,
                Margin = new Thickness(4, 0),
                HorizontalOptions = isUser ? LayoutOptions.End : LayoutOptions.Start
            };

            var column = new VerticalStackLayout
            {
                Spacing = 4,
                HorizontalOptions = isUser ? LayoutOptions.End : LayoutOptions.Start,
                MaximumWidthRequest = 520,
                Children = { bubble, timestamp }
            };

            View row;
            if (isUser)
            {
                row = column;
            }
            else
            {
                var grid = new Grid
                {
                    ColumnSpacing = 9,
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) }
                };
                grid.Add(Avatar(), 0, 0);
                grid.Add(column, 1, 0);
                row = grid;
            }

            _transcript.Children.Add(row);
            ScrollToEnd();
        }

        /// <summary>Show or hide the "thinking…" bubble at the end of the transcript.</summary>
        private void ShowTyping(bool on)
        {
            if (on)
            {
                if (_typingRow.Parent == null)
                    _transcript.Children.Add(_typingRow);
                _emptyState.IsVisible = false;
                ScrollToEnd();
            }
            else
            {
                _transcript.Children.Remove(_typingRow);
                if (_transcript.Children.Count == 0)
                    _emptyState.IsVisible = true;
            }
        }

        private Border Avatar() => new Border
        {
            WidthRequest = 30,
            HeightRequest = 30,
            BackgroundColor = DemoTheme.PillBackground,
            Stroke = Colors.Transparent,
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
            VerticalOptions = LayoutOptions.Start,
            Content = DemoTheme.MakeIcon(GlyphModel, DemoTheme.PrimaryText, 17)
        };

        /// <summary>Glyph identifying the module.</summary>
        protected string HeroGlyph { get; set; } = MaskRcnnPage.GlyphInfo;

        private async void ScrollToEnd()
        {
            try
            {
                // Let the new bubble get a size before scrolling to it. On desktop
                // the conversation has its own scroller; on the phone the whole
                // column scrolls and the conversation is at its end.
                await Task.Yield();
                if (_wide)
                    await _chatScroll.ScrollToAsync(0, _chatBody.Height, true);
                else if (_pageScroll?.Content is View content)
                    await _pageScroll.ScrollToAsync(0, content.Height, true);
            }
            catch (Exception)
            {
            }
        }

        // ---------- Status ----------

        /// <summary>Show a status line just above the composer, or hide it when null.</summary>
        protected void SetStatus(string message)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _statusRow.IsVisible = !string.IsNullOrEmpty(message);
                _statusLabel.Text = message ?? string.Empty;
                _statusDot.Color = StatusDotColor(message);
                UpdateModelState();
            });
        }

        // Green for ordinary news, lime while something is live (a download or an
        // answer in progress), red for a failure or a model that won't fit.
        private Color StatusDotColor(string message)
        {
            if (string.IsNullOrEmpty(message))
                return DemoTheme.StatusGreen;
            if (message.StartsWith("Error") || message.StartsWith("Could not") || message.StartsWith("That file") || message.Contains("more memory"))
                return BlockedText;
            if (_busy || DownloadState.IsActiveFor(_moduleKey))
                return DemoTheme.Lime;
            return DemoTheme.StatusGreen;
        }

        /// <summary>The dot + line under the model summary: loaded, downloading, downloaded or not.</summary>
        private void UpdateModelState()
        {
            ChatModel model = SelectedModel;
            if (model == null)
                return;

            Color dot;
            string text;
            if (IsModelLoaded)
            {
                dot = DemoTheme.StatusGreen;
                text = "Loaded · runs on this device";
            }
            else if (DownloadState.IsActiveFor(_moduleKey))
            {
                dot = DemoTheme.Lime;
                text = DownloadState.Message ?? "Preparing the model…";
            }
            else if (ExceedsDeviceMemory(model))
            {
                dot = BlockedText;
                text = "Probably too big for this device";
            }
            else if (IsInstalled(model))
            {
                dot = DemoTheme.StatusGreen;
                text = "Downloaded · loads with your first message";
            }
            else
            {
                dot = DemoTheme.MutedText;
                text = "Not downloaded · downloads with your first message";
            }
            _modelStateDot.Color = dot;
            _modelStateLabel.Text = text;
        }

        /// <summary>
        /// Mark the start of loading this module's model, before any bytes move.
        /// Call this instead of <see cref="SetStatus"/> when beginning a load that
        /// may download, so guards like the delete action see it immediately
        /// rather than only once the first progress callback lands.
        /// </summary>
        protected void BeginModelLoad(string message)
        {
            DownloadState.Begin(_moduleKey, message);
        }

        /// <summary>Format a download-progress callback into the status line.</summary>
        protected void OnDownloadProgress(long? totalBytesToReceive, long bytesReceived, double? progressPercentage)
        {
            string msg = totalBytesToReceive != null
                ? $"Downloading model… {bytesReceived / (1024 * 1024)} of {totalBytesToReceive.Value / (1024 * 1024)} MB ({(int)(progressPercentage ?? 0)}%)"
                : $"Downloading model… {bytesReceived / (1024 * 1024)} MB";
            DownloadState.Report(_moduleKey, msg);
        }

        // ---------- Download state that outlives the page ----------

        /// <summary>
        /// Progress for the in-flight model download.
        ///
        /// A page is a new instance every time it is navigated to, so anything kept
        /// in a field is gone the moment the user backs out — which made a running
        /// download look like it had reset. The transfer itself is unaffected by
        /// navigation (it is a plain task with no tie to page lifetime and no
        /// cancellation support), so the progress it reports is held here instead
        /// and re-attached by whichever page instance is on screen.
        /// </summary>
        private static class DownloadState
        {
            private static readonly object Sync = new object();

            /// <summary>Which module owns the running download, null if none.</summary>
            public static string Owner { get; private set; }

            public static string Message { get; private set; }

            /// <summary>Raised with the owning module and its latest message.</summary>
            public static event Action<string, string> Changed;

            public static bool IsActiveFor(string owner)
            {
                lock (Sync)
                    return Owner != null && Owner == owner;
            }

            /// <summary>
            /// Mark a download as started, before the first byte arrives.
            ///
            /// Registering at the point of intent rather than on the first
            /// progress callback matters: the gap covers the confirmation prompt,
            /// the SHA256 validation of any existing files and the initial
            /// request, and anything guarding on "is a download running" — such as
            /// the delete action — would wave the user straight through it.
            /// </summary>
            public static void Begin(string owner, string message)
            {
                lock (Sync)
                {
                    Owner = owner;
                    Message = message;
                }
                Changed?.Invoke(owner, message);
            }

            public static void Report(string owner, string message)
            {
                lock (Sync)
                {
                    Owner = owner;
                    Message = message;
                }
                Changed?.Invoke(owner, message);
            }

            public static void Finish(string owner)
            {
                lock (Sync)
                {
                    // Only the owner may clear it, and only if it was actually
                    // running: firing otherwise would wipe a status the caller has
                    // just set, such as a download failure.
                    if (Owner == null || Owner != owner)
                        return;
                    Owner = null;
                    Message = null;
                }
                Changed?.Invoke(owner, null);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BuildLayout();
            DownloadState.Changed += OnSharedDownloadProgress;
            if (DownloadState.IsActiveFor(_moduleKey))
                SetStatus(DownloadState.Message);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            DownloadState.Changed -= OnSharedDownloadProgress;
        }

        // Only react to this module's own download. Two chat pages share this
        // state, and echoing the other one's progress made a page look busy when
        // it was not — and, worse, silently swallowed the user's own send.
        private void OnSharedDownloadProgress(string owner, string message)
        {
            if (owner != _moduleKey)
                return;

            SetStatus(message);

            // Rebuild only when the download starts or stops, never on the
            // progress ticks in between: the delete action has to appear and
            // disappear with it, but rebuilding every row a few times a second
            // would make the list unusable.
            bool active = DownloadState.IsActiveFor(_moduleKey);
            if (active == _rowsShowDownloading)
                return;
            _rowsShowDownloading = active;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                BuildModelRows();
                UpdateModelSummary();
            });
        }

        // ---------- Send ----------

        private async Task OnSend()
        {
            if (_busy)
                return;

            string prompt = _promptEditor.Text;
            if (string.IsNullOrWhiteSpace(prompt))
                return;

            string reason;
            if (!CanSend(out reason))
            {
                SetStatus(reason ?? "Not ready yet.");
                return;
            }

            // Offer the override here as well as on the row. A module with a single
            // model shows no expander at all, so the row is not reachable and this
            // is the only place the user can get past the estimate — and it is
            // where they asked to run the thing anyway.
            ChatModel selected = SelectedModel;
            if (selected != null && ExceedsDeviceMemory(selected))
            {
                if (!await ConfirmOversizedAsync(selected))
                {
                    SetStatus($"{selected.Name} needs more memory than this device allows.");
                    return;
                }
                BuildModelRows();
                UpdateModelSummary();
            }

            // A download started by an earlier visit to this page is still running
            // and still writing to the model folder. Starting another one would
            // point a second writer at the same files (each is opened with
            // FileMode.Create), so wait for the first to land instead.
            if (DownloadState.IsActiveFor(_moduleKey))
            {
                SetStatus(DownloadState.Message);
                return;
            }

            SetBusy(true);
            try
            {
                if (!await EnsureModelReadyAsync())
                    return;

                _promptEditor.Text = string.Empty;
                AddMessage(prompt.Trim(), true);
                // The "thinking…" bubble shows the work in progress, so clear any
                // leftover download line rather than leave it reading 100%.
                SetStatus(null);
                ShowTyping(true);

                // Kept so teardown can wait for it: the model and (for the vision
                // module) the image are native objects this call is still reading
                // on a background thread. Disposing them out from under it is a
                // hard crash, not a catchable exception.
                // Timed here, around the generation alone: the download and model
                // load above are deliberately not part of the reply time.
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                Task<string> generation = Task.Run(() => Generate(prompt));
                _generation = generation;
                string response = await generation;
                stopwatch.Stop();

                ShowTyping(false);
                AddMessage((response ?? string.Empty).Trim(), false);
                RecordReply(stopwatch.Elapsed.TotalSeconds);
                SetStatus(null);
            }
            catch (Exception ex)
            {
                SetStatus("Error: " + ex.Message);
            }
            finally
            {
                ShowTyping(false);
                DownloadState.Finish(_moduleKey);
                SetBusy(false);
                UpdateModelSummary();
            }
        }

        private void SetBusy(bool busy)
        {
            _busy = busy;
            _sendIcon.IsVisible = !busy;
            _sendSpinner.IsVisible = busy;
            _sendSpinner.IsRunning = busy;
            _sendButton.Opacity = busy ? 0.75 : 1;
            _promptEditor.IsEnabled = !busy;
        }

        private void UpdateCounter()
        {
            string text = _promptEditor.Text ?? string.Empty;
            if (text.Length > MaxPromptLength)
            {
                text = text.Substring(0, MaxPromptLength);
                _promptEditor.Text = text;
            }
            _counterLabel.Text = SendHint + text.Length + " / " + MaxPromptLength;
        }

        // ---------- Model card ----------

        private async Task ToggleModelList()
        {
            _expanded = !_expanded;
            _modelRows.IsVisible = _expanded;
            // On the phone, performance and the code link fold away with the list.
            if (!_wide && _phoneDetails != null)
                _phoneDetails.IsVisible = _expanded;
            await _chevron.RotateToAsync(_expanded ? 180 : 0, 180, Easing.CubicOut);
        }

        private void BuildModelRows()
        {
            // A flat list: hairlines between rows instead of a card per row.
            _modelRows.Children.Clear();
            for (int i = 0; i < _models.Length; i++)
            {
                _modelRows.Children.Add(DemoTheme.Divider());
                _modelRows.Children.Add(BuildModelRow(i));
            }
            _modelRows.Children.Add(DemoTheme.Divider());

            if (_models.Length > 1)
            {
                var noteGrid = new Grid
                {
                    ColumnSpacing = 9,
                    Margin = new Thickness(0, 12, 0, 0),
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) }
                };
                var noteIcon = DemoTheme.MakeIcon(DemoTheme.GlyphInfo, DemoTheme.SecondaryText, 16);
                noteIcon.VerticalOptions = LayoutOptions.Start;
                noteGrid.Add(noteIcon, 0, 0);
                noteGrid.Add(new Label
                {
                    Text = "Larger models answer better but generate more slowly, and each one downloads separately.",
                    FontFamily = DemoTheme.BodyFont,
                    FontSize = 13,
                    TextColor = DemoTheme.SecondaryText
                }, 1, 0);

                _modelRows.Children.Add(noteGrid);
            }
        }

        private View BuildModelRow(int index)
        {
            ChatModel model = _models[index];
            bool selected = index == _selectedIndex;
            bool installed = IsInstalled(model);
            bool blocked = ExceedsDeviceMemory(model);

            var radioDot = new Border
            {
                WidthRequest = 10,
                HeightRequest = 10,
                BackgroundColor = DemoTheme.Accent,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(5) },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsVisible = selected
            };
            var radio = new Border
            {
                WidthRequest = 22,
                HeightRequest = 22,
                BackgroundColor = Colors.Transparent,
                Stroke = selected ? DemoTheme.Accent : DemoTheme.MutedText,
                StrokeThickness = selected ? 6 : 1.5,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(11) },
                VerticalOptions = LayoutOptions.Center,
                Content = radioDot
            };
            // A thick accent ring already reads as "filled"; the inner dot would
            // just muddy it at this size.
            radioDot.IsVisible = false;

            var name = new Label { Text = model.Name, FontFamily = DemoTheme.TitleFont, FontSize = 16, TextColor = DemoTheme.PrimaryText, LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 1 };
            var detail = new Label { Text = model.Detail, FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText, LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 2 };
            var text = new VerticalStackLayout { Spacing = 2, VerticalOptions = LayoutOptions.Center, Children = { name, detail } };

            var badges = new HorizontalStackLayout { Spacing = 6, VerticalOptions = LayoutOptions.Center };
            if (blocked)
            {
                // One clear reason beats a stack of badges that ends in "but you
                // can't have it".
                // Kept short: this badge shares a row with the model name, and a
                // long caption starves it. Tapping the row explains in full.
                badges.Children.Add(Badge("Too big", BlockedText, BlockedFill));
            }
            else
            {
                // Plain grey pill either way; black type marks the one that's ready.
                badges.Children.Add(Badge(
                    installed ? "Installed" : "Not installed",
                    installed ? DemoTheme.PrimaryText : DemoTheme.SecondaryText,
                    DemoTheme.PillBackground));
            }


            var grid = new Grid
            {
                ColumnSpacing = 12,
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                }
            };
            grid.Add(radio, 0, 0);
            grid.Add(text, 1, 0);
            grid.Add(badges, 2, 0);

            // Unavailable rows stay visible but visibly inert: hiding them would
            // just look like the option went missing, and the user would wonder
            // where the bigger model went.
            if (blocked)
            {
                radio.IsVisible = false;
                name.TextColor = DemoTheme.SecondaryText;
            }

            var rowContent = new VerticalStackLayout { Children = { grid } };

            // Anything on disk can be removed — including a half-finished download,
            // which is exactly the case worth reclaiming and the one a plain
            // "Installed" check would miss. This replaces the old "Default" badge:
            // which model is the default matters far less on a phone than being
            // able to get several gigabytes back.
            //
            // It sits on its own labelled line rather than as an icon among the
            // badges: a bare glyph next to a status pill reads as decoration, and
            // the one thing a destructive action must never be is ambiguous. It is
            // also withheld entirely while this model is downloading, so the
            // tap that deletes a file mid-write is not even offered.
            long onDisk = FolderBytes(model);
            if (onDisk > 0 && !DownloadState.IsActiveFor(_moduleKey))
                rowContent.Children.Add(DeleteRow(model, onDisk));

            // Flat row on whatever it sits on (the page, or the panel well); the
            // filled radio marks the selection and hairlines (added by
            // BuildModelRows) separate the rows.
            var row = new Border
            {
                BackgroundColor = Colors.Transparent,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                Padding = new Thickness(0, 14),
                Opacity = blocked ? 0.6 : 1,
                Content = rowContent
            };

            int captured = index;
            var tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) =>
            {
                if (blocked)
                {
                    if (!await ConfirmOversizedAsync(model))
                        return;
                    SelectModel(captured);
                    BuildModelRows();
                    UpdateModelSummary();
                    return;
                }
                SelectModel(captured);
            };
            row.GestureRecognizers.Add(tap);
            return row;
        }

        // ---------- Deleting a downloaded model ----------

        /// <summary>Total bytes the model currently occupies on disk, 0 if absent.</summary>
        private static long FolderBytes(ChatModel model)
        {
            try
            {
                string root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var dir = new DirectoryInfo(System.IO.Path.Combine(root, "emgu", model.Folder));
                if (!dir.Exists)
                    return 0;
                long total = 0;
                foreach (FileInfo f in dir.GetFiles("*", SearchOption.AllDirectories))
                    total += f.Length;
                return total;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private static string FormatSize(long bytes)
        {
            double gb = bytes / (1024.0 * 1024.0 * 1024.0);
            if (gb >= 1)
                return $"{gb:0.0} GB";
            return $"{bytes / (1024.0 * 1024.0):0} MB";
        }

        private View DeleteRow(ChatModel model, long onDisk)
        {
            var icon = MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphClose, BlockedText, 16);
            icon.VerticalOptions = LayoutOptions.Center;

            var label = new Label
            {
                // Says what it removes and what you get back, so the consequence is
                // on the button rather than only in the confirmation.
                Text = $"Delete download · frees {FormatSize(onDisk)}",
                FontFamily = DemoTheme.TitleFont,
                FontSize = 13,
                TextColor = BlockedText,
                VerticalOptions = LayoutOptions.Center
            };

            var content = new HorizontalStackLayout
            {
                Spacing = 6,
                Padding = new Thickness(0, 9, 0, 1),
                Children = { icon, label }
            };

            var stack = new VerticalStackLayout
            {
                Margin = new Thickness(0, 9, 0, 0),
                Children =
                {
                    DemoTheme.Divider(),
                    content
                }
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) => await DeleteModel(model, onDisk);
            stack.GestureRecognizers.Add(tap);
            return stack;
        }

        private async Task DeleteModel(ChatModel model, long onDisk)
        {
            if (_busy)
            {
                SetStatus("Hang on — still answering.");
                return;
            }
            if (DownloadState.IsActiveFor(_moduleKey))
            {
                SetStatus("This model is downloading right now. Let it finish first.");
                return;
            }

            bool confirmed = await DisplayAlertAsync(
                $"Delete {model.Name}?",
                $"This frees {FormatSize(onDisk)} on this device. You can download the model again later.",
                "Delete",
                "Keep");
            if (!confirmed)
                return;

            try
            {
                // Let the module drop any loaded instance first: the files are about
                // to disappear from under it.
                OnModelDeleted(model);

                string root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string path = System.IO.Path.Combine(root, "emgu", model.Folder);
                if (Directory.Exists(path))
                    Directory.Delete(path, true);

                SetStatus($"Deleted {model.Name} — {FormatSize(onDisk)} freed.");
            }
            catch (Exception ex)
            {
                SetStatus("Could not delete the model: " + ex.Message);
            }

            BuildModelRows();
            UpdateModelSummary();
        }

        /// <summary>
        /// Called just before a model's files are deleted, so the module can
        /// dispose any loaded copy of it.
        /// </summary>
        protected virtual void OnModelDeleted(ChatModel model) { }

        private static Border Badge(string text, Color textColor, Color fill) => new Border
        {
            BackgroundColor = fill,
            Stroke = Colors.Transparent,
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
            Padding = new Thickness(12, 5),
            VerticalOptions = LayoutOptions.Center,
            Content = new Label { Text = text, FontFamily = DemoTheme.BodyFont, FontSize = 12, CharacterSpacing = 0.5, TextColor = textColor }
        };

        private void SelectModel(int index)
        {
            if (index == _selectedIndex || _busy)
                return;
            _selectedIndex = index;
            // Reply times belong to one model; start counting again for the new one.
            _replySeconds.Clear();
            RefreshPerf();
            BuildModelRows();
            UpdateModelSummary();
            OnModelChanged(SelectedModel);
        }

        /// <summary>Refresh the summary row (e.g. after a download completes).</summary>
        protected void UpdateModelSummary()
        {
            ChatModel model = SelectedModel;
            if (model == null)
                return;

            _collapsedName.Text = model.Name;
            _collapsedDetail.Text = model.Detail;
            UpdateModelState();

            // "Too big" outranks "Installed": a model sitting on disk that the
            // device cannot load is not usable, and saying "Installed" here is the
            // exact contradiction that sends someone to press Send and get killed.
            if (ExceedsDeviceMemory(model))
            {
                _collapsedBadgeLabel.Text = "Too big";
                _collapsedBadgeLabel.TextColor = BlockedText;
                _collapsedBadge.BackgroundColor = BlockedFill;
                return;
            }

            bool installed = IsInstalled(model);
            _collapsedBadgeLabel.Text = installed ? "Installed" : "Not installed";
            _collapsedBadgeLabel.TextColor = installed ? DemoTheme.PrimaryText : DemoTheme.SecondaryText;
            _collapsedBadge.BackgroundColor = DemoTheme.PillBackground;
        }

        // ---------- Does this model fit in memory? ----------

        // Peak memory as a multiple of the weight files on disk.
        //
        // Measured, not guessed, and it is much worse than it looks: the ONNX
        // importer builds its own graph and tensors rather than mapping the file,
        // so the weights are effectively resident more than once during load. Two
        // observations on an iPhone 16 (8 GB), both killed by the OS:
        //
        //   Qwen3 0.6B    3.0 GB of weights  -> killed
        //   SmolVLM2 256M 1.1 GB of weights  -> killed
        //
        // The smaller one already exceeded a budget of ~3.2 GB, which puts the
        // real multiplier near or above 3x. Anything lower here hands the user a
        // model that loads for a while and then takes the whole app down.
        private const double WeightOverhead = 3.0;

        // Share of physical RAM one app can realistically hold. iOS enforces a
        // per-process limit well below the device total and kills anything that
        // crosses it outright (SIGKILL, no exception, no crash log), so this is
        // deliberately pessimistic.
        //
        // Calibration point, measured on this project: an iPhone 16 (8 GB) was
        // killed while loading Qwen3 0.6B, whose weights are 2.8 GB — so the real
        // ceiling there is under 2.8 x 1.3 = 3.7 GB, i.e. below 46% of RAM. 0.40
        // sits under that and still clears the ~1.1 GB models comfortably.
        //
        // This is an estimate, not a documented limit. It errs towards declaring a
        // model unavailable, which costs the user a model they might have squeezed
        // in; the alternative — an unexplained instant termination mid-download —
        // is far worse. A paid developer account can lift the real ceiling with the
        // Increased Memory Limit entitlement (see Platforms/iOS/Entitlements.plist).
        private const double UsableMemoryShare = 0.40;

        private static readonly long MemoryBudget = (long)(PhysicalMemoryBytes() * UsableMemoryShare);

        private static ulong PhysicalMemoryBytes()
        {
            try
            {
#if __IOS__ || __MACCATALYST__
                return Foundation.NSProcessInfo.ProcessInfo.PhysicalMemory;
#elif ANDROID
                var manager = (Android.App.ActivityManager)Android.App.Application.Context
                    .GetSystemService(Android.Content.Context.ActivityService);
                var info = new Android.App.ActivityManager.MemoryInfo();
                manager.GetMemoryInfo(info);
                return (ulong)info.TotalMem;
#else
                // Desktop: not memory-constrained in the way phones are, and there
                // is no cheap portable probe. Report unknown and block nothing.
                return 0;
#endif
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Whether this device is too small to hold the model. Unknown sizes and
        /// unknown device memory both count as "fits", so the check can only ever
        /// rule out a model it has real numbers for.
        /// </summary>
        private static bool ExceedsDeviceMemory(ChatModel model)
        {
            if (model.WeightBytes <= 0 || MemoryBudget <= 0)
                return false;
            if (_forceAllowed.Contains(model.Folder))
                return false;
            return model.WeightBytes * WeightOverhead > MemoryBudget;
        }

        // Models the user has explicitly chosen to try despite the estimate.
        //
        // The budget above is inferred from observed kills, not from any figure
        // Apple publishes, and it changes with the build: a Release build carries a
        // fraction of Debug's runtime overhead. An estimate that can only ever say
        // no would permanently hide a model that might run fine, so the block is a
        // strong warning the user can override rather than a locked door.
        private static readonly HashSet<string> _forceAllowed = new HashSet<string>();

        /// <summary>
        /// Warn that a model looks too big and let the user proceed anyway.
        /// Returns true if they chose to continue, and records the choice so the
        /// model stops being blocked for the rest of the session.
        /// </summary>
        private async Task<bool> ConfirmOversizedAsync(ChatModel model)
        {
            bool tryAnyway = await DisplayAlertAsync(
                $"{model.Name} may be too big",
                $"Needs about {FormatSize((long)(model.WeightBytes * WeightOverhead))} to load; this device allows " +
                $"{DescribeBudget()}. iOS will probably close the app.\n\nThis is an estimate — you can still try.",
                "Try anyway",
                "Cancel");

            if (tryAnyway)
                _forceAllowed.Add(model.Folder);
            return tryAnyway;
        }

        private static string DescribeBudget()
        {
            double gb = MemoryBudget / (1024.0 * 1024.0 * 1024.0);
            return $"about {gb:0.0} GB";
        }

        /// <summary>
        /// Whether every file of a model is already on disk at its full size.
        ///
        /// This is deliberately a presence-and-size check, not the SHA256 validation
        /// <see cref="Emgu.Util.DownloadableFile.IsLocalFileValid"/> performs: hashing
        /// a 2.8 GB model would stall the page every time this badge is drawn.
        ///
        /// The size comparison matters more than it looks. Downloads are written
        /// straight to the final path with FileMode.Create, so an interrupted one
        /// leaves a real file at the right name holding partial content — an
        /// existence check alone reports that as "Installed" and flatly contradicts
        /// the download prompt the user then gets. Checking the exact byte count
        /// catches every truncated file; only a same-length corruption slips
        /// through, and the SHA256 gate still refuses to load that.
        ///
        /// The folder resolves through the same API the downloader uses, so this
        /// always looks exactly where the files will actually be written — on iOS
        /// that is the app's Documents folder, not Library/Application Support.
        /// </summary>
        private static bool IsInstalled(ChatModel model)
        {
            if (model.Files.Length == 0)
                return false;
            try
            {
                string root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                foreach ((string name, long size) in model.Files)
                {
                    var info = new FileInfo(System.IO.Path.Combine(root, "emgu", model.Folder, name));
                    if (!info.Exists || info.Length == 0)
                        return false;
                    if (size > 0 && info.Length != size)
                        return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Clear the keyboard inset left on the page's scroll views after the keyboard
        /// is dismissed on iOS/MacCatalyst, which otherwise freezes scrolling until
        /// the editor is focused again.
        /// </summary>
        private void ResetScrollAfterKeyboard()
        {
#if __IOS__ || __MACCATALYST__
            //Run after MAUI's own keyboard-dismiss handling has settled.
            this.Dispatcher.Dispatch(() =>
            {
                // The page content is now a layout grid, so reset the scroll views
                // it holds: the page column, and on desktop the conversation's own.
                foreach (ScrollView view in new[] { _pageScroll, _chatScroll })
                {
                    if (view?.Handler?.PlatformView is UIKit.UIScrollView scrollView)
                    {
                        scrollView.ContentInset = UIKit.UIEdgeInsets.Zero;
                        scrollView.ScrollIndicatorInsets = UIKit.UIEdgeInsets.Zero;
                        scrollView.ScrollEnabled = true;
                    }
                }
            });
#endif
        }
    }
}
