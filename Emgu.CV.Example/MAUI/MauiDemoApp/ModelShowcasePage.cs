//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Models;
using Emgu.CV.Structure;

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace MauiDemoApp
{
    /// <summary>
    /// A reusable, on-theme showcase page for any <see cref="IProcessAndRenderModel"/>.
    /// It reproduces the bespoke Mask R-CNN experience (curated sample photos, a
    /// photo/camera picker, a styled results card and — on iOS — a full-screen
    /// live camera page) for every other detection module, without duplicating the
    /// layout. Each module only supplies a title, subtitle, glyph, a factory for its
    /// model, its sample photos and (optionally) a version picker.
    ///
    /// Unlike the bespoke Mask R-CNN page (which renders a structured per-object
    /// list), this base uses the common <see cref="IProcessAndRenderModel.ProcessAndRender"/>
    /// contract: it shows the annotated image plus the message string the model
    /// returns. That works uniformly for every module.
    /// </summary>
    public class ModelShowcasePage : ContentPage
    {
        /// <summary>A curated sample photo shown in the Change Photo sheet.</summary>
        public sealed class Sample
        {
            public string File { get; }
            public string Name { get; }
            public string Glyph { get; }
            public Sample(string file, string name, string glyph)
            {
                File = file; Name = name; Glyph = glyph;
            }
        }

        private readonly Func<IProcessAndRenderModel> _modelFactory;
        private readonly Sample[] _samples;
        private readonly string _pickerTitle;
        private readonly string[] _pickerOptions;
        private readonly bool _hasCamera;
        private readonly bool _hasStillImage;
        private readonly string _title;

        // Optional per-module hook: given the model and input image, produce the
        // annotated PNG plus a list of textual result items (e.g. recognized
        // words). When set, the Results card lists these rows instead of the
        // generic message string.
        private readonly Func<IProcessAndRenderModel, Mat, (byte[] png, IReadOnlyList<string> items)> _customProcess;

        private IProcessAndRenderModel _model;
        private Mat _currentImage;
        private Mat _renderBuffer;
        private bool _busy;
        private bool _defaultLoaded;
        private byte[] _lastResultPng;
        private int _pickerIndex = -1;

        // Runtime UI.
        private readonly Image _previewImage;
        private readonly VerticalStackLayout _resultsBody;
        private readonly VerticalStackLayout _emptyState;
        private readonly Label _resultsStatusLabel;
        private readonly Image _resultsCheck;
        private readonly Label _resultsTimeLabel;
        private readonly Label _resultsMessage;
        private readonly Button _detectButton;
        private readonly Button _liveButton;
        private readonly Grid _loadingOverlay;
        private readonly Label _loadingLabel;
        private readonly ActivityIndicator _loadingIndicator;
        private readonly Microsoft.Maui.Controls.Picker _versionPicker;

        // Change Photo sheet.
        private readonly Grid _sheetOverlay;
        private readonly BoxView _sheetScrim;
        private readonly Border _sheetCard;
        private readonly VerticalStackLayout _sheetList;
        private TaskCompletionSource<string> _sheetTcs;
        private bool _sheetAnimating;

        /// <summary>
        /// Create a showcase page for a model.
        /// </summary>
        /// <param name="title">Module name, shown in the header.</param>
        /// <param name="subtitle">One-line description under the title.</param>
        /// <param name="glyph">Material Symbols glyph for the header tile.</param>
        /// <param name="modelFactory">Creates a fresh model instance (used by both the still page and the live page).</param>
        /// <param name="samples">Curated sample photos. Pass null/empty for a camera-only module.</param>
        /// <param name="pickerTitle">Optional label for a version picker.</param>
        /// <param name="pickerOptions">Optional version options, passed to Init as the selection string.</param>
        /// <param name="hasCamera">Whether the module supports live camera detection.</param>
        public ModelShowcasePage(
            string title,
            string subtitle,
            string glyph,
            Func<IProcessAndRenderModel> modelFactory,
            Sample[] samples,
            string pickerTitle = null,
            string[] pickerOptions = null,
            bool hasCamera = true,
            Func<IProcessAndRenderModel, Mat, (byte[] png, IReadOnlyList<string> items)> customProcess = null)
        {
            _modelFactory = modelFactory;
            _customProcess = customProcess;
            _samples = samples ?? Array.Empty<Sample>();
            _pickerTitle = pickerTitle;
            _pickerOptions = pickerOptions;
            _hasCamera = hasCamera;
            _hasStillImage = _samples.Length > 0;
            _title = title;

            _model = modelFactory();

            Title = title;
            BackgroundColor = MaskRcnnPage.PageBackground;
            Shell.SetNavBarIsVisible(this, false);

            // ---------- Header ----------
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
            var subtitleLabel = new Label
            {
                Text = subtitle,
                FontFamily = MaskRcnnPage.BodyFont,
                FontSize = 15,
                TextColor = MaskRcnnPage.SecondaryText,
                Margin = new Thickness(0, 2, 0, 0)
            };
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

            var pageChildren = new VerticalStackLayout
            {
                Spacing = 18,
                Padding = new Thickness(20, 16, 20, 28),
                Children = { header }
            };

            // ---------- Input Image card (only for still-image modules) ----------
            _previewImage = new Image { Aspect = Aspect.AspectFit, HeightRequest = 340 };
            if (_hasStillImage)
            {
                var inputTitle = new Label { Text = "Input Image", FontFamily = MaskRcnnPage.TitleFont, FontSize = 19, TextColor = MaskRcnnPage.PrimaryText, VerticalOptions = LayoutOptions.Center };
                var changePhoto = PillButton(MaskRcnnPage.GlyphImage, "Change Photo", OnChangePhoto);
                var inputHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
                inputHeader.Add(inputTitle, 0, 0);
                inputHeader.Add(changePhoto, 1, 0);

                var previewFrame = new Border
                {
                    BackgroundColor = MaskRcnnPage.ImageBackground,
                    Stroke = MaskRcnnPage.RowBorder,
                    StrokeThickness = 1,
                    Padding = new Thickness(10),
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
                    Content = _previewImage
                };
                var previewTap = new TapGestureRecognizer();
                previewTap.Tapped += async (s, e) =>
                {
                    if (_lastResultPng != null)
                        await Navigation.PushModalAsync(new FullScreenImagePage(_lastResultPng, title));
                };
                previewFrame.GestureRecognizers.Add(previewTap);

                var inputChildren = new VerticalStackLayout { Spacing = 14, Children = { inputHeader, previewFrame } };

                // Optional version picker.
                if (_pickerOptions != null && _pickerOptions.Length > 0)
                {
                    _versionPicker = new Microsoft.Maui.Controls.Picker
                    {
                        Title = _pickerTitle,
                        FontFamily = MaskRcnnPage.BodyFont,
                        TextColor = MaskRcnnPage.PrimaryText,
                        TitleColor = MaskRcnnPage.SecondaryText
                    };
                    foreach (string opt in _pickerOptions)
                        _versionPicker.Items.Add(opt);
                    _versionPicker.SelectedIndex = 0;
                    _pickerIndex = 0;
                    _versionPicker.SelectedIndexChanged += OnPickerChanged;

                    var pickerFrame = new Border
                    {
                        BackgroundColor = MaskRcnnPage.ImageBackground,
                        Stroke = MaskRcnnPage.RowBorder,
                        StrokeThickness = 1,
                        Padding = new Thickness(12, 2),
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                        Content = _versionPicker
                    };
                    inputChildren.Children.Add(pickerFrame);
                }

                var inputCard = new Border
                {
                    BackgroundColor = MaskRcnnPage.CardBackground,
                    Stroke = Colors.Transparent,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                    Padding = new Thickness(16),
                    Content = inputChildren
                };
                pageChildren.Children.Add(inputCard);
            }

            // ---------- Buttons ----------
            _detectButton = new Button
            {
                Text = _hasStillImage ? "Detect Image" : "Open Camera",
                FontFamily = MaskRcnnPage.TitleFont,
                FontSize = 17,
                BackgroundColor = MaskRcnnPage.Accent,
                TextColor = Colors.White,
                CornerRadius = 16,
                HeightRequest = 56,
                ImageSource = new FontImageSource { FontFamily = MaskRcnnPage.IconFont, Glyph = _hasStillImage ? MaskRcnnPage.GlyphPlay : MaskRcnnPage.GlyphVideo, Color = Colors.White, Size = 20 },
                ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 8)
            };
            if (_hasStillImage)
                _detectButton.Clicked += OnDetectClicked;
            else
                _detectButton.Clicked += OnLiveDetection;

            var buttonChildren = new VerticalStackLayout { Spacing = 12, Children = { _detectButton } };

            // A separate Live button only makes sense when the primary action is a
            // still-image detect and the module also supports the camera.
            _liveButton = new Button
            {
                Text = "Live Detection",
                FontFamily = MaskRcnnPage.TitleFont,
                FontSize = 16,
                BackgroundColor = MaskRcnnPage.CardBackground,
                TextColor = MaskRcnnPage.Accent,
                BorderColor = MaskRcnnPage.Accent,
                BorderWidth = 1,
                CornerRadius = 16,
                HeightRequest = 52,
                ImageSource = new FontImageSource { FontFamily = MaskRcnnPage.IconFont, Glyph = MaskRcnnPage.GlyphVideo, Color = MaskRcnnPage.Accent, Size = 20 },
                ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 8)
            };
            _liveButton.Clicked += OnLiveDetection;
            if (_hasStillImage && _hasCamera)
                buttonChildren.Children.Add(_liveButton);
            pageChildren.Children.Add(buttonChildren);

            // ---------- Results card ----------
            var resultsTitle = new Label { Text = "Results", FontFamily = MaskRcnnPage.TitleFont, FontSize = 19, TextColor = MaskRcnnPage.PrimaryText, VerticalOptions = LayoutOptions.Center };

            _resultsStatusLabel = new Label { Text = "Ready", FontFamily = MaskRcnnPage.BodyFont, FontSize = 14, TextColor = MaskRcnnPage.SecondaryText, VerticalOptions = LayoutOptions.Center };
            _resultsCheck = MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphCheck, Color.FromArgb("#2BA84A"), 18);
            _resultsCheck.IsVisible = false;
            _resultsTimeLabel = new Label { Text = "", FontFamily = MaskRcnnPage.BodyFont, FontSize = 12, TextColor = MaskRcnnPage.SecondaryText, IsVisible = false, HorizontalTextAlignment = TextAlignment.End };
            var statusRow = new HorizontalStackLayout { Spacing = 6, HorizontalOptions = LayoutOptions.End, Children = { _resultsStatusLabel, _resultsCheck } };
            var statusStack = new VerticalStackLayout { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, Children = { statusRow, _resultsTimeLabel } };

            var resultsHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            resultsHeader.Add(resultsTitle, 0, 0);
            resultsHeader.Add(statusStack, 1, 0);

            _emptyState = new VerticalStackLayout
            {
                Spacing = 6,
                Padding = new Thickness(0, 18, 0, 8),
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphImage, Color.FromArgb("#C2C7D6"), 40),
                    new Label { Text = "No results yet", FontFamily = MaskRcnnPage.TitleFont, FontSize = 15, TextColor = MaskRcnnPage.PrimaryText, HorizontalTextAlignment = TextAlignment.Center },
                    new Label { Text = _hasStillImage ? "Tap the button above to analyze." : "Open the camera to start.", FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = MaskRcnnPage.SecondaryText, HorizontalTextAlignment = TextAlignment.Center }
                }
            };

            _resultsMessage = new Label { FontFamily = MaskRcnnPage.BodyFont, FontSize = 15, TextColor = MaskRcnnPage.PrimaryText, IsVisible = false };
            _resultsBody = new VerticalStackLayout { Spacing = 0, Children = { _resultsMessage } };

            var resultsCard = new Border
            {
                BackgroundColor = MaskRcnnPage.CardBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                Padding = new Thickness(16),
                Content = new VerticalStackLayout { Spacing = 12, Children = { resultsHeader, _emptyState, _resultsBody } }
            };
            pageChildren.Children.Add(resultsCard);

            // ---------- Loading overlay ----------
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

            // ---------- Change Photo sheet ----------
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
            var sheetScrimTap = new TapGestureRecognizer();
            sheetScrimTap.Tapped += (s, e) => CloseSheet(null);
            _sheetScrim.GestureRecognizers.Add(sheetScrimTap);
            _sheetOverlay = new Grid { IsVisible = false, Children = { _sheetScrim, _sheetCard } };
            if (_hasStillImage)
                BuildSheet();

            Content = new Grid { Children = { new Microsoft.Maui.Controls.ScrollView { Content = pageChildren }, _loadingOverlay, _sheetOverlay } };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_hasStillImage && !_defaultLoaded)
            {
                _defaultLoaded = true;
                Mat m = await LoadSampleAsync(0);
                if (m != null)
                    SetCurrentImage(m);
            }
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            base.OnNavigatedFrom(args);
            if (!Navigation.NavigationStack.Contains(this) && !Navigation.ModalStack.Contains(this))
            {
                _currentImage?.Dispose();
                _currentImage = null;
                _renderBuffer?.Dispose();
                _renderBuffer = null;
                _model?.Dispose();
                _model = null;
            }
        }

        // ---------- Version picker ----------

        private void OnPickerChanged(object sender, EventArgs e)
        {
            if (_versionPicker.SelectedIndex == _pickerIndex)
                return;
            _pickerIndex = _versionPicker.SelectedIndex;
            // A different version means the model must be re-initialized.
            _model?.Clear();
            ResetResults();
        }

        private string CurrentPickerSelection =>
            (_versionPicker != null && _versionPicker.SelectedIndex >= 0)
                ? _versionPicker.Items[_versionPicker.SelectedIndex]
                : null;

        // ---------- Image sources ----------

        private async void OnChangePhoto(object sender, EventArgs e)
        {
            string action = await OpenChangePhotoSheet();
            if (string.IsNullOrEmpty(action))
                return;

            if (action.StartsWith("sample:"))
            {
                int idx = int.Parse(action.Substring("sample:".Length));
                Mat sm = await LoadSampleAsync(idx);
                if (sm != null) SetCurrentImage(sm);
                return;
            }

            try
            {
                FileResult file = action == "camera"
                    ? await MediaPicker.Default.CapturePhotoAsync()
                    : await MediaPicker.Default.PickPhotoAsync();
                if (file == null)
                    return;

                SetBusy(true, "Loading photo…");
                using Stream stream = await file.OpenReadAsync();
                using MemoryStream ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                Mat m = new Mat();
                CvInvoke.Imdecode(ms.ToArray(), ImreadModes.ColorBgr, m);
                SetBusy(false);
                if (m.IsEmpty)
                {
                    m.Dispose();
                    await DisplayAlert("Photo", "That file could not be read as an image.", "OK");
                    return;
                }
                SetCurrentImage(MaskRcnnPage.Downscale(m, 1024));
            }
            catch (Exception ex)
            {
                SetBusy(false);
                await DisplayAlert("Could not load photo", ex.Message, "OK");
            }
        }

        // ---------- Change Photo bottom sheet ----------

        private void BuildSheet()
        {
            _sheetList.Children.Clear();
            _sheetList.Children.Add(new BoxView { WidthRequest = 40, HeightRequest = 5, CornerRadius = 3, Color = Color.FromArgb("#D5D8E0"), HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 10, 0, 6) });
            _sheetList.Children.Add(new Label { Text = "Change Photo", FontFamily = MaskRcnnPage.TitleFont, FontSize = 20, TextColor = MaskRcnnPage.PrimaryText, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 0, 6) });

            _sheetList.Children.Add(SheetSection("SAMPLE IMAGES"));
            for (int i = 0; i < _samples.Length; i++)
                _sheetList.Children.Add(SheetRow(_samples[i].Glyph, _samples[i].Name, $"sample:{i}", i < _samples.Length - 1));

            _sheetList.Children.Add(SheetSection("YOUR PHOTOS"));
            bool cam = MediaPicker.Default.IsCaptureSupported;
            _sheetList.Children.Add(SheetRow(MaskRcnnPage.GlyphImage, "Photo Library", "library", cam));
            if (cam)
                _sheetList.Children.Add(SheetRow(MaskRcnnPage.GlyphCamera, "Take Photo", "camera", false));

            var cancelLabel = new Label { Text = "Cancel", FontFamily = MaskRcnnPage.TitleFont, FontSize = 17, TextColor = MaskRcnnPage.Accent, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            var cancel = new Border { BackgroundColor = Color.FromArgb("#F2F2F7"), Stroke = Colors.Transparent, StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) }, HeightRequest = 52, Margin = new Thickness(0, 14, 0, 0), Content = cancelLabel };
            var cancelTap = new TapGestureRecognizer();
            cancelTap.Tapped += (s, e) => CloseSheet(null);
            cancel.GestureRecognizers.Add(cancelTap);
            _sheetList.Children.Add(cancel);
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

        private View SheetRow(string glyph, string text, string value, bool divider)
        {
            var grid = new Grid
            {
                Padding = new Thickness(2, 14),
                ColumnSpacing = 16,
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) }
            };
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

        private Task<string> OpenChangePhotoSheet()
        {
            _sheetTcs = new TaskCompletionSource<string>();
            _sheetOverlay.IsVisible = true;
            _sheetScrim.Opacity = 0;
            _sheetCard.TranslationY = 700;
            _ = Task.WhenAll(
                _sheetScrim.FadeTo(1, 220, Easing.CubicOut),
                _sheetCard.TranslateTo(0, 0, 260, Easing.CubicOut));
            return _sheetTcs.Task;
        }

        private async void CloseSheet(string value)
        {
            if (_sheetAnimating)
                return;
            _sheetAnimating = true;
            await Task.WhenAll(
                _sheetScrim.FadeTo(0, 180, Easing.CubicIn),
                _sheetCard.TranslateTo(0, 700, 220, Easing.CubicIn));
            _sheetOverlay.IsVisible = false;
            _sheetAnimating = false;
            _sheetTcs?.TrySetResult(value);
        }

        private async Task<Mat> LoadSampleAsync(int idx)
        {
            try
            {
                byte[] bytes = await MaskRcnnPage.ReadAppFileAsync(_samples[idx].File);
                Mat m = new Mat();
                CvInvoke.Imdecode(bytes, ImreadModes.ColorBgr, m);
                return m.IsEmpty ? null : m;
            }
            catch
            {
                return null;
            }
        }

        private void SetCurrentImage(Mat image)
        {
            _currentImage?.Dispose();
            _currentImage = image;
            _previewImage.Source = MaskRcnnPage.MatToImageSource(image);
            ResetResults();
        }

        private void ResetResults()
        {
            _resultsBody.Children.Clear();
            _resultsBody.Children.Add(_resultsMessage);
            _emptyState.IsVisible = true;
            _resultsMessage.IsVisible = false;
            _resultsMessage.Text = "";
            _resultsStatusLabel.Text = "Ready";
            _resultsStatusLabel.TextColor = MaskRcnnPage.SecondaryText;
            _resultsCheck.IsVisible = false;
            _resultsTimeLabel.IsVisible = false;
            _lastResultPng = null;
        }

        // ---------- Detection (still image) ----------

        private async void OnDetectClicked(object sender, EventArgs e)
        {
            if (_busy || _currentImage == null || _model == null)
                return;

            _busy = true;
            _detectButton.IsEnabled = false;
            SetBusy(true, "Preparing…");

            try
            {
                if (!_model.Initialized)
                {
                    SetBusy(true, "Downloading model…\n(first time only)");
                    string selection = CurrentPickerSelection;
                    await Task.Run(() => _model.Init(OnDownloadProgress, selection));
                    if (!_model.Initialized)
                    {
                        ShowError("Could not download or initialize the model. Check your connection and try again.");
                        return;
                    }
                }

                SetBusy(true, "Analyzing…");
                Mat input = _currentImage;
                var sw = Stopwatch.StartNew();

                byte[] png;
                if (_customProcess != null)
                {
                    // Module supplies structured items (e.g. recognized words).
                    IReadOnlyList<string> items;
                    (png, items) = await Task.Run(() => _customProcess(_model, input));
                    sw.Stop();
                    ShowItemResults(items);
                }
                else
                {
                    // Generic: annotated image + the model's message string. Most
                    // models only return timing, so strip it (we show our own
                    // timestamp) and fall back to a friendly line.
                    string message;
                    (png, message) = await Task.Run(() => RunProcess(input));
                    sw.Stop();
                    ShowMessageResult(CleanMessage(message));
                }

                _lastResultPng = png;
                if (png != null)
                    _previewImage.Source = MaskRcnnPage.MatBytesToImageSource(png);

                _resultsStatusLabel.TextColor = MaskRcnnPage.PrimaryText;
                _resultsCheck.IsVisible = true;
                _resultsTimeLabel.Text = $"Analyzed in {sw.ElapsedMilliseconds} ms";
                _resultsTimeLabel.IsVisible = true;
            }
            catch (Exception ex)
            {
                ShowError("Detection failed: " + ex.Message);
            }
            finally
            {
                _busy = false;
                _detectButton.IsEnabled = true;
                SetBusy(false);
            }
        }

        // Runs on a background thread: process the image and return the annotated
        // PNG plus the model's message. Honors the model's RenderMethod.
        private (byte[] png, string message) RunProcess(Mat input)
        {
            if (_renderBuffer == null)
                _renderBuffer = new Mat();
            input.CopyTo(_renderBuffer);
            string msg = _model.ProcessAndRender(input, _renderBuffer);
            return (MaskRcnnPage.Encode(_renderBuffer), msg);
        }

        // Show the generic single-message result (annotated image + one line).
        private void ShowMessageResult(string detail)
        {
            _resultsBody.Children.Clear();
            _resultsBody.Children.Add(_resultsMessage);
            _resultsMessage.IsVisible = true;
            _resultsMessage.Text = detail ?? "Results are highlighted on the image above.";
            _emptyState.IsVisible = false;
            _resultsStatusLabel.Text = "Complete";
        }

        // Show a list of textual result items (e.g. recognized words) as rows.
        private void ShowItemResults(IReadOnlyList<string> items)
        {
            _resultsBody.Children.Clear();
            _resultsMessage.IsVisible = false;
            _emptyState.IsVisible = false;

            if (items == null || items.Count == 0)
            {
                _resultsBody.Children.Add(_resultsMessage);
                _resultsMessage.IsVisible = true;
                _resultsMessage.Text = "Nothing recognized in this image.";
                _resultsStatusLabel.Text = "0 found";
                return;
            }

            for (int i = 0; i < items.Count; i++)
                _resultsBody.Children.Add(BuildTextRow(items[i], i == items.Count - 1));
            _resultsStatusLabel.Text = items.Count == 1 ? "1 found" : $"{items.Count} found";
        }

        private static View BuildTextRow(string text, bool isLast)
        {
            var icon = MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphCheck, MaskRcnnPage.Accent, 16);
            var label = new Label { Text = text, FontFamily = MaskRcnnPage.BodyFont, FontSize = 16, TextColor = MaskRcnnPage.PrimaryText, VerticalOptions = LayoutOptions.Center };
            var grid = new Grid
            {
                Padding = new Thickness(2, 14),
                ColumnSpacing = 12,
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) }
            };
            grid.Add(icon, 0, 0);
            grid.Add(label, 1, 0);
            var stack = new VerticalStackLayout();
            stack.Children.Add(grid);
            if (!isLast)
                stack.Children.Add(new BoxView { HeightRequest = 1, Color = MaskRcnnPage.RowBorder });
            return stack;
        }

        // Remove any timing phrase from a model's result message (we show our own
        // timestamp). Returns null when nothing meaningful is left, so the caller
        // can substitute a friendly default. Keeps real content (barcode values,
        // OCR text, "No barcodes found", etc.).
        private static string CleanMessage(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
                return null;
            // Drop "(in 42 ms)" / "in 42 milliseconds" style phrases.
            string s = System.Text.RegularExpressions.Regex.Replace(
                msg, @"\(?\s*in\s+\d+\s*(ms|milliseconds)\s*\)?",
                "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            s = s.Trim().Trim('.', ' ', '(', ')').Trim();
            // "Detected" (with the timing stripped) carries no extra information.
            if (s.Length == 0 || s.Equals("Detected", StringComparison.OrdinalIgnoreCase))
                return null;
            return s;
        }

        private async void OnLiveDetection(object sender, EventArgs e)
        {
#if __IOS__ || __MACCATALYST__
            await Navigation.PushModalAsync(new ModelShowcaseLivePage(_title, _modelFactory, CurrentPickerSelection));
#else
            await Navigation.PushAsync(new Emgu.CV.Platform.Maui.UI.ProcessAndRenderPage(
                _modelFactory(), _title + " (Live)", null, "Real-time detection from the camera."));
#endif
        }

        private void OnDownloadProgress(long? totalBytesToReceive, long bytesReceived, double? progressPercentage)
        {
            string msg = totalBytesToReceive != null
                ? $"Downloading model…\n{bytesReceived / (1024 * 1024)} of {totalBytesToReceive.Value / (1024 * 1024)} MB ({(int)(progressPercentage ?? 0)}%)"
                : $"Downloading model…\n{bytesReceived / (1024 * 1024)} MB";
            MainThread.BeginInvokeOnMainThread(() => _loadingLabel.Text = msg);
        }

        private void ShowError(string message)
        {
            _emptyState.IsVisible = true;
            _resultsMessage.IsVisible = false;
            ((Label)_emptyState.Children[1]).Text = "Something went wrong";
            ((Label)_emptyState.Children[2]).Text = message;
        }

        private void SetBusy(bool busy, string message = "Working…")
        {
            _loadingLabel.Text = message;
            _loadingOverlay.IsVisible = busy;
            _loadingIndicator.IsRunning = busy;
        }

        // ---------- Small styled controls ----------

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
            t.Tapped += (s, e) => onTap(s, e);
            pill.GestureRecognizers.Add(t);
            return pill;
        }
    }

