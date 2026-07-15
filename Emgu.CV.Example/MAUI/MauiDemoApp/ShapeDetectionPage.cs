//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Models;
using Emgu.CV.Structure;
using Emgu.CV.Util;

using Microsoft.Maui.Controls.Shapes;

using DrawColor = System.Drawing.Color;
using DrawPoint = System.Drawing.Point;
using DrawSize = System.Drawing.Size;

namespace MauiDemoApp
{
    /// <summary>
    /// Bespoke, on-theme showcase page for the classic (non-DNN) OpenCV shape
    /// detection demo. Generates a random scene of non-overlapping shapes,
    /// runs <see cref="ShapeDetector"/>, and presents each detection stage as a
    /// tappable result row.
    /// </summary>
    public class ShapeDetectionPage : ContentPage
    {
        // ---------- Palette (matches the home screen) ----------
        private static readonly Color PageBackground = Color.FromArgb("#EEF1F8");
        private static readonly Color CardBackground = Colors.White;
        private static readonly Color PrimaryText = Color.FromArgb("#1A1C2E");
        private static readonly Color SecondaryText = Color.FromArgb("#8A8FA3");
        private static readonly Color Accent = Color.FromArgb("#3D7BF7");
        private static readonly Color RowBorder = Color.FromArgb("#ECEEF5");
        private static readonly Color TileBackground = Color.FromArgb("#E8EFFE");
        private static readonly Color ImageBackground = Color.FromArgb("#F2F2F7");
        private static readonly Color CancelColor = Color.FromArgb("#E5484D");

        private const string BodyFont = "InterRegular";
        private const string TitleFont = "InterSemiBold";
        private const string IconFont = "MaterialSymbols";

        // ---------- Material Symbols glyphs ----------
        private const string GlyphShape = "";        // crop_free
        private const string GlyphInfo = "";         // info
        private const string GlyphChevronRight = ""; // chevron_right
        private const string GlyphChevronLeft = "";  // chevron_left
        private const string GlyphExpandMore = "";   // expand_more
        private const string GlyphCube = "";         // view_in_ar
        private const string GlyphImage = "";        // image
        private const string GlyphSettings = "";     // settings
        private const string GlyphCheckCircle = "";  // check_circle
        private const string GlyphClose = "";        // close
        private const string GlyphPlay = "";   // play_arrow

        private readonly ShapeDetector _detector = new ShapeDetector();
        private readonly Random _rng = new Random();

        // The current sample image, owned by this page.
        private Mat _currentImage;

        // Guard against overlapping detection runs (e.g. rapid Random taps).
        private bool _detecting;
        private bool _redetectPending;
        private bool _cancelRequested;

        // UI we need to update at runtime.
        private readonly Image _previewImage;
        private readonly VerticalStackLayout _resultsRows;
        private readonly Label _resultsHint;
        private readonly HorizontalStackLayout _statusStack;
        private readonly Label _statusLabel;
        private readonly Button _detectButton;
        private readonly Image _pipelineChevron;
        private readonly VerticalStackLayout _pipelineDetails;
        private readonly Grid _loadingOverlay;
        private readonly Label _loadingLabel;
        private readonly ActivityIndicator _loadingIndicator;

        public ShapeDetectionPage()
        {
            Title = "Shape Detection";
            BackgroundColor = PageBackground;
            // Draw our own header (back + info) to match the home screen.
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
                Content = MakeIcon(GlyphShape, Accent, 32)
            };

            var titleLabel = new Label { Text = "Shape Detection", FontFamily = TitleFont, FontSize = 30, TextColor = PrimaryText };
            var subtitleLabel = new Label
            {
                Text = "Detect circles, lines, triangles and rectangles using OpenCV.",
                FontFamily = BodyFont,
                FontSize = 15,
                TextColor = SecondaryText,
                Margin = new Thickness(0, 2, 0, 0)
            };
            var titleStack = new VerticalStackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = { titleLabel, subtitleLabel }
            };

            var backButton = CircleButton(GlyphChevronLeft, async () => await Navigation.PopAsync());
            var infoButton = CircleButton(GlyphInfo, async () => await Navigation.PushAsync(new AboutPage()));

