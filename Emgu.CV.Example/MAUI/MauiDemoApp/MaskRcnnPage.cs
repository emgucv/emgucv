//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Models;
using Emgu.CV.Structure;
using Emgu.CV.Util;

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

using DrawSize = System.Drawing.Size;

namespace MauiDemoApp
{
    /// <summary>
    /// Bespoke, on-theme showcase page for the Mask R-CNN (DNN) object-detection
    /// demo. Shows an input photo (curated sample or user-picked), runs Mask R-CNN
    /// on the still image, and lists the objects it found. Real-time camera
    /// detection opens as a dedicated full-screen page.
    /// </summary>
    public class MaskRcnnPage : ContentPage
    {
        // ---------- Palette ----------
        internal static readonly Color PageBackground = Color.FromArgb("#EEF1F8");
        internal static readonly Color CardBackground = Colors.White;
        internal static readonly Color PrimaryText = Color.FromArgb("#1A1C2E");
        internal static readonly Color SecondaryText = Color.FromArgb("#8A8FA3");
        internal static readonly Color Accent = Color.FromArgb("#3D7BF7");
        internal static readonly Color RowBorder = Color.FromArgb("#ECEEF5");
        internal static readonly Color TileBackground = Color.FromArgb("#E8EFFE");
        internal static readonly Color ImageBackground = Color.FromArgb("#F2F2F7");

        internal const string BodyFont = "InterRegular";
        internal const string TitleFont = "InterSemiBold";
        internal const string IconFont = "MaterialSymbols";

        // ---------- Material Symbols glyphs ----------
        internal const string GlyphMask = "";        // theater_comedy
        internal const string GlyphInfo = "";        // info
        internal const string GlyphChevronLeft = ""; // chevron_left
        internal const string GlyphChevronRight = "";// chevron_right
        internal const string GlyphCheck = "";       // check_circle
        internal const string GlyphImage = "";       // image
        internal const string GlyphPlay = "";        // play_arrow
        internal const string GlyphVideo = "";       // videocam
        internal const string GlyphClose = "";       // close

        internal const string GlyphDog = "";    // pets
        internal const string GlyphWalk = "";   // directions_walk
        internal const string GlyphStop = "";   // report
        internal const string GlyphCar = "";    // directions_car
        internal const string GlyphCamera = ""; // photo_camera

        // Curated sample photos the model handles well.
        private static readonly (string File, string Name)[] Samples =
        {
            ("dog416.png", "Dog & bike"),
            ("pedestrian.png", "Street"),
            ("stop-sign.jpg", "Stop sign"),
            ("cars_license_plate.png", "Cars")
        };

        // Icon per sample (parallel to Samples), for the Change Photo sheet.
        private static readonly string[] SampleGlyphs = { GlyphDog, GlyphWalk, GlyphStop, GlyphCar };

        private readonly MaskRcnn _model = new MaskRcnn();

        private Mat _currentImage;
        private bool _detecting;
        private bool _defaultLoaded;
        private byte[] _lastResultPng;

        // UI we update at runtime.
        private readonly Image _previewImage;
        private readonly VerticalStackLayout _resultsRows;
        private readonly VerticalStackLayout _emptyState;
        private readonly Label _resultsCountLabel;
        private readonly Image _resultsCheck;
        private readonly Label _resultsTimeLabel;
        private readonly Button _detectButton;
        private readonly Grid _loadingOverlay;
        private readonly Label _loadingLabel;
        private readonly ActivityIndicator _loadingIndicator;

        // Change Photo bottom sheet (in-page overlay).
        private readonly Grid _sheetOverlay;
        private readonly BoxView _sheetScrim;
        private readonly Border _sheetCard;
        private readonly VerticalStackLayout _sheetList;
        private TaskCompletionSource<string> _sheetTcs;
        private bool _sheetAnimating;