#if __IOS__ || __MACCATALYST__
    /// <summary>
    /// Full-screen, real-time detection from the camera for any
    /// <see cref="IProcessAndRenderModel"/>. Mirrors the Mask R-CNN live page but
    /// drives the generic <see cref="IProcessAndRenderModel.ProcessAndRender"/> per
    /// frame.
    /// </summary>
    internal class ModelShowcaseLivePage : Emgu.Util.AvCaptureSessionPage
    {
        private readonly IProcessAndRenderModel _model;
        private readonly string _pickerSelection;
        private readonly Image _feed;
        private readonly Grid _overlay;
        private readonly Label _loadingLabel;
        private readonly ActivityIndicator _spinner;
        private bool _running;
        private bool _busy;
        private bool _started;
        private Mat _frame;
        private Mat _renderMat;

        public ModelShowcaseLivePage(string title, Func<IProcessAndRenderModel> modelFactory, string pickerSelection)
        {
            _model = modelFactory();
            _pickerSelection = pickerSelection;

            BackgroundColor = MaskRcnnPage.PageBackground;
            On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            AllowAvCaptureSession = true;
            outputRecorder.BufferReceived += OnCameraFrame;

            _feed = new Image { Aspect = Aspect.AspectFill, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill };

            var exitRow = new HorizontalStackLayout
            {
                Spacing = 2,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Children =
                {
                    MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphChevronLeft, MaskRcnnPage.Accent, 24),
                    new Label { Text = "Exit", FontFamily = MaskRcnnPage.TitleFont, FontSize = 17, TextColor = MaskRcnnPage.Accent, VerticalOptions = LayoutOptions.Center }
                }
            };
            var exitTap = new TapGestureRecognizer();
            exitTap.Tapped += async (s, e) =>
            {
                _running = false;
                await Navigation.PopModalAsync();
            };
            exitRow.GestureRecognizers.Add(exitTap);

            var titleLabel = new Label { Text = title, FontFamily = MaskRcnnPage.TitleFont, FontSize = 18, TextColor = MaskRcnnPage.PrimaryText, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };

            var topBar = new Grid
            {
                Padding = new Thickness(16, 8, 16, 10),
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
            };
            topBar.Add(titleLabel, 0, 0);
            Grid.SetColumnSpan(titleLabel, 3);
            topBar.Add(exitRow, 0, 0);

            var dot = new Border { WidthRequest = 10, HeightRequest = 10, BackgroundColor = Color.FromArgb("#2BE07A"), Stroke = Colors.Transparent, StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(5) }, VerticalOptions = LayoutOptions.Center };
            var scanningTitle = new Label { Text = "Scanning…", FontFamily = MaskRcnnPage.TitleFont, FontSize = 15, TextColor = Colors.White };
            var scanningSub = new Label { Text = "Point your camera at the subject", FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = Color.FromArgb("#D6D6DE"), HorizontalOptions = LayoutOptions.Center };
            var pill = new Border
            {
                BackgroundColor = Color.FromArgb("#E01A1C2E"),
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(18) },
                Padding = new Thickness(22, 12),
                Margin = new Thickness(0, 0, 0, 20),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                Content = new VerticalStackLayout
                {
                    Spacing = 2,
                    Children =
                    {
                        new HorizontalStackLayout { Spacing = 8, HorizontalOptions = LayoutOptions.Center, Children = { dot, scanningTitle } },
                        scanningSub
                    }
                }
            };

            var feedCard = new Border
            {
                BackgroundColor = Colors.Black,
                Stroke = MaskRcnnPage.RowBorder,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(24) },
                Margin = new Thickness(16, 4, 16, 20),
                Content = new Grid { Children = { _feed, pill } }
            };

            _spinner = new ActivityIndicator { IsRunning = false, Color = MaskRcnnPage.Accent, WidthRequest = 44, HeightRequest = 44, HorizontalOptions = LayoutOptions.Center };
            _loadingLabel = new Label { Text = "Starting camera…", FontFamily = MaskRcnnPage.BodyFont, FontSize = 15, TextColor = MaskRcnnPage.PrimaryText, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            var loadingBox = new VerticalStackLayout { Spacing = 14, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, Children = { _spinner, _loadingLabel } };
            _overlay = new Grid { BackgroundColor = Color.FromArgb("#F5EEF1F8"), Children = { loadingBox } };

            var root = new Grid { RowDefinitions = { new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Star) } };
            root.Add(topBar, 0, 0);
            root.Add(feedCard, 0, 1);

            Content = new Grid { Children = { root, _overlay } };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_started)
                return;
            _started = true;

            SetOverlay(true, "Downloading model…\n(first time only)");
            await Task.Run(() => _model.Init(OnDownloadProgress, _pickerSelection));
            if (!_model.Initialized)
            {
                SetOverlay(true, "Could not load the model.\nCheck your connection and exit.");
                return;
            }

            SetOverlay(false, null);
            _running = true;
            CheckVideoPermissionAndStart();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _running = false;
            try { if (session != null) StopCaptureSession(); } catch { }

            Mat frame = _frame;
            Mat render = _renderMat;
            _frame = null;
            _renderMat = null;
            Task.Run(async () =>
            {
                for (int i = 0; i < 40 && _busy; i++)
                    await Task.Delay(100);
                try { frame?.Dispose(); } catch { }
                try { render?.Dispose(); } catch { }
                try { _model.Dispose(); } catch { }
            });
        }

        private void OnCameraFrame(object sender, Emgu.Util.OutputRecorder.BufferReceivedEventArgs e)
        {
            if (!_running || _busy)
                return;
            _busy = true;
            try
            {
                if (_frame == null)
                    _frame = new Mat();

                using (CoreVideo.CVPixelBuffer pixelBuffer = e.Buffer.GetImageBuffer() as CoreVideo.CVPixelBuffer)
                {
                    pixelBuffer.Lock(CoreVideo.CVPixelBufferLock.ReadOnly);
                    using (CoreImage.CIImage ciImage = new CoreImage.CIImage(pixelBuffer))
                        ciImage.ToArray(_frame, ImreadModes.ColorBgr);
                    pixelBuffer.Unlock(CoreVideo.CVPixelBufferLock.ReadOnly);
                }

                if (_frame.IsEmpty)
                    return;

                CvInvoke.Rotate(_frame, _frame, RotateFlags.Rotate90Clockwise);

                if (_renderMat == null)
                    _renderMat = new Mat();
                _frame.CopyTo(_renderMat);
                _model.ProcessAndRender(_frame, _renderMat);
                byte[] png = MaskRcnnPage.Encode(_renderMat);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (_running)
                        _feed.Source = MaskRcnnPage.MatBytesToImageSource(png);
                });
            }
            catch
            {
                // Skip a bad frame rather than crashing the live feed.
            }
            finally
            {
                _busy = false;
            }
        }

        private void OnDownloadProgress(long? totalBytesToReceive, long bytesReceived, double? progressPercentage)
        {
            string msg = totalBytesToReceive != null
                ? $"Downloading model…\n{bytesReceived / (1024 * 1024)} of {totalBytesToReceive.Value / (1024 * 1024)} MB ({(int)(progressPercentage ?? 0)}%)"
                : $"Downloading model…\n{bytesReceived / (1024 * 1024)} MB";
            MainThread.BeginInvokeOnMainThread(() => _loadingLabel.Text = msg);
        }

        private void SetOverlay(bool visible, string message)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _overlay.IsVisible = visible;
                _spinner.IsRunning = visible;
                if (message != null)
                    _loadingLabel.Text = message;
            });
        }

        public override void SetMessage(String message, int heightRequest = 60)
        {
            MainThread.BeginInvokeOnMainThread(() => _loadingLabel.Text = message);
        }
    }
#endif
}