            var topRow = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
            };
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

            // ---------- Sample Image card ----------
            var sampleTitle = new Label { Text = "Sample Image", FontFamily = TitleFont, FontSize = 19, TextColor = PrimaryText, VerticalOptions = LayoutOptions.Center };

            _previewImage = new Image { Aspect = Aspect.AspectFit, HeightRequest = 220 };
            var previewFrame = new Border
            {
                BackgroundColor = ImageBackground,
                Stroke = RowBorder,
                StrokeThickness = 1,
                Padding = new Thickness(10),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
                Content = _previewImage
            };

            var sampleCard = new Border
            {
                BackgroundColor = CardBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                Padding = new Thickness(16),
                Content = new VerticalStackLayout { Spacing = 14, Children = { sampleTitle, previewFrame } }
            };

            // ---------- Detect button ----------
            _detectButton = new Button
            {
                Text = "New Image",
                FontFamily = TitleFont,
                FontSize = 17,
                BackgroundColor = Accent,
                TextColor = Colors.White,
                CornerRadius = 16,
                HeightRequest = 56,
                ImageSource = new FontImageSource { FontFamily = IconFont, Glyph = "", Color = Colors.White, Size = 20 }, // play_arrow
                ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 8)
            };
            _detectButton.Clicked += OnDetectButtonClicked;

            // ---------- Results card ----------
            var resultsTitle = new Label { Text = "Results", FontFamily = TitleFont, FontSize = 19, TextColor = PrimaryText, VerticalOptions = LayoutOptions.Center };
            _statusLabel = new Label { FontFamily = BodyFont, FontSize = 14, TextColor = SecondaryText, VerticalOptions = LayoutOptions.Center };
            _statusStack = new HorizontalStackLayout
            {
                Spacing = 6,
                IsVisible = false,
                VerticalOptions = LayoutOptions.Center,
                Children = { _statusLabel, MakeIcon(GlyphCheckCircle, Color.FromArgb("#2BA84A"), 18) }
            };
            var resultsHeader = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            resultsHeader.Add(resultsTitle, 0, 0);
            resultsHeader.Add(_statusStack, 1, 0);

            _resultsHint = new Label
            {
                Text = "Tap “New Image” to generate a scene and detect shapes.",
                FontFamily = BodyFont,
                FontSize = 14,
                TextColor = SecondaryText,
                Margin = new Thickness(2, 6, 0, 0)
            };
            _resultsRows = new VerticalStackLayout { Spacing = 10 };

            var resultsCard = new Border
            {
                BackgroundColor = CardBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                Padding = new Thickness(16),
                Content = new VerticalStackLayout { Spacing = 12, Children = { resultsHeader, _resultsHint, _resultsRows } }
            };

            // ---------- Pipeline (collapsible) ----------
            _pipelineChevron = MakeIcon(GlyphExpandMore, SecondaryText, 22);
            _pipelineChevron.HorizontalOptions = LayoutOptions.End;

            var pipelineTitle = new Label { Text = "Pipeline", FontFamily = TitleFont, FontSize = 16, TextColor = PrimaryText };
            var pipelineSteps = new Label
            {
                Text = "Grayscale → Blur → Canny → Hough → Contours",
                FontFamily = BodyFont,
                FontSize = 13,
                TextColor = SecondaryText
            };
            var pipelineTile = new Border
            {
                WidthRequest = 42,
                HeightRequest = 42,
                BackgroundColor = TileBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(11) },
                VerticalOptions = LayoutOptions.Center,
                Content = MakeIcon(GlyphSettings, Accent, 22)
            };
            var pipelineHeader = new Grid
            {
                ColumnSpacing = 12,
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
            };
            pipelineHeader.Add(pipelineTile, 0, 0);
            pipelineHeader.Add(new VerticalStackLayout { VerticalOptions = LayoutOptions.Center, Children = { pipelineTitle, pipelineSteps } }, 1, 0);
            pipelineHeader.Add(_pipelineChevron, 2, 0);

            _pipelineDetails = new VerticalStackLayout
            {
                Spacing = 8,
                IsVisible = false,
                Margin = new Thickness(0, 12, 0, 0),
                Children =
                {
                    PipelineStep("Grayscale", "Convert the image to a single intensity channel."),
                    PipelineStep("Gaussian Blur", "Smooth the image to suppress noise."),
                    PipelineStep("Canny", "Detect edges from intensity gradients."),
                    PipelineStep("Hough Transform", "Find circles (HoughCircles) and line segments (HoughLinesP)."),
                    PipelineStep("Contours", "Approximate polygons to classify triangles and rectangles.")
                }
            };

            var pipelineCard = new Border
            {
                BackgroundColor = CardBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                Padding = new Thickness(16),
                Content = new VerticalStackLayout { Children = { pipelineHeader, _pipelineDetails } }
            };
            var pipelineTap = new TapGestureRecognizer();
            pipelineTap.Tapped += (s, e) =>
            {
                _pipelineDetails.IsVisible = !_pipelineDetails.IsVisible;
                ((FontImageSource)_pipelineChevron.Source).Glyph = _pipelineDetails.IsVisible ? GlyphChevronRight : GlyphExpandMore;
            };
            pipelineHeader.GestureRecognizers.Add(pipelineTap);

            var content = new VerticalStackLayout
            {
                Spacing = 18,
                Padding = new Thickness(20, 16, 20, 28),
                Children =
                {
                    header, sampleCard, _detectButton, resultsCard, pipelineCard,
                    ModelShowcasePage.BuildAboutCard("Finds geometric shapes — triangles, rectangles, circles — in an image using classic contour analysis (no AI model).")
                }
            };

            // ---------- Loading overlay (spinner shown while busy) ----------
            _loadingIndicator = new ActivityIndicator { IsRunning = false, Color = Accent, WidthRequest = 44, HeightRequest = 44, HorizontalOptions = LayoutOptions.Center };
            _loadingLabel = new Label { Text = "Detecting…", FontFamily = BodyFont, FontSize = 15, TextColor = PrimaryText, HorizontalOptions = LayoutOptions.Center };
            var loadingCard = new Border
            {
                BackgroundColor = CardBackground,
                Stroke = RowBorder,
                StrokeThickness = 1,
                Padding = new Thickness(28, 22),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = new VerticalStackLayout { Spacing = 14, Children = { _loadingIndicator, _loadingLabel } }
            };
            _loadingOverlay = new Grid
            {
                IsVisible = false,
                BackgroundColor = Color.FromArgb("#B3EEF1F8"),
                Children = { loadingCard }
            };

            Content = new Grid { Children = { new ScrollView { Content = content }, _loadingOverlay } };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Fresh random sample every time the page is opened.
            if (_currentImage == null)
                SetCurrentImage(GenerateRandomShapes());
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            base.OnNavigatedFrom(args);
            // Release native resources only when the page has actually been
            // popped — NOT when the full-screen viewer modal is presented over
            // it (disposing the detector here is what caused Detect to throw a
            // NullReferenceException after closing the viewer).
            if (!Navigation.NavigationStack.Contains(this) && !Navigation.ModalStack.Contains(this))
            {
                _currentImage?.Dispose();
                _currentImage = null;
                _detector.Dispose();
            }
        }

        // ---------- Sample image sources ----------

        private void SetCurrentImage(Mat image)
        {
            _currentImage?.Dispose();
            _currentImage = image;
            _previewImage.Source = MatToImageSource(image);

            // A new input automatically runs detection.
            _ = RunDetectionAsync();
        }

        private static readonly DrawColor[] ShapePalette =
        {
            DrawColor.RoyalBlue, DrawColor.SeaGreen, DrawColor.OrangeRed, DrawColor.MediumPurple,
            DrawColor.Goldenrod, DrawColor.Crimson, DrawColor.Teal, DrawColor.DarkOrange, DrawColor.HotPink
        };

        /// <summary>
        /// Generate a fresh random scene of shapes. Shapes are placed so they
        /// never overlap each other and are always fully inside the canvas (never
        /// clipped by an edge), which keeps every shape cleanly detectable. Their
        /// type, size, rotation, color and fill/outline style are randomized.
        /// </summary>
        private Mat GenerateRandomShapes()
        {
            const int w = 800, h = 560, margin = 28, gap = 16;
            Mat img = new Mat(new DrawSize(w, h), DepthType.Cv8U, 3);
            img.SetTo(new MCvScalar(255, 255, 255));

            int target = _rng.Next(5, 9); // 5–8 shapes
            var placed = new System.Collections.Generic.List<(int cx, int cy, int r)>();

            int attempts = 0;
            while (placed.Count < target && attempts < 400)
            {
                attempts++;
                int type = _rng.Next(5);
                int size = _rng.Next(34, 74);
                int r = BoundingRadius(type, size);

                // Keep the whole shape inside the canvas (with a margin) so it is
                // never cut off by an edge.
                int minX = margin + r, maxX = w - margin - r;
                int minY = margin + r, maxY = h - margin - r;
                if (maxX <= minX || maxY <= minY)
                    continue; // too big for this canvas — try another

                int cx = _rng.Next(minX, maxX);
                int cy = _rng.Next(minY, maxY);

                // Reject a position that would overlap an already-placed shape.
                bool overlaps = false;
                foreach (var p in placed)
                {
                    long dx = p.cx - cx, dy = p.cy - cy;
                    long minDist = p.r + r + gap;
                    if (dx * dx + dy * dy < minDist * minDist) { overlaps = true; break; }
                }
                if (overlaps)
                    continue;

                placed.Add((cx, cy, r));
                DrawShape(img, type, cx, cy, size);
            }
            return img;
        }

        // The maximum distance from a shape's center to any of its points — used
        // both to keep it inside the canvas and to space shapes apart.
        private static int BoundingRadius(int type, int size) => type switch
        {
            0 => (int)Math.Ceiling(size * 1.23), // axis-aligned rectangle (half-diagonal)
            1 => size,                           // circle
            2 => (int)Math.Ceiling(size * 1.20), // rotated rectangle (half-diagonal)
            3 => size,                           // triangle (circumradius)
            _ => size                            // line (half-length)
        };

        // Draw one randomly-styled shape of the given type, centered at (cx, cy).
        private void DrawShape(Mat img, int type, int cx, int cy, int size)
        {
            double angle = _rng.NextDouble() * Math.PI; // random rotation
            MCvScalar color = new Bgr(ShapePalette[_rng.Next(ShapePalette.Length)]).MCvScalar;
            // Mix filled and bold-outline styles for variety.
            int thickness = _rng.Next(3) == 0 ? _rng.Next(3, 6) : -1;

            switch (type)
            {
                case 0: // axis-aligned rectangle
                    CvInvoke.Rectangle(img,
                        new System.Drawing.Rectangle(cx - size, cy - (int)(size * 0.7), size * 2, (int)(size * 1.4)),
                        color, thickness);
                    break;
                case 1: // circle
                    CvInvoke.Circle(img, new DrawPoint(cx, cy), size, color, thickness);
                    break;
                case 2: // rotated rectangle (harder to detect)
                    DrawPolygon(img, RotatedRectPoints(cx, cy, size * 2, (int)(size * 1.3), angle), color, thickness);
                    break;
                case 3: // triangle
                    DrawPolygon(img, RotatedTrianglePoints(cx, cy, size, angle), color, thickness);
                    break;
                default: // line
                    int dx = (int)(size * Math.Cos(angle)), dy = (int)(size * Math.Sin(angle));
                    CvInvoke.Line(img, new DrawPoint(cx - dx, cy - dy), new DrawPoint(cx + dx, cy + dy),
                        color, _rng.Next(3, 6));
                    break;
            }
        }

        private static DrawPoint[] RotatedRectPoints(int cx, int cy, int width, int height, double angle)
        {
            double hw = width / 2.0, hh = height / 2.0, cos = Math.Cos(angle), sin = Math.Sin(angle);
            (double x, double y)[] corners = { (-hw, -hh), (hw, -hh), (hw, hh), (-hw, hh) };
            DrawPoint[] pts = new DrawPoint[4];
            for (int i = 0; i < 4; i++)
                pts[i] = new DrawPoint(
                    (int)(cx + corners[i].x * cos - corners[i].y * sin),
                    (int)(cy + corners[i].x * sin + corners[i].y * cos));
            return pts;
        }

        private static DrawPoint[] RotatedTrianglePoints(int cx, int cy, int size, double angle)
        {
            DrawPoint[] pts = new DrawPoint[3];
            for (int i = 0; i < 3; i++)
            {
                double a = angle + i * 2.0 * Math.PI / 3.0;
                pts[i] = new DrawPoint((int)(cx + size * Math.Cos(a)), (int)(cy + size * Math.Sin(a)));
            }
            return pts;
        }

        private static void DrawPolygon(Mat img, DrawPoint[] pts, MCvScalar color, int thickness)
        {
            using VectorOfPoint vp = new VectorOfPoint(pts);
            if (thickness < 0)
                CvInvoke.FillConvexPoly(img, vp, color);
            else
                CvInvoke.Polylines(img, vp, true, color, thickness);
        }

        // ---------- Detection ----------

        // The button doubles as New Image / Cancel depending on the busy state.
        // Idle: generate a fresh scene (which automatically runs detection).
        private void OnDetectButtonClicked(object sender, EventArgs e)
        {
            if (_detecting)
            {
                _cancelRequested = true;
                _redetectPending = false;
            }
            else
            {
                SetCurrentImage(GenerateRandomShapes());
            }
        }

        private async Task RunDetectionAsync()
        {
            // If a run is already in progress, just flag that the latest image
            // should be re-detected once it finishes (handles rapid Random taps).
            if (_detecting)
            {
                _redetectPending = true;
                return;
            }

            _detecting = true;
            _cancelRequested = false;
            SetBusy(true, "Detecting…");

            try
            {
                do
                {
                    _redetectPending = false;

                    Mat input = _currentImage;
                    if (input == null)
                        break;

                    // Detect on a clone on a background thread: keeps the UI
                    // responsive (spinner animates) and stays safe if the source
                    // image is replaced/disposed while detection is running.
                    using Mat clone = input.Clone();
                    using ShapeDetectionResult result = await Task.Run(() => _detector.Detect(clone));

                    if (_cancelRequested)
                        break;

                    _resultsRows.Children.Clear();
                    BuildResultRow(1, "Original", result.Original);
                    BuildResultRow(2, "Triangles & Rectangles", result.TrianglesRectangles);
                    BuildResultRow(3, "Circles", result.Circles);
                    BuildResultRow(4, "Lines", result.Lines);

                    _statusLabel.Text = $"Completed in {result.ElapsedMilliseconds} ms";
                    _statusStack.IsVisible = true;
                    _resultsHint.IsVisible = false;
                }
                while (_redetectPending && !_cancelRequested);
            }
            catch (Exception ex)
            {
                _resultsRows.Children.Clear();
                _statusStack.IsVisible = false;
                _resultsHint.IsVisible = true;
                _resultsHint.Text = "Detection failed: " + ex.Message;
            }
            finally
            {
                _detecting = false;
                SetBusy(false);

                if (_cancelRequested)
                {
                    _resultsRows.Children.Clear();
                    _statusStack.IsVisible = false;
                    _resultsHint.IsVisible = true;
                    _resultsHint.Text = "Detection cancelled. Tap “New Image” to run again.";
                }
            }
        }

        // Toggle the busy state: spinner overlay + Run/Cancel button.
        private void SetBusy(bool busy, string message = "Detecting…")
        {
            _loadingLabel.Text = message;
            _loadingOverlay.IsVisible = busy;
            _loadingIndicator.IsRunning = busy;
            _detectButton.Text = busy ? "Cancel" : "New Image";
            _detectButton.BackgroundColor = busy ? CancelColor : Accent;
            _detectButton.ImageSource = new FontImageSource
            {
                FontFamily = IconFont,
                Glyph = busy ? GlyphClose : GlyphCube,
                Color = Colors.White,
                Size = 20
            };
        }

        private void BuildResultRow(int number, string title, Mat image)
        {
            byte[] png = Encode(image);

            var thumb = new Image { Source = MatToImageSource(png), Aspect = Aspect.AspectFit, WidthRequest = 64, HeightRequest = 48 };
            var thumbCard = new Border
            {
                WidthRequest = 78,
                HeightRequest = 56,
                BackgroundColor = ImageBackground,
                Stroke = RowBorder,
                StrokeThickness = 1,
                Padding = new Thickness(4),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                VerticalOptions = LayoutOptions.Center,
                Content = thumb
            };

            var nameLabel = new Label
            {
                Text = $"{number}. {title}",
                FontFamily = TitleFont,
                FontSize = 16,
                TextColor = PrimaryText,
                VerticalOptions = LayoutOptions.Center
            };

            var expandTile = new Border
            {
                WidthRequest = 38,
                HeightRequest = 38,
                BackgroundColor = TileBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Content = MakeIcon(GlyphShape, Accent, 20)
            };

            var grid = new Grid
            {
                Padding = new Thickness(10),
                ColumnSpacing = 14,
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                }
            };
            grid.Add(thumbCard, 0, 0);
            grid.Add(nameLabel, 1, 0);
            grid.Add(expandTile, 2, 0);

            var row = new Border
            {
                BackgroundColor = CardBackground,
                Stroke = RowBorder,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                Content = grid
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) => await Navigation.PushModalAsync(new FullScreenImagePage(png, $"{number}. {title}"));
            row.GestureRecognizers.Add(tap);

            _resultsRows.Children.Add(row);
        }

        // ---------- Small UI helpers ----------

        private static Image MakeIcon(string glyph, Color color, double size) => new Image
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

        private View PipelineStep(string name, string description)
        {
            return new VerticalStackLayout
            {
                Children =
                {
                    new Label { Text = name, FontFamily = TitleFont, FontSize = 14, TextColor = PrimaryText },
                    new Label { Text = description, FontFamily = BodyFont, FontSize = 13, TextColor = SecondaryText }
                }
            };
        }

        // ---------- Mat <-> ImageSource ----------

        private static byte[] Encode(Mat m)
        {
            using VectorOfByte buf = new VectorOfByte();
            CvInvoke.Imencode(".png", m, buf);
            return buf.ToArray();
        }

        private static ImageSource MatToImageSource(Mat m) => MatToImageSource(Encode(m));

        private static ImageSource MatToImageSource(byte[] png) =>
            ImageSource.FromStream(() => new MemoryStream(png));
    }

    /// <summary>
    /// Simple full-screen image viewer with pinch-to-zoom, pan, and double-tap
    /// to reset. Presented modally from the Shape Detection result rows.
    /// </summary>
    internal class FullScreenImagePage : ContentPage
    {
        private const string IconFont = "MaterialSymbols";
        private const string GlyphClose = "";

        private double _currentScale = 1;
        private double _startScale = 1;
        private double _xOffset = 0;
        private double _yOffset = 0;

        public FullScreenImagePage(byte[] png, string title)
        {
            BackgroundColor = Colors.Black;
            NavigationPage.SetHasNavigationBar(this, false);

            var image = new Image
            {
                Source = ImageSource.FromStream(() => new MemoryStream(png)),
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            var pinch = new PinchGestureRecognizer();
            pinch.PinchUpdated += (s, e) =>
            {
                if (e.Status == GestureStatus.Started)
                    _startScale = image.Scale;
                else if (e.Status == GestureStatus.Running)
                {
                    _currentScale = Math.Clamp(_startScale * e.Scale, 1, 6);
                    image.Scale = _currentScale;
                }
            };
            image.GestureRecognizers.Add(pinch);

            var pan = new PanGestureRecognizer();
            pan.PanUpdated += (s, e) =>
            {
                if (image.Scale <= 1)
                    return;
                if (e.StatusType == GestureStatus.Running)
                {
                    image.TranslationX = _xOffset + e.TotalX;
                    image.TranslationY = _yOffset + e.TotalY;
                }
                else if (e.StatusType == GestureStatus.Completed)
                {
                    _xOffset = image.TranslationX;
                    _yOffset = image.TranslationY;
                }
            };
            image.GestureRecognizers.Add(pan);

            var doubleTap = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            doubleTap.Tapped += (s, e) =>
            {
                _currentScale = image.Scale > 1 ? 1 : 2.5;
                image.Scale = _currentScale;
                image.TranslationX = _xOffset = 0;
                image.TranslationY = _yOffset = 0;
            };
            image.GestureRecognizers.Add(doubleTap);

            var titleLabel = new Label
            {
                Text = title,
                TextColor = Colors.White,
                FontFamily = "InterSemiBold",
                FontSize = 16,
                VerticalOptions = LayoutOptions.Center
            };

            var closeButton = new Border
            {
                WidthRequest = 44,
                HeightRequest = 44,
                BackgroundColor = Color.FromArgb("#33FFFFFF"),
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                HorizontalOptions = LayoutOptions.End,
                Content = new Image
                {
                    Source = new FontImageSource { FontFamily = IconFont, Glyph = GlyphClose, Color = Colors.White, Size = 24 },
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };
            var closeTap = new TapGestureRecognizer();
            closeTap.Tapped += async (s, e) => await Navigation.PopModalAsync();
            closeButton.GestureRecognizers.Add(closeTap);

            var topBar = new Grid
            {
                Padding = new Thickness(20, 16),
                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
            };
            topBar.Add(titleLabel, 0, 0);
            topBar.Add(closeButton, 1, 0);

            var layout = new Grid
            {
                RowDefinitions = { new RowDefinition(GridLength.Auto), new RowDefinition(GridLength.Star) }
            };
            layout.Add(image, 0, 1);
            Grid.SetRowSpan(image, 2);
            layout.Add(topBar, 0, 0);

            Content = layout;
        }
    }
}