        public MaskRcnnPage()
        {
            Title = "Mask R-CNN";
            BackgroundColor = PageBackground;
            Shell.SetNavBarIsVisible(this, false);

            // ---------- Header ----------
            var headerTile = new Border
            {
                WidthRequest = 64,
                HeightRequest = 64,
                BackgroundColor = CardBackground,
                Stroke = RowBorder,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(18) },
                VerticalOptions = LayoutOptions.Start,
                Content = MakeIcon(GlyphMask, Accent, 32)
            };
            var titleLabel = new Label { Text = "Mask R-CNN", FontFamily = TitleFont, FontSize = 30, TextColor = PrimaryText };
            var subtitleLabel = new Label
            {
                Text = "Find, label and outline objects in a photo using AI.",
                FontFamily = BodyFont,
                FontSize = 15,
                TextColor = SecondaryText,
                Margin = new Thickness(0, 2, 0, 0)
            };
            var titleStack = new VerticalStackLayout { VerticalOptions = LayoutOptions.Center, Children = { titleLabel, subtitleLabel } };

            var backButton = CircleButton(GlyphChevronLeft, async () => await Navigation.PopAsync());
            var infoButton = CircleButton(GlyphInfo, async () => await Navigation.PushAsync(new AboutPage()));

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

            // ---------- Input Image card ----------
            var inputTitle = new Label { Text = "Input Image", FontFamily = TitleFont, FontSize = 19, TextColor = PrimaryText, VerticalOptions = LayoutOptions.Center };
            var changePhoto = PillButton(GlyphImage, "Change Photo", OnChangePhoto);
            var inputHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            inputHeader.Add(inputTitle, 0, 0);
            inputHeader.Add(changePhoto, 1, 0);

            _previewImage = new Image { Aspect = Aspect.AspectFit, HeightRequest = 340 };
            var previewFrame = new Border
            {
                BackgroundColor = ImageBackground,
                Stroke = RowBorder,
                StrokeThickness = 1,
                Padding = new Thickness(10),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
                Content = _previewImage
            };
            // Tap the (annotated) image to view it full-screen.
            var previewTap = new TapGestureRecognizer();
            previewTap.Tapped += async (s, e) =>
            {
                if (_lastResultPng != null)
                    await Navigation.PushModalAsync(new FullScreenImagePage(_lastResultPng, "Mask R-CNN"));
            };
            previewFrame.GestureRecognizers.Add(previewTap);

            var inputCard = new Border
            {
                BackgroundColor = CardBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                Padding = new Thickness(16),
                Content = new VerticalStackLayout { Spacing = 14, Children = { inputHeader, previewFrame } }
            };

            // ---------- Buttons ----------
            _detectButton = new Button
            {
                Text = "Detect Image",
                FontFamily = TitleFont,
                FontSize = 17,
                BackgroundColor = Accent,
                TextColor = Colors.White,
                CornerRadius = 16,
                HeightRequest = 56,
                ImageSource = new FontImageSource { FontFamily = IconFont, Glyph = GlyphPlay, Color = Colors.White, Size = 20 },
                ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 8)
            };
            _detectButton.Clicked += OnDetectClicked;

