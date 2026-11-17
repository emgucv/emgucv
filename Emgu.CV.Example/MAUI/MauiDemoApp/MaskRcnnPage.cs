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
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Storage;

using DrawRect = System.Drawing.Rectangle;
using DrawSize = System.Drawing.Size;

namespace MauiDemoApp
{
    /// <summary>
    /// Mask R-CNN instance segmentation on the shared <see cref="StagePage"/>
    /// layout: each object's mask is tinted and outlined on the stage image, its
    /// box and label are drawn live over it, and the panel lists every object with
    /// its confidence. Real-time camera detection opens as a dedicated page.
    ///
    /// This class also hosts palette, glyph and Mat / ImageSource helpers that the
    /// rest of the app uses.
    /// </summary>
    public class MaskRcnnPage : StagePage
    {
        // ---------- Palette (from DemoTheme) ----------
        internal static Color PageBackground => DemoTheme.PageBackground;
        internal static Color CardBackground => DemoTheme.CardBackground;
        internal static Color PrimaryText => DemoTheme.PrimaryText;
        internal static Color SecondaryText => DemoTheme.SecondaryText;
        internal static Color Accent => DemoTheme.Accent;
        internal static Color RowBorder => DemoTheme.Hairline;
        internal static Color TileBackground => DemoTheme.PillBackground;
        internal static Color ImageBackground => DemoTheme.ImageBackground;

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

        private const string ModelName = "R-50-FPN";

        private MaskRcnn _model = new MaskRcnn();

        public MaskRcnnPage() : base(new StageConfig
        {
            Title = "Mask R-CNN",
            Tag = "DNN",
            Category = "Detection",
            Module = "dnn module",
            Description = "Find, label and outline every object in a photo.",
            About = "Instance segmentation: for every object it finds, Mask R-CNN gives a label, a confidence score, a bounding box and a pixel mask tracing the object's exact outline. Two people standing side by side get two separate masks.\n\n" +
                    "This is the ResNet-50 FPN model from the ONNX Model Zoo, trained on the 80 everyday COCO classes (people, animals, vehicles, household items). It runs on OpenCV's DNN module and downloads once, the first time you use it.\n\n" +
                    "It is far more detailed than a box-only detector such as YOLO, but also much slower, so the live camera view updates less smoothly.",
            Samples = new[]
            {
                new StageSample("dog416.png", "Dog & bike"),
                new StageSample("pedestrian.png", "Street"),
                new StageSample("stop-sign.jpg", "Stop sign"),
                new StageSample("cars_license_plate.png", "Cars"),
            },
            HasCamera = true,
            RunLabel = "Detect",
            EmptyTitle = "no detections yet",
            EmptyCaption = "run a detection to see objects here",
            MaxImageSize = 1024,
        })
        {
        }

        protected override string BackendName => "CPU · OpenCV DNN";

        protected override void OnStatusInit() => UpdateStatus();

        // Local file the model downloads to (same folder / name MaskRcnn.Init uses).
        private static string ModelFilePath => System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "emgu", "mask_rcnn_onnx", "MaskRCNN-10.onnx");

        private void UpdateStatus()
        {
            SetOptionsValue(ModelName);
            if (_model != null && _model.Initialized)
            {
                SetStatus("Loaded");
                return;
            }
            var fi = new FileInfo(ModelFilePath);
            if (fi.Exists)
                SetStatus($"Downloaded · {fi.Length / (1024.0 * 1024.0):0.0} MB");
            else
                SetStatus("Not downloaded", DemoTheme.MutedText, showDownload: true);
        }

        protected override async Task<bool> PrepareAsync()
        {
            if (_model == null)
                return false;
            if (_model.Initialized)
                return true;
            SetStatus("Loading model…", DemoTheme.Lime);
            await Task.Run(() => _model.Init(OnDownloadProgress, null));
            if (!_model.Initialized)
            {
                SetStatus("Download failed · check your connection", DemoTheme.MutedText, showDownload: true);
                return false;
            }
            UpdateStatus();
            return true;
        }

        protected override StageResult RunCore(Mat input)
        {
            var total = Stopwatch.StartNew();
            // "pre": a 3-channel BGR copy to draw the masks on.
            using var canvas = new Mat();
            if (input.NumberOfChannels == 4)
                CvInvoke.CvtColor(input, canvas, ColorConversion.Bgra2Bgr);
            else
                input.CopyTo(canvas);

            var infer = Stopwatch.StartNew();
            MaskedObject[] objects = _model.Detect(canvas);
            infer.Stop();

            // "post": tint and outline each mask, then collect boxes and rows.
            objects = objects.OrderByDescending(o => o.Confident).ToArray();
            var boxes = new List<StageBox>();
            var rows = new List<StageRow>();
            foreach (MaskedObject o in objects)
            {
                Color color = ClassColor(o.ClassId);
                DrawMask(canvas, o, new MCvScalar(color.Blue * 255, color.Green * 255, color.Red * 255));
                boxes.Add(new StageBox(new RectF(o.Region.X, o.Region.Y, o.Region.Width, o.Region.Height), $"{o.Label} {o.Confident:0.00}", color, o.Confident));
                rows.Add(ScoreRow(o.Label, o.Confident, color));
                o.Dispose();
            }
            byte[] png = Encode(canvas);
            total.Stop();

            return new StageResult
            {
                OutputPng = png,
                OutputWidth = canvas.Width,
                OutputHeight = canvas.Height,
                Boxes = boxes.ToArray(),
                Rows = rows.ToArray(),
                InferenceMs = infer.Elapsed.TotalMilliseconds,
                PrePostMs = total.Elapsed.TotalMilliseconds - infer.Elapsed.TotalMilliseconds,
            };
        }

