//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Linq;
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
    /// What remains of the old showcase page: the collapsible "About this module"
    /// card (still used by SimpleDemoPage / the licenses page). The demos
    /// themselves now use <see cref="StagePage"/>; the live camera page below is
    /// shared by them.
    /// </summary>
    internal static class ModelShowcasePage
    {
        // ---------- About this module (collapsible one-paragraph card) ----------
        // Static + self-contained (its own toggle state is captured in the tap
        // closure) so the bespoke pages (Mask R-CNN, Shape Detection) can reuse it.
        internal static View BuildAboutCard(string about, bool flush = false)
        {
            var titleLabel = new Label { Text = "ABOUT THIS MODULE", FontFamily = MaskRcnnPage.BodyFont, FontSize = 12, CharacterSpacing = 1, TextColor = MaskRcnnPage.SecondaryText, VerticalOptions = LayoutOptions.Center };
            var chevron = MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphChevronRight, MaskRcnnPage.SecondaryText, 22);

            var headerRow = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            headerRow.Add(titleLabel, 0, 0);
            headerRow.Add(chevron, 1, 0);

            var details = new VerticalStackLayout { IsVisible = false, Margin = new Thickness(0, 12, 0, 2) };
            details.Children.Add(new Label { Text = about, FontFamily = MaskRcnnPage.BodyFont, FontSize = 15, TextColor = MaskRcnnPage.SecondaryText });

            bool expanded = false;
            var tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) =>
            {
                expanded = !expanded;
                details.IsVisible = expanded;
                await chevron.RotateToAsync(expanded ? 90 : 0, 180, Easing.CubicOut);
            };
            headerRow.GestureRecognizers.Add(tap);

            return new Border
            {
                BackgroundColor = MaskRcnnPage.CardBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                Padding = flush ? new Thickness(0, 4) : new Thickness(16),
                Content = new VerticalStackLayout { Children = { headerRow, details } }
            };
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
        private readonly IMultiPaneModel _paneModel;
        private View _splitControls;
        private View _collapseControl;

        public ModelShowcaseLivePage(string title, Func<IProcessAndRenderModel> modelFactory, string pickerSelection)
        {
            _model = modelFactory();
            _pickerSelection = pickerSelection;
            _paneModel = _model as IMultiPaneModel;

            BackgroundColor = MaskRcnnPage.PageBackground;
            this.SafeAreaEdges = Microsoft.Maui.SafeAreaEdges.All;
            AllowAvCaptureSession = true;
            outputRecorder.BufferReceived += OnCameraFrame;

            _feed = new Image { Aspect = Aspect.AspectFill, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill };

            // Floating exit control (white on a translucent pill) so the video
            // can run edge-to-edge with no chrome bar stealing vertical space.
            var exitRow = new HorizontalStackLayout
            {
                Spacing = 2,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Children =
                {
                    MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphChevronLeft, Colors.White, 24),
                    new Label { Text = "Exit", FontFamily = MaskRcnnPage.TitleFont, FontSize = 17, TextColor = Colors.White, VerticalOptions = LayoutOptions.Center }
                }
            };
            var exitPill = new Border
            {
                BackgroundColor = Color.FromArgb("#E0111111"),
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(18) },
                Padding = new Thickness(14, 8, 18, 8),
                Margin = new Thickness(12, 12, 0, 0),
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Content = exitRow
            };
            var exitTap = new TapGestureRecognizer();
            exitTap.Tapped += async (s, e) =>
            {
                _running = false;
                await Navigation.PopModalAsync();
            };
            exitPill.GestureRecognizers.Add(exitTap);

            var dot = new Border { WidthRequest = 10, HeightRequest = 10, BackgroundColor = Color.FromArgb("#2BE07A"), Stroke = Colors.Transparent, StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(5) }, VerticalOptions = LayoutOptions.Center };
            var scanningTitle = new Label { Text = "Scanning…", FontFamily = MaskRcnnPage.TitleFont, FontSize = 15, TextColor = Colors.White };
            var scanningSub = new Label { Text = "Point your camera at the subject", FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = Color.FromArgb("#D0D0D0"), HorizontalOptions = LayoutOptions.Center };
            var pill = new Border
            {
                BackgroundColor = Color.FromArgb("#E0111111"),
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

            _spinner = new ActivityIndicator { IsRunning = false, Color = MaskRcnnPage.Accent, WidthRequest = 44, HeightRequest = 44, HorizontalOptions = LayoutOptions.Center };
            _loadingLabel = new Label { Text = "Starting camera…", FontFamily = MaskRcnnPage.BodyFont, FontSize = 15, TextColor = MaskRcnnPage.PrimaryText, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            var loadingBox = new VerticalStackLayout { Spacing = 14, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, Children = { _spinner, _loadingLabel } };
            _overlay = new Grid { BackgroundColor = DemoTheme.LoadingVeilStrong, Children = { loadingBox } };

            // Edge-to-edge: the feed fills the whole page (theme background behind
            // it), with the exit control and floating pills on top — no chrome bar
            // or card margin stealing space from the video.
            var layers = new Grid { BackgroundColor = MaskRcnnPage.PageBackground };
            layers.Children.Add(_feed);
            layers.Children.Add(exitPill);
            layers.Children.Add(pill);
            // When the model exposes stacked panes (e.g. Video Surveillance),
            // overlay a per-pane expand control so either pane can go full-screen.
            if (_paneModel != null)
            {
                BuildPaneControls();
                layers.Children.Add(_splitControls);
                layers.Children.Add(_collapseControl);
            }
            layers.Children.Add(_overlay);
            Content = layers;
        }

        private void BuildPaneControls()
        {
            var names = _paneModel.PaneNames;

            // A translucent pill that pops a pane full-screen when tapped, laid
            // into row `index` of a 2-row grid so it sits over that pane.
            Func<int, string, View> expandPill = (index, name) =>
            {
                var row = new HorizontalStackLayout
                {
                    Spacing = 4,
                    Children =
                    {
                        new Label { Text = name, FontFamily = MaskRcnnPage.TitleFont, FontSize = 14, TextColor = Colors.White, VerticalOptions = LayoutOptions.Center },
                        MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphChevronRight, Colors.White, 18)
                    }
                };
                var border = new Border
                {
                    BackgroundColor = Color.FromArgb("#E0111111"),
                    Stroke = Colors.Transparent,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
                    Padding = new Thickness(12, 6, 10, 6),
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 12, 12, 0),
                    Content = row
                };
                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) => SetFocusedPane(index);
                border.GestureRecognizers.Add(tap);
                return border;
            };

            // The grid spans the whole screen to align a pill over each pane, but
            // must not swallow taps meant for the controls beneath it (e.g. Exit);
            // pass empty-area touches through while keeping the pills tappable.
            var splitGrid = new Grid
            {
                RowDefinitions = { new RowDefinition(GridLength.Star), new RowDefinition(GridLength.Star) },
                InputTransparent = true,
                CascadeInputTransparent = false
            };
            for (int i = 0; i < names.Length && i < 2; i++)
            {
                var p = expandPill(i, names[i]);
                Grid.SetRow(p, i);
                splitGrid.Children.Add(p);
            }
            _splitControls = splitGrid;

            var collapseRow = new HorizontalStackLayout
            {
                Spacing = 2,
                Children =
                {
                    MaskRcnnPage.MakeIcon(MaskRcnnPage.GlyphChevronLeft, Colors.White, 20),
                    new Label { Text = "Split", FontFamily = MaskRcnnPage.TitleFont, FontSize = 15, TextColor = Colors.White, VerticalOptions = LayoutOptions.Center }
                }
            };
            var collapseBorder = new Border
            {
                BackgroundColor = Color.FromArgb("#E0111111"),
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
                Padding = new Thickness(10, 6, 14, 6),
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 12, 12, 0),
                IsVisible = false,
                Content = collapseRow
            };
            var ctap = new TapGestureRecognizer();
            ctap.Tapped += (s, e) => SetFocusedPane(-1);
            collapseBorder.GestureRecognizers.Add(ctap);
            _collapseControl = collapseBorder;
        }

        private void SetFocusedPane(int index)
        {
            if (_paneModel == null)
                return;
            _paneModel.FocusedPane = index;
            // Split preview crops to fill; a focused single pane shows uncropped.
            _feed.Aspect = index < 0 ? Aspect.AspectFill : Aspect.AspectFit;
            _splitControls.IsVisible = index < 0;
            _collapseControl.IsVisible = index >= 0;
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
                    pixelBuffer.ToArray(_frame, ImreadModes.ColorBgr);

                if (_frame.IsEmpty)
                    return;

#if !__MACCATALYST__
                // An iPhone camera delivers landscape frames; rotate to match the
                // phone held upright (portrait). A Mac webcam is already upright.
                CvInvoke.Rotate(_frame, _frame, RotateFlags.Rotate90Clockwise);
#endif

                if (_renderMat == null)
                    _renderMat = new Mat();
                _frame.CopyTo(_renderMat);
                _model.ProcessAndRender(_frame, _renderMat);
                if (_running)
                    MaskRcnnPage.ShowLiveFrame(_feed, _renderMat);
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