            var liveButton = new Button
            {
                Text = "Live Detection",
                FontFamily = TitleFont,
                FontSize = 16,
                BackgroundColor = CardBackground,
                TextColor = Accent,
                BorderColor = Accent,
                BorderWidth = 1,
                CornerRadius = 16,
                HeightRequest = 52,
                ImageSource = new FontImageSource { FontFamily = IconFont, Glyph = GlyphVideo, Color = Accent, Size = 20 },
                ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 8)
            };
            liveButton.Clicked += OnLiveDetection;

            var buttonStack = new VerticalStackLayout { Spacing = 12, Children = { _detectButton, liveButton } };

            // ---------- Results card ----------
            var resultsTitle = new Label { Text = "Results", FontFamily = TitleFont, FontSize = 19, TextColor = PrimaryText, VerticalOptions = LayoutOptions.Center };

            _resultsCountLabel = new Label { Text = "0 objects", FontFamily = BodyFont, FontSize = 14, TextColor = SecondaryText, VerticalOptions = LayoutOptions.Center };
            _resultsCheck = MakeIcon(GlyphCheck, Color.FromArgb("#2BA84A"), 18);
            _resultsCheck.IsVisible = false;
            _resultsTimeLabel = new Label { Text = "", FontFamily = BodyFont, FontSize = 12, TextColor = SecondaryText, IsVisible = false, HorizontalTextAlignment = TextAlignment.End };
            var countRow = new HorizontalStackLayout { Spacing = 6, HorizontalOptions = LayoutOptions.End, Children = { _resultsCountLabel, _resultsCheck } };
            var statusStack = new VerticalStackLayout { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, Children = { countRow, _resultsTimeLabel } };

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
                    MakeIcon(GlyphImage, Color.FromArgb("#C2C7D6"), 40),
                    new Label { Text = "No detections yet", FontFamily = TitleFont, FontSize = 15, TextColor = PrimaryText, HorizontalTextAlignment = TextAlignment.Center },
                    new Label { Text = "Tap “Detect Image” to analyze.", FontFamily = BodyFont, FontSize = 13, TextColor = SecondaryText, HorizontalTextAlignment = TextAlignment.Center }
                }
            };

            _resultsRows = new VerticalStackLayout { Spacing = 0 };

            var resultsCard = new Border
            {
                BackgroundColor = CardBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                Padding = new Thickness(16),
                Content = new VerticalStackLayout { Spacing = 12, Children = { resultsHeader, _emptyState, _resultsRows } }
            };

            var content = new VerticalStackLayout
            {
                Spacing = 18,
                Padding = new Thickness(20, 16, 20, 28),
                Children = { header, inputCard, buttonStack, resultsCard }
            };

            // ---------- Loading overlay ----------
            _loadingIndicator = new ActivityIndicator { IsRunning = false, Color = Accent, WidthRequest = 44, HeightRequest = 44, HorizontalOptions = LayoutOptions.Center };
            _loadingLabel = new Label { Text = "Working…", FontFamily = BodyFont, FontSize = 15, TextColor = PrimaryText, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            var loadingCard = new Border
            {
                BackgroundColor = CardBackground,
                Stroke = RowBorder,
                StrokeThickness = 1,
                Padding = new Thickness(28, 22),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = new VerticalStackLayout { Spacing = 14, WidthRequest = 240, Children = { _loadingIndicator, _loadingLabel } }
            };
            _loadingOverlay = new Grid { IsVisible = false, BackgroundColor = Color.FromArgb("#B3EEF1F8"), Children = { loadingCard } };

            // ---------- Change Photo sheet overlay (covers the whole page) ----------
            _sheetList = new VerticalStackLayout { Spacing = 0 };
            _sheetCard = new Border
            {
                BackgroundColor = CardBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(28, 28, 0, 0) },
                // Negative bottom margin bleeds the card past the home-indicator
                // safe area so the white reaches the screen edge; the extra bottom
                // padding keeps Cancel above the home indicator.
                Padding = new Thickness(20, 8, 20, 60),
                Margin = new Thickness(0, 0, 0, -40),
                VerticalOptions = LayoutOptions.End,
                Content = _sheetList
            };
            // Bleed the scrim past the top/bottom safe areas so the whole screen dims.
            _sheetScrim = new BoxView { Color = Color.FromArgb("#66000000"), Opacity = 0, Margin = new Thickness(0, -120, 0, -120) };
            var sheetScrimTap = new TapGestureRecognizer();
            sheetScrimTap.Tapped += (s, e) => CloseSheet(null);
            _sheetScrim.GestureRecognizers.Add(sheetScrimTap);
            _sheetOverlay = new Grid { IsVisible = false, Children = { _sheetScrim, _sheetCard } };
            BuildSheet();

            Content = new Grid { Children = { new Microsoft.Maui.Controls.ScrollView { Content = content }, _loadingOverlay, _sheetOverlay } };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!_defaultLoaded)
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
                _model.Dispose();
            }
        }

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
                SetCurrentImage(Downscale(m, 1024));
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
            _sheetList.Children.Add(new Label { Text = "Change Photo", FontFamily = TitleFont, FontSize = 20, TextColor = PrimaryText, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 0, 6) });

            _sheetList.Children.Add(SheetSection("SAMPLE IMAGES"));
            for (int i = 0; i < Samples.Length; i++)
                _sheetList.Children.Add(SheetRow(SampleGlyphs[i], Samples[i].Name, $"sample:{i}", i < Samples.Length - 1));

            _sheetList.Children.Add(SheetSection("YOUR PHOTOS"));
            bool cam = MediaPicker.Default.IsCaptureSupported;
            _sheetList.Children.Add(SheetRow(GlyphImage, "Photo Library", "library", cam));
            if (cam)
                _sheetList.Children.Add(SheetRow(GlyphCamera, "Take Photo", "camera", false));

            var cancelLabel = new Label { Text = "Cancel", FontFamily = TitleFont, FontSize = 17, TextColor = Accent, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            var cancel = new Border { BackgroundColor = Color.FromArgb("#F2F2F7"), Stroke = Colors.Transparent, StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) }, HeightRequest = 52, Margin = new Thickness(0, 14, 0, 0), Content = cancelLabel };
            var cancelTap = new TapGestureRecognizer();
            cancelTap.Tapped += (s, e) => CloseSheet(null);
            cancel.GestureRecognizers.Add(cancelTap);
            _sheetList.Children.Add(cancel);
        }

        private static View SheetSection(string text) => new Label
        {
            Text = text,
            FontFamily = TitleFont,
            FontSize = 12,
            TextColor = SecondaryText,
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
            grid.Add(MakeIcon(glyph, Accent, 26), 0, 0);
            grid.Add(new Label { Text = text, FontFamily = BodyFont, FontSize = 17, TextColor = PrimaryText, VerticalOptions = LayoutOptions.Center }, 1, 0);
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => CloseSheet(value);
            grid.GestureRecognizers.Add(tap);

            var stack = new VerticalStackLayout();
            stack.Children.Add(grid);
            if (divider)
                stack.Children.Add(new BoxView { HeightRequest = 1, Color = RowBorder, Margin = new Thickness(42, 0, 0, 0) });
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
                byte[] bytes = await ReadAppFileAsync(Samples[idx].File);
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
            _previewImage.Source = MatToImageSource(image);
            ResetResults();
        }

        private void ResetResults()
        {
            _resultsRows.Children.Clear();
            _emptyState.IsVisible = true;
            _resultsCountLabel.Text = "0 objects";
            _resultsCountLabel.TextColor = SecondaryText;
            _resultsCheck.IsVisible = false;
            _resultsTimeLabel.IsVisible = false;
            _lastResultPng = null;
        }

        // ---------- Detection ----------

        private async void OnDetectClicked(object sender, EventArgs e)
        {
            if (_detecting || _currentImage == null)
                return;

            _detecting = true;
            _detectButton.IsEnabled = false;
            SetBusy(true, "Preparing…");

            try
            {
                if (!_model.Initialized)
                {
                    SetBusy(true, "Downloading AI model…\n(first time only)");
                    await Task.Run(() => _model.Init(OnDownloadProgress, null));
                    if (!_model.Initialized)
                    {
                        ShowError("Could not download or initialize the model. Check your connection and try again.");
                        return;
                    }
                }

                SetBusy(true, "Detecting objects…");
                Mat input = _currentImage;
                var sw = Stopwatch.StartNew();
                (byte[] png, List<Found> found) = await Task.Run(() => RunDetect(input, _model));
                sw.Stop();

                _lastResultPng = png;
                _previewImage.Source = MatBytesToImageSource(png);
                _emptyState.IsVisible = false;

                _resultsRows.Children.Clear();
                for (int i = 0; i < found.Count; i++)
                    _resultsRows.Children.Add(BuildObjectRow(found[i], i == found.Count - 1));

                _resultsCountLabel.Text = found.Count == 1 ? "1 object" : $"{found.Count} objects";
                _resultsCountLabel.TextColor = PrimaryText;
                _resultsCheck.IsVisible = found.Count > 0;
                _resultsTimeLabel.Text = $"Detected in {sw.ElapsedMilliseconds} ms";
                _resultsTimeLabel.IsVisible = true;

                if (found.Count == 0)
                {
                    _emptyState.IsVisible = true;
                    ((Label)_emptyState.Children[1]).Text = "No objects found";
                    ((Label)_emptyState.Children[2]).Text = "Try a different photo.";
                }
            }
            catch (Exception ex)
            {
                ShowError("Detection failed: " + ex.Message);
            }
            finally
            {
                _detecting = false;
                _detectButton.IsEnabled = true;
                SetBusy(false);
            }
        }

        private async void OnLiveDetection(object sender, EventArgs e)
        {
#if __IOS__ || __MACCATALYST__
            await Navigation.PushModalAsync(new MaskRcnnLivePage());
#else
            // Other platforms: reuse the proven full-featured camera detector.
            await Navigation.PushAsync(new Emgu.CV.Platform.Maui.UI.ProcessAndRenderPage(
                new MaskRcnn(), "Mask R-CNN (Live)", null, "Real-time object detection from the camera."));
#endif
        }

        // Runs on a background thread: detect, draw boxes/labels/masks, and
        // collect the found list. Shared by the still page and live page.
        internal static (byte[] png, List<Found> found) RunDetect(Mat input, MaskRcnn model)
        {
            using Mat annotated = input.Clone();
            var found = new List<Found>();

            MaskedObject[] objects = model.Detect(input);
            foreach (MaskedObject o in objects)
            {
                MCvScalar color = model.RenderColorMask[o.ClassId];
                o.Render(annotated, model.RenderColorRectangle, color);
                found.Add(new Found(o.Label, o.Confident, color));
                o.Dispose();
            }
            return (Encode(annotated), found);
        }

        private void OnDownloadProgress(long? totalBytesToReceive, long bytesReceived, double? progressPercentage)
        {
            string msg = totalBytesToReceive != null
                ? $"Downloading AI model…\n{bytesReceived / (1024 * 1024)} of {totalBytesToReceive.Value / (1024 * 1024)} MB ({(int)(progressPercentage ?? 0)}%)"
                : $"Downloading AI model…\n{bytesReceived / (1024 * 1024)} MB";
            MainThread.BeginInvokeOnMainThread(() => _loadingLabel.Text = msg);
        }

        private void ShowError(string message)
        {
            _resultsRows.Children.Clear();
            _emptyState.IsVisible = true;
            ((Label)_emptyState.Children[1]).Text = "Something went wrong";
            ((Label)_emptyState.Children[2]).Text = message;
        }

        private void SetBusy(bool busy, string message = "Working…")
        {
            _loadingLabel.Text = message;
            _loadingOverlay.IsVisible = busy;
            _loadingIndicator.IsRunning = busy;
        }

        // A detected object for the results list.
        internal readonly struct Found
        {
            public readonly string Label;
            public readonly double Confident;
            public readonly MCvScalar Color;
            public Found(string label, double confident, MCvScalar color)
            {
                Label = label; Confident = confident; Color = color;
            }
        }

        private View BuildObjectRow(Found f, bool isLast)
        {
            var dot = new Border
            {
                WidthRequest = 14,
                HeightRequest = 14,
                BackgroundColor = Color.FromRgb((int)f.Color.V2, (int)f.Color.V1, (int)f.Color.V0),
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                VerticalOptions = LayoutOptions.Center
            };
            var nameLabel = new Label { Text = Capitalize(f.Label), FontFamily = BodyFont, FontSize = 16, TextColor = PrimaryText, VerticalOptions = LayoutOptions.Center };
            var confLabel = new Label { Text = $"{(int)Math.Round(f.Confident * 100)}%", FontFamily = TitleFont, FontSize = 15, TextColor = PrimaryText, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center };
            var chevron = MakeIcon(GlyphChevronRight, Color.FromArgb("#C2C7D6"), 20);

            var grid = new Grid
            {
                Padding = new Thickness(2, 14),
                ColumnSpacing = 12,
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Auto)
                }
            };
            grid.Add(dot, 0, 0);
            grid.Add(nameLabel, 1, 0);
            grid.Add(confLabel, 2, 0);
            grid.Add(chevron, 3, 0);

            var stack = new VerticalStackLayout();
            stack.Children.Add(grid);
            if (!isLast)
                stack.Children.Add(new BoxView { HeightRequest = 1, Color = RowBorder });
            return stack;
        }

        // ---------- Helpers ----------

        internal static string Capitalize(string s) =>
            string.IsNullOrEmpty(s) ? s : char.ToUpperInvariant(s[0]) + s.Substring(1);

        internal static async Task<byte[]> ReadAppFileAsync(string fileName)
        {
            using Stream stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using MemoryStream ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return ms.ToArray();
        }

        internal static Mat Downscale(Mat src, int maxDim)
        {
            int longest = Math.Max(src.Width, src.Height);
            if (longest <= maxDim)
                return src;
            double scale = (double)maxDim / longest;
            Mat dst = new Mat();
            CvInvoke.Resize(src, dst, new DrawSize((int)(src.Width * scale), (int)(src.Height * scale)));
            src.Dispose();
            return dst;
        }

        internal static Image MakeIcon(string glyph, Color color, double size) => new Image
        {
            Source = new FontImageSource { FontFamily = IconFont, Glyph = glyph, Color = color, Size = size },
            WidthRequest = size,
            HeightRequest = size,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };

        private Border CircleButton(string glyph, Action onTap)
        {
            var cb = new Border
            {
                WidthRequest = 44,
                HeightRequest = 44,
                BackgroundColor = CardBackground,
                Stroke = RowBorder,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                VerticalOptions = LayoutOptions.Center,
                Content = MakeIcon(glyph, PrimaryText, 22)
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
                BackgroundColor = TileBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(13) },
                Padding = new Thickness(14, 9),
                VerticalOptions = LayoutOptions.Center,
                Content = new HorizontalStackLayout
                {
                    Spacing = 7,
                    Children =
                    {
                        MakeIcon(glyph, Accent, 18),
                        new Label { Text = text, FontFamily = TitleFont, FontSize = 14, TextColor = Accent, VerticalOptions = LayoutOptions.Center }
                    }
                }
            };
            var t = new TapGestureRecognizer();
            t.Tapped += (s, e) => onTap(s, e);
            pill.GestureRecognizers.Add(t);
            return pill;
        }

        // ---------- Mat <-> ImageSource ----------

        internal static byte[] Encode(Mat m)
        {
            using VectorOfByte buf = new VectorOfByte();
            CvInvoke.Imencode(".png", m, buf);
            return buf.ToArray();
        }

        internal static ImageSource MatToImageSource(Mat m) => MatBytesToImageSource(Encode(m));

        internal static ImageSource MatBytesToImageSource(byte[] png) =>
            ImageSource.FromStream(() => new MemoryStream(png));
    }