        // Tint the object's pixels (mask probability above 0.5) with its color and
        // trace the mask's outline. The region is already clipped to the image.
        private static void DrawMask(Mat image, MaskedObject o, MCvScalar color)
        {
            DrawRect r = o.Region;
            if (r.Width <= 0 || r.Height <= 0)
                return;
            using Mat roi = new Mat(image, r);
            using Mat prob = new Mat();
            CvInvoke.Resize(o.Mask, prob, r.Size);
            using Mat bin = new Mat();
            CvInvoke.Threshold(prob, bin, 0.5, 255, ThresholdType.Binary);
            using Mat mask = new Mat();
            bin.ConvertTo(mask, DepthType.Cv8U);

            using Mat tint = new Mat(r.Size, DepthType.Cv8U, 3);
            tint.SetTo(color);
            using Mat blended = new Mat();
            CvInvoke.AddWeighted(roi, 0.5, tint, 0.5, 0, blended);
            blended.CopyTo(roi, mask);

            using var contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(mask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple, r.Location);
            CvInvoke.DrawContours(image, contours, -1, color, 2, LineType.AntiAlias);
        }

        protected override async Task OpenLiveCameraAsync()
        {
#if __IOS__ || __MACCATALYST__
            await Navigation.PushModalAsync(new MaskRcnnLivePage());
#else
            // Other platforms: reuse the full-featured camera detector.
            await Navigation.PushAsync(new Emgu.CV.Platform.Maui.UI.ProcessAndRenderPage(
                new MaskRcnn(), "Mask R-CNN (Live)", null, "Real-time object detection from the camera."));
#endif
        }

        protected override string CodeSnippet => @"using Emgu.CV;
using Emgu.CV.Models;
using Emgu.CV.Structure;

using var maskRcnn = new MaskRcnn();
await maskRcnn.Init();   // downloads the model once

using Mat image = CvInvoke.Imread(""dog.png"");
MaskedObject[] objects = maskRcnn.Detect(image,
    matchScoreThreshold: 0.5f, nmsThreshold: 0.4f);

foreach (MaskedObject o in objects)
{
    // o.Mask is a small probability map for o.Region
    Console.WriteLine($""{o.Label} {o.Confident:0.00} at {o.Region}"");
    o.Render(image, new MCvScalar(0, 0, 255),
        maskRcnn.RenderColorMask[o.ClassId]);
    o.Dispose();
}";

        protected override void OnClosed()
        {
            _model?.Dispose();
            _model = null;
        }

        /// <summary>
        /// Detect objects and draw them onto a copy of the input. The caller owns the returned Mat.
        /// Used by the live camera page.
        /// </summary>
        internal static (Mat annotated, List<Found> found) RenderDetections(Mat input, MaskRcnn model)
        {
            Mat annotated = input.Clone();
            var found = new List<Found>();

            MaskedObject[] objects = model.Detect(input);
            foreach (MaskedObject o in objects)
            {
                MCvScalar color = model.RenderColorMask[o.ClassId];
                o.Render(annotated, model.RenderColorRectangle, color);
                found.Add(new Found(o.Label, o.Confident, color));
                o.Dispose();
            }
            return (annotated, found);
        }

        // A detected object, as returned by RenderDetections.
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

        // ---------- Helpers ----------
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

        // ---------- Mat <-> ImageSource ----------

        internal static byte[] Encode(Mat m)
        {
            using VectorOfByte buf = new VectorOfByte();
            CvInvoke.Imencode(".png", m, buf);
            return buf.ToArray();
        }

        internal static ImageSource MatToImageSource(Mat m) => MatBytesToImageSource(Encode(m));

#if __IOS__ || __MACCATALYST__
        /// <summary>
        /// Show a live camera frame by handing a UIImage straight to the native UIImageView,
        /// avoiding a per-frame PNG encode/decode. The view's Aspect still applies, because the
        /// MAUI image handler maps it to the UIImageView's ContentMode.
        /// </summary>
        internal static void ShowLiveFrame(Image view, Mat frame)
        {
            UIKit.UIImage image = frame.ToUIImage();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (view.Handler?.PlatformView is UIKit.UIImageView imageView)
                {
                    UIKit.UIImage previous = imageView.Image;
                    imageView.Image = image;
                    previous?.Dispose();
                }
                else
                {
                    image.Dispose();
                }
            });
        }
#endif

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
            this.SafeAreaEdges = Microsoft.Maui.SafeAreaEdges.All;
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
            infoTap.Tapped += async (s, e) => await DisplayAlertAsync("Live Detection", "Point your camera at objects. Each detected object is boxed and labeled in real time.", "OK");
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
            var scanningSub = new Label { Text = "Point your camera at objects", FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = Color.FromArgb("#D0D0D0"), HorizontalOptions = LayoutOptions.Center };
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

            _overlay = new Grid { BackgroundColor = DemoTheme.LoadingVeilStrong, Children = { loadingBox } };

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
                    pixelBuffer.ToArray(_frame, ImreadModes.ColorBgr);

                if (_frame.IsEmpty)
                    return;

                // The camera delivers landscape frames; rotate to match the
                // phone held upright (portrait).
                CvInvoke.Rotate(_frame, _frame, RotateFlags.Rotate90Clockwise);

                (Mat annotated, _) = MaskRcnnPage.RenderDetections(_frame, _model);
                using (annotated)
                {
                    if (_running)
                        MaskRcnnPage.ShowLiveFrame(_feed, annotated);
                }
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
