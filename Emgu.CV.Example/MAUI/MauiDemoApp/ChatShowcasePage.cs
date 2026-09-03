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
    /// Qwen Chat and Chat with Image the same shell the detection modules get from
    /// <see cref="ModelShowcasePage"/>: the card layout, palette, fonts and spacing
    /// all come from the shared tokens on <see cref="MaskRcnnPage"/>.
    ///
    /// The page owns everything that is identical between the two demos — header,
    /// model card, transcript, composer, busy and download state — and leaves each
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

        // Bubble palette. The user's turn picks up the same accent tint the app
        // uses for selected rows and pills; the model's turn uses the neutral
        // image/well background, so a long answer never becomes a wall of accent.
        private static readonly Color UserBubble = MaskRcnnPage.TileBackground;
        private static readonly Color ModelBubble = MaskRcnnPage.ImageBackground;
        private static readonly Color InstalledText = Color.FromArgb("#2BA84A");
        private static readonly Color InstalledFill = Color.FromArgb("#E7F7EC");
        private static readonly Color BlockedText = Color.FromArgb("#C2453D");
        private static readonly Color BlockedFill = Color.FromArgb("#FBEAE9");

        private const int MaxPromptLength = 4000;

        // Both already in the subset icon font; MaskRcnnPage doesn't declare them,
        // so keep local copies rather than widen that page's surface. Written as
        // escapes rather than literal private-use characters, which do not survive
        // every editor and tool that touches this file.
        private const string GlyphExpandMore = ""; // expand_more
        // auto_awesome. Used for the model chip and the model's chat avatar so they
        // read as "the model" rather than repeating the page's own title glyph.
        private const string GlyphModel = "";

        private readonly ChatModel[] _models;
        private readonly string _composerPlaceholder;

        private int _selectedIndex;
        private bool _expanded;
        private bool _busy;
        private Task _generation;
        private readonly string _moduleKey;
        private bool _rowsShowDownloading;

        // Runtime UI.
        private readonly VerticalStackLayout _transcript;
        private readonly VerticalStackLayout _emptyState;
        private readonly VerticalStackLayout _modelRows;
        private readonly Label _collapsedName;
        private readonly Label _collapsedDetail;
        private readonly Border _collapsedBadge;
        private readonly Label _collapsedBadgeLabel;
        private readonly Image _chevron;
        private readonly Border _chevronDisc;
        private readonly Editor _promptEditor;
        private readonly Label _counterLabel;
        private readonly Border _sendButton;
        private readonly Image _sendIcon;
        private readonly ActivityIndicator _sendSpinner;
        private readonly Label _statusLabel;
        private readonly Border _statusCard;
        private readonly ScrollView _scroll;
        private readonly VerticalStackLayout _pageChildren;

        /// <summary>The model the user currently has selected.</summary>
        protected ChatModel SelectedModel => _models.Length > 0 ? _models[_selectedIndex] : null;

        /// <summary>
        /// Create a chat page.
        /// </summary>
        /// <param name="title">Page title shown in the header bar.</param>
        /// <param name="heroTitle">Headline inside the hero card.</param>
        /// <param name="heroSubtitle">Supporting line under the headline.</param>
        /// <param name="glyph">Material Symbols glyph for the hero tile and model avatars.</param>
        /// <param name="models">Selectable models. Pass a single entry for a module with no choice.</param>
        /// <param name="actionGlyph">Glyph for the header action pill.</param>
        /// <param name="actionText">Caption for the header action pill.</param>
        /// <param name="composerPlaceholder">Placeholder text for the message box.</param>
        protected ChatShowcasePage(
            string title,
            string heroTitle,
            string heroSubtitle,
            string glyph,
            ChatModel[] models,
            string actionGlyph,
            string actionText,
            string composerPlaceholder)
        {
            _models = models ?? Array.Empty<ChatModel>();
            _composerPlaceholder = composerPlaceholder;
            HeroGlyph = glyph;
            _moduleKey = title;

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
            BackgroundColor = MaskRcnnPage.PageBackground;
            Shell.SetNavBarIsVisible(this, false);

            // ---------- Header ----------
            var backButton = CircleButton(MaskRcnnPage.GlyphChevronLeft, async () => await Navigation.PopAsync());
            var titleLabel = new Label
            {
                Text = title,
                FontFamily = MaskRcnnPage.TitleFont,
                FontSize = 20,
                TextColor = MaskRcnnPage.PrimaryText,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center
            };
            var topRow = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                }
            };
            topRow.Add(backButton, 0, 0);
            topRow.Add(titleLabel, 1, 0);

            // A module whose action belongs next to its own content (e.g. "Change
            // Photo" above the image, as the detection pages do it) passes no
            // caption and places BuildActionPill() itself.
            if (!string.IsNullOrEmpty(actionText))
                topRow.Add(BuildActionPill(actionGlyph, actionText), 2, 0);

            // ---------- Hero card ----------
            var heroTile = new Border
            {
                WidthRequest = 54,
                HeightRequest = 54,
                BackgroundColor = MaskRcnnPage.TileBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
                VerticalOptions = LayoutOptions.Start,
                Content = MaskRcnnPage.MakeIcon(glyph, MaskRcnnPage.Accent, 28)
            };
            var heroText = new VerticalStackLayout
            {
                Spacing = 3,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label { Text = heroTitle, FontFamily = MaskRcnnPage.TitleFont, FontSize = 21, TextColor = MaskRcnnPage.PrimaryText },
                    new Label { Text = heroSubtitle, FontFamily = MaskRcnnPage.BodyFont, FontSize = 14, TextColor = MaskRcnnPage.SecondaryText }
                }
            };
            var heroRow = new Grid
            {
                ColumnSpacing = 14,
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) }
            };
            heroRow.Add(heroTile, 0, 0);
            heroRow.Add(heroText, 1, 0);

            // ---------- Model card ----------
            // Collapsed, this is a single summary row; expanded, it reveals the
            // full radio list plus a note about what the choice costs.
            // Truncate rather than wrap: the badge beside these sizes to its own
            // content, so a long one (e.g. "Too big") would otherwise squeeze the
            // name into a one-word-per-line tower.
            _collapsedName = new Label { FontFamily = MaskRcnnPage.TitleFont, FontSize = 16, TextColor = MaskRcnnPage.PrimaryText, LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 1 };
            _collapsedDetail = new Label { FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = MaskRcnnPage.SecondaryText, LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 1 };
            _collapsedBadgeLabel = new Label { FontFamily = MaskRcnnPage.TitleFont, FontSize = 12, VerticalOptions = LayoutOptions.Center };
            _collapsedBadge = new Border
            {
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(9) },
                Padding = new Thickness(9, 4),
                VerticalOptions = LayoutOptions.Center,
                Content = _collapsedBadgeLabel
            };
            _chevron = MaskRcnnPage.MakeIcon(GlyphExpandMore, MaskRcnnPage.SecondaryText, 22);
            _chevron.VerticalOptions = LayoutOptions.Center;

            var summaryAvatar = new Border
            {
                WidthRequest = 36,
                HeightRequest = 36,
                BackgroundColor = MaskRcnnPage.TileBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(11) },
                VerticalOptions = LayoutOptions.Center,
                Content = MaskRcnnPage.MakeIcon(GlyphModel, MaskRcnnPage.Accent, 19)
            };
            var summaryText = new VerticalStackLayout
            {
                Spacing = 1,
                VerticalOptions = LayoutOptions.Center,
                Children = { _collapsedName, _collapsedDetail }
            };

            // A tinted disc behind the chevron so the row reads as something you
            // can open, rather than a bare status line.
            _chevronDisc = new Border
            {
                WidthRequest = 30,
                HeightRequest = 30,
                BackgroundColor = MaskRcnnPage.ImageBackground,
                Stroke = Colors.Transparent,
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
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                }
            };
            summaryRow.Add(summaryAvatar, 0, 0);
            summaryRow.Add(summaryText, 1, 0);
            summaryRow.Add(summaryTrailing, 2, 0);

            var summaryFrame = new Border
            {
                BackgroundColor = MaskRcnnPage.CardBackground,
                Stroke = MaskRcnnPage.RowBorder,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
                Padding = new Thickness(12, 10),
                Margin = new Thickness(0, 14, 0, 0),
                Content = summaryRow
            };

            _modelRows = new VerticalStackLayout { Spacing = 8, IsVisible = false, Margin = new Thickness(0, 12, 0, 0) };

            // The expander opens even for a single model. There is nothing to
            // choose between, but the row behind it is where the size, the
            // installed state, the delete action and any "too big for this device"
            // explanation live — hiding all of that just because a module ships one
            // model left the vision page looking like a different, emptier app.
            if (_models.Length > 0)
            {
                var expandTap = new TapGestureRecognizer();
                expandTap.Tapped += async (s, e) => await ToggleModelList();
                summaryFrame.GestureRecognizers.Add(expandTap);
            }
            else
            {
                _chevronDisc.IsVisible = false;
            }

            var heroChildren = new VerticalStackLayout { Children = { heroRow } };
            if (_models.Length > 0)
            {
                heroChildren.Children.Add(summaryFrame);
                heroChildren.Children.Add(_modelRows);
            }

            var heroCard = new Border
            {
                BackgroundColor = MaskRcnnPage.CardBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                Padding = new Thickness(16),
                Content = heroChildren
            };

            // ---------- Transcript ----------
            _emptyState = new VerticalStackLayout
            {
                Spacing = 6,
                Padding = new Thickness(0, 26, 0, 22),
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    MaskRcnnPage.MakeIcon(glyph, Color.FromArgb("#C2C7D6"), 40),
                    new Label { Text = "No messages yet", FontFamily = MaskRcnnPage.TitleFont, FontSize = 15, TextColor = MaskRcnnPage.PrimaryText, HorizontalTextAlignment = TextAlignment.Center },
                    new Label { Text = EmptyStateHint, FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = MaskRcnnPage.SecondaryText, HorizontalTextAlignment = TextAlignment.Center }
                }
            };
            _transcript = new VerticalStackLayout { Spacing = 14, Children = { _emptyState } };

            var transcriptCard = new Border
            {
                BackgroundColor = MaskRcnnPage.CardBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                Padding = new Thickness(14),
                Content = _transcript
            };

            // ---------- Status line ----------
            _statusLabel = new Label { FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = MaskRcnnPage.Accent, VerticalOptions = LayoutOptions.Center };
            _statusCard = new Border
            {
                BackgroundColor = MaskRcnnPage.TileBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                Padding = new Thickness(12, 10),
                IsVisible = false,
                Content = new HorizontalStackLayout
                {
                    Spacing = 9,
                    Children = { MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphInfo, MaskRcnnPage.Accent, 18), _statusLabel }
                }
            };

            // ---------- Composer ----------
            _promptEditor = new Editor
            {
                Placeholder = _composerPlaceholder,
                PlaceholderColor = MaskRcnnPage.SecondaryText,
                BackgroundColor = Colors.Transparent,
                TextColor = MaskRcnnPage.PrimaryText,
                FontFamily = MaskRcnnPage.BodyFont,
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
                Text = "0 / " + MaxPromptLength,
                FontFamily = MaskRcnnPage.BodyFont,
                FontSize = 12,
                TextColor = MaskRcnnPage.SecondaryText,
                VerticalOptions = LayoutOptions.Center
            };

            _sendIcon = MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphPlay, Colors.White, 24);
            _sendSpinner = new ActivityIndicator { Color = Colors.White, WidthRequest = 22, HeightRequest = 22, IsRunning = false, IsVisible = false };
            _sendButton = new Border
            {
                WidthRequest = 48,
                HeightRequest = 48,
                BackgroundColor = MaskRcnnPage.Accent,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(24) },
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Content = new Grid { Children = { _sendIcon, _sendSpinner } }
            };
            var sendTap = new TapGestureRecognizer();
            sendTap.Tapped += async (s, e) => await OnSend();
            _sendButton.GestureRecognizers.Add(sendTap);

            var composerFooter = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            composerFooter.Add(_counterLabel, 0, 0);
            composerFooter.Add(_sendButton, 1, 0);

            var composerCard = new Border
            {
                BackgroundColor = MaskRcnnPage.CardBackground,
                Stroke = MaskRcnnPage.RowBorder,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                Padding = new Thickness(14, 8, 14, 12),
                Content = new VerticalStackLayout { Spacing = 4, Children = { _promptEditor, composerFooter } }
            };

            // ---------- Page ----------
            _pageChildren = new VerticalStackLayout
            {
                Spacing = 16,
                Padding = new Thickness(20, 16, 20, 28),
                Children = { topRow, heroCard }
            };

            // Modules with extra chrome (e.g. the image being discussed) drop it in
            // between the hero card and the transcript.
            View extra = BuildExtraCard();
            if (extra != null)
                _pageChildren.Children.Add(extra);

            _pageChildren.Children.Add(_statusCard);
            _pageChildren.Children.Add(transcriptCard);
            _pageChildren.Children.Add(composerCard);

            _scroll = new Microsoft.Maui.Controls.ScrollView { Content = _pageChildren };
            Content = _scroll;

            BuildModelRows();
            UpdateModelSummary();
        }

        // ---------- Hooks for the concrete modules ----------

        /// <summary>Hint shown under "No messages yet".</summary>
        protected virtual string EmptyStateHint => "Ask a question to get started.";

        /// <summary>Optional card inserted between the hero card and the transcript.</summary>
        protected virtual View BuildExtraCard() => null;

        /// <summary>
        /// Make sure the selected model is loaded, downloading it if needed. Report
        /// progress with <see cref="SetStatus"/>. Return false to abort the turn.
        /// </summary>
        protected abstract Task<bool> EnsureModelReadyAsync();

        /// <summary>Produce the answer for a prompt. Called on a background thread.</summary>
        protected abstract string Generate(string prompt);

        /// <summary>Called when the user picks a different model.</summary>
        protected virtual void OnModelChanged(ChatModel model) { }

        /// <summary>Called when the header action pill is tapped.</summary>
        protected virtual void OnHeaderAction(object sender, EventArgs e) { }

        /// <summary>
        /// Build the module's action button in the shared pill style, guarded so it
        /// cannot fire mid-turn: "New Chat" resets the model's conversation state
        /// and "Change Photo" swaps the image out, and the background generation is
        /// still using both. Letting either through corrupts the answer at best and
        /// crashes inside native code at worst.
        /// </summary>
        protected View BuildActionPill(string glyph, string text) => PillButton(glyph, text, (s, e) =>
        {
            if (_busy)
            {
                SetStatus("Hang on — still answering.");
                return;
            }
            OnHeaderAction(s, e);
        });

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

        // ---------- Transcript ----------

        /// <summary>Remove every message from the transcript and show the empty state.</summary>
        protected void ClearTranscript()
        {
            _transcript.Children.Clear();
            _transcript.Children.Add(_emptyState);
            _emptyState.IsVisible = true;
        }

        /// <summary>
        /// Append one turn to the transcript: the user's on the right in the accent
        /// tint, the model's on the left behind its avatar.
        /// </summary>
        protected void AddMessage(string text, bool isUser)
        {
            _emptyState.IsVisible = false;

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
                    TextColor = MaskRcnnPage.PrimaryText,
                    FontFamily = MaskRcnnPage.BodyFont,
                    FontSize = 15,
                    LineBreakMode = LineBreakMode.WordWrap
                }
            };

            var timestamp = new Label
            {
                Text = DateTime.Now.ToString("t"),
                FontFamily = MaskRcnnPage.BodyFont,
                FontSize = 11,
                TextColor = MaskRcnnPage.SecondaryText,
                Margin = new Thickness(4, 0),
                HorizontalOptions = isUser ? LayoutOptions.End : LayoutOptions.Start
            };

            var column = new VerticalStackLayout
            {
                Spacing = 4,
                HorizontalOptions = isUser ? LayoutOptions.End : LayoutOptions.Start,
                MaximumWidthRequest = 460,
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

        private Border Avatar() => new Border
        {
            WidthRequest = 30,
            HeightRequest = 30,
            BackgroundColor = MaskRcnnPage.Accent,
            Stroke = Colors.Transparent,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
            VerticalOptions = LayoutOptions.Start,
            Content = MaskRcnnPage.MakeIcon(GlyphModel, Colors.White, 17)
        };

        /// <summary>Glyph identifying the module, used for the hero tile and empty state.</summary>
        protected string HeroGlyph { get; set; } = MaskRcnnPage.GlyphInfo;

        private async void ScrollToEnd()
        {
            try
            {
                // Let the new bubble get a size before scrolling to it.
                await Task.Yield();
                await _scroll.ScrollToAsync(0, _pageChildren.Height, true);
            }
            catch (Exception)
            {
            }
        }

        // ---------- Status ----------

        /// <summary>Show a status line above the transcript, or hide it when null.</summary>
        protected void SetStatus(string message)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _statusCard.IsVisible = !string.IsNullOrEmpty(message);
                _statusLabel.Text = message ?? string.Empty;
            });
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
                SetStatus("Generating…");

                // Kept so teardown can wait for it: the model and (for the vision
                // module) the image are native objects this call is still reading
                // on a background thread. Disposing them out from under it is a
                // hard crash, not a catchable exception.
                Task<string> generation = Task.Run(() => Generate(prompt));
                _generation = generation;
                string response = await generation;

                AddMessage((response ?? string.Empty).Trim(), false);
                SetStatus(null);
            }
            catch (Exception ex)
            {
                SetStatus("Error: " + ex.Message);
            }
            finally
            {
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
            _counterLabel.Text = text.Length + " / " + MaxPromptLength;
        }

        // ---------- Model card ----------

        private async Task ToggleModelList()
        {
            _expanded = !_expanded;
            _modelRows.IsVisible = _expanded;
            await _chevron.RotateToAsync(_expanded ? 180 : 0, 180, Easing.CubicOut);
        }

        private void BuildModelRows()
        {
            _modelRows.Children.Clear();
            for (int i = 0; i < _models.Length; i++)
                _modelRows.Children.Add(BuildModelRow(i));

            if (_models.Length > 1)
            {
                var noteGrid = new Grid
                {
                    ColumnSpacing = 9,
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) }
                };
                var noteIcon = MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphInfo, MaskRcnnPage.Accent, 18);
                noteIcon.VerticalOptions = LayoutOptions.Start;
                noteGrid.Add(noteIcon, 0, 0);
                noteGrid.Add(new Label
                {
                    Text = "Larger models answer better but generate more slowly, and each one downloads separately.",
                    FontFamily = MaskRcnnPage.BodyFont,
                    FontSize = 13,
                    TextColor = MaskRcnnPage.Accent
                }, 1, 0);

                _modelRows.Children.Add(new Border
                {
                    BackgroundColor = MaskRcnnPage.TileBackground,
                    Stroke = Colors.Transparent,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                    Padding = new Thickness(12, 10),
                    Margin = new Thickness(0, 2, 0, 0),
                    Content = noteGrid
                });
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
                BackgroundColor = MaskRcnnPage.Accent,
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
                Stroke = selected ? MaskRcnnPage.Accent : Color.FromArgb("#C2C7D6"),
                StrokeThickness = selected ? 6 : 1.5,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(11) },
                VerticalOptions = LayoutOptions.Center,
                Content = radioDot
            };
            // A thick accent ring already reads as "filled"; the inner dot would
            // just muddy it at this size.
            radioDot.IsVisible = false;

            var name = new Label { Text = model.Name, FontFamily = MaskRcnnPage.TitleFont, FontSize = 16, TextColor = MaskRcnnPage.PrimaryText, LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 1 };
            var detail = new Label { Text = model.Detail, FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = MaskRcnnPage.SecondaryText, LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 2 };
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
                badges.Children.Add(Badge(
                    installed ? "Installed" : "Not installed",
                    installed ? InstalledText : MaskRcnnPage.SecondaryText,
                    installed ? InstalledFill : MaskRcnnPage.ImageBackground));
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
                name.TextColor = MaskRcnnPage.SecondaryText;
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

            var row = new Border
            {
                BackgroundColor = selected && !blocked ? MaskRcnnPage.TileBackground : MaskRcnnPage.CardBackground,
                Stroke = selected && !blocked ? MaskRcnnPage.Accent : MaskRcnnPage.RowBorder,
                StrokeThickness = selected && !blocked ? 1.5 : 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(15) },
                Padding = new Thickness(12, 11),
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
                FontFamily = MaskRcnnPage.TitleFont,
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
                    new BoxView { HeightRequest = 1, Color = MaskRcnnPage.RowBorder },
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
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(9) },
            Padding = new Thickness(9, 4),
            VerticalOptions = LayoutOptions.Center,
            Content = new Label { Text = text, FontFamily = MaskRcnnPage.TitleFont, FontSize = 12, TextColor = textColor }
        };

        private void SelectModel(int index)
        {
            if (index == _selectedIndex || _busy)
                return;
            _selectedIndex = index;
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
            _collapsedBadgeLabel.TextColor = installed ? InstalledText : MaskRcnnPage.SecondaryText;
            _collapsedBadge.BackgroundColor = installed ? InstalledFill : MaskRcnnPage.ImageBackground;
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

        // ---------- Small styled controls (mirrors ModelShowcasePage) ----------

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

        private Border PillButton(string glyph, string text, EventHandler onTap)
        {
            var content = new HorizontalStackLayout { Spacing = 7, VerticalOptions = LayoutOptions.Center };
            content.Children.Add(MaskRcnnPage.MakeIcon(glyph, MaskRcnnPage.Accent, 18));
            content.Children.Add(new Label { Text = text, FontFamily = MaskRcnnPage.TitleFont, FontSize = 14, TextColor = MaskRcnnPage.Accent, VerticalOptions = LayoutOptions.Center });

            var pill = new Border
            {
                BackgroundColor = MaskRcnnPage.TileBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(13) },
                Padding = new Thickness(14, 9),
                VerticalOptions = LayoutOptions.Center,
                Content = content
            };
            var t = new TapGestureRecognizer();
            t.Tapped += (s, e) => onTap(s, e);
            pill.GestureRecognizers.Add(t);
            return pill;
        }

        /// <summary>
        /// Clear the keyboard inset left on the page ScrollView after the keyboard
        /// is dismissed on iOS/MacCatalyst, which otherwise freezes scrolling until
        /// the editor is focused again.
        /// </summary>
        private void ResetScrollAfterKeyboard()
        {
#if __IOS__ || __MACCATALYST__
            //Run after MAUI's own keyboard-dismiss handling has settled.
            this.Dispatcher.Dispatch(() =>
            {
                if (this.Content?.Handler?.PlatformView is UIKit.UIScrollView scrollView)
                {
                    scrollView.ContentInset = UIKit.UIEdgeInsets.Zero;
                    scrollView.ScrollIndicatorInsets = UIKit.UIEdgeInsets.Zero;
                    scrollView.ScrollEnabled = true;
                }
            });
#endif
        }
    }
}