#if __IOS__ || __MACCATALYST__
    /// <summary>
    /// Full-screen, real-time Mask R-CNN detection from the camera. Uses the iOS
    /// AVCaptureSession camera pipeline, draws boxes/labels/masks on each frame,
    /// and shows the annotated frame full-screen with an Exit button and a
    /// "Scanning" pill. Starts the camera automatically on appearing.
    /// </summary>
    internal class MaskRcnnLivePage : Emgu.Util.AvCaptureSessionPage
    {
        private readonly MaskRcnn _model = new MaskRcnn();
        private readonly Image _feed;
        private readonly Grid _overlay;
        private readonly Label _loadingLabel;
        private readonly ActivityIndicator _spinner;
        private bool _running;
        private bool _busy;
        private bool _started;
        private Mat _frame;

        public MaskRcnnLivePage()
        {
            BackgroundColor = MaskRcnnPage.PageBackground;
            On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            AllowAvCaptureSession = true;
            outputRecorder.BufferReceived += OnCameraFrame;

            _feed = new Image { Aspect = Aspect.AspectFill, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill };

            // ---------- Top bar: Exit (left) / title (centered) / info (right) ----------
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

            var titleLabel = new Label { Text = "Live Detection", FontFamily = MaskRcnnPage.TitleFont, FontSize = 18, TextColor = MaskRcnnPage.PrimaryText, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };

            var infoBtn = new Border
            {
                WidthRequest = 40,
                HeightRequest = 40,
                BackgroundColor = MaskRcnnPage.CardBackground,
                Stroke = MaskRcnnPage.RowBorder,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Content = MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphInfo, MaskRcnnPage.PrimaryText, 20)
            };
            var infoTap = new TapGestureRecognizer();
            infoTap.Tapped += async (s, e) => await DisplayAlert("Live Detection", "Point your camera at objects. Each detected object is boxed and labeled in real time.", "OK");
            infoBtn.GestureRecognizers.Add(infoTap);

            var topBar = new Grid
            {
                Padding = new Thickness(16, 8, 16, 10),
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
            };
            topBar.Add(titleLabel, 0, 0);
            Grid.SetColumnSpan(titleLabel, 3);
            topBar.Add(exitRow, 0, 0);
            topBar.Add(infoBtn, 2, 0);

            // ---------- Scanning pill (overlaid on the camera) ----------
            var dot = new Border { WidthRequest = 10, HeightRequest = 10, BackgroundColor = Color.FromArgb("#2BE07A"), Stroke = Colors.Transparent, StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(5) }, VerticalOptions = LayoutOptions.Center };
            var scanningTitle = new Label { Text = "Scanning…", FontFamily = MaskRcnnPage.TitleFont, FontSize = 15, TextColor = Colors.White };
            var scanningSub = new Label { Text = "Point your camera at objects", FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = Color.FromArgb("#D6D6DE"), HorizontalOptions = LayoutOptions.Center };
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

            // ---------- Camera card (rounded, white margins) ----------
            var feedCard = new Border
            {
                BackgroundColor = Colors.Black,
                Stroke = MaskRcnnPage.RowBorder,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(24) },
                Margin = new Thickness(16, 4, 16, 20),
                Content = new Grid { Children = { _feed, pill } }
            };

            // ---------- Loading overlay (model download) ----------
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

            SetOverlay(true, "Downloading AI model…\n(first time only)");
            await Task.Run(() => _model.Init(OnDownloadProgress, null));
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

            // Dispose native resources only after any in-flight frame has
            // finished processing, to avoid crashing the capture thread.
            Mat frame = _frame;
            _frame = null;
            Task.Run(async () =>
            {
                for (int i = 0; i < 40 && _busy; i++)
                    await Task.Delay(100);
                try { frame?.Dispose(); } catch { }
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

                // The camera delivers landscape frames; rotate to match the
                // phone held upright (portrait).
                CvInvoke.Rotate(_frame, _frame, RotateFlags.Rotate90Clockwise);

                (byte[] png, List<MaskRcnnPage.Found> found) = MaskRcnnPage.RunDetect(_frame, _model);
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
                ? $"Downloading AI model…\n{bytesReceived / (1024 * 1024)} of {totalBytesToReceive.Value / (1024 * 1024)} MB ({(int)(progressPercentage ?? 0)}%)"
                : $"Downloading AI model…\n{bytesReceived / (1024 * 1024)} MB";
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
