//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;

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
    /// Classic (non-DNN) OpenCV shape detection on the shared <see cref="StagePage"/>
    /// layout. Each run draws a random scene of non-overlapping shapes, runs
    /// <see cref="ShapeDetector"/> on it, and shows the scene with every detected
    /// triangle, rectangle, circle and line outlined, plus a count per shape type.
    /// </summary>
    public class ShapeDetectionPage : StagePage
    {
        // One color per shape type, shared by the outlines and the result rows.
        private static Color TriangleColor => ClassColor(2);   // pink
        private static Color RectangleColor => ClassColor(1);  // sky
        private static Color CircleColor => ClassColor(3);     // orange
        private static Color LineColor => ClassColor(0);       // lime

        private ShapeDetector _detector = new ShapeDetector();
        private readonly Random _rng = new Random();
        private Mat _scene;                 // the last generated scene
        private bool _keepScene;            // re-detect the same scene instead of drawing a new one
        private SegmentedControl _sceneControl;

        public ShapeDetectionPage() : base(new StageConfig
        {
            Title = "Shape Detection",
            Category = "Detection",
            Module = "imgproc module",
            Description = "Find triangles, rectangles, circles and lines with classic OpenCV.",
            About = "No AI model here: this is classic computer vision. The image is converted to grayscale and blurred, Canny finds its edges, the Hough transform finds circles and straight line segments, and contours simplified to 3 or 4 corners become triangles and rectangles (4 corners count as a rectangle only if every angle is close to 90°).\n\n" +
                    "Each run draws a fresh random scene of filled and outlined shapes, some of them rotated. Choose \"Keep scene\" to run the detector again on the same one.\n\n" +
                    "Line segments are counted along every straight edge, including the sides of rectangles and triangles, so there are usually more lines than shapes.",
            RunLabel = "Detect shapes",
            AutoRun = true,
            OptionsTitle = "Scene",
            EmptyCaption = "run it to count the shapes",
            ItemSingular = "shape type",
            ItemPlural = "shape types",
            ResultsTitle = "Shapes found",
        })
        {
        }

        protected override string BackendName => "CPU · OpenCV";

        protected override View BuildOptions()
        {
            _sceneControl = new SegmentedControl(new[] { "New each run", "Keep scene" }, 0);
            _sceneControl.SelectionChanged += (s, e) =>
            {
                _keepScene = _sceneControl.SelectedIndex == 1;
                UpdateStatus();
            };
            _sceneControl.HorizontalOptions = LayoutOptions.Start;
            return _sceneControl;
        }

        protected override void OnStatusInit() => UpdateStatus();

        private void UpdateStatus()
        {
            SetOptionsValue("800 × 560");
            SetStatus(_keepScene ? "Detects the same scene again" : "A new random scene every run");
        }

        protected override StageResult RunCore(Mat input)
        {
            var total = Stopwatch.StartNew();
            // "pre": draw the scene (or reuse the last one).
            if (_scene == null || !_keepScene)
            {
                _scene?.Dispose();
                _scene = GenerateRandomShapes();
            }

            var infer = Stopwatch.StartNew();
            using ShapeDetectionResult result = _detector.Detect(_scene);
            infer.Stop();

            // "post": outline what was found on a darkened copy of the scene.
            using Mat canvas = RenderFound(_scene, result);
            byte[] png = MaskRcnnPage.Encode(canvas);
            total.Stop();

            int shapes = result.TriangleCount + result.RectangleCount + result.CircleCount;
            int max = Math.Max(1, Math.Max(Math.Max(result.TriangleCount, result.RectangleCount), Math.Max(result.CircleCount, result.LineCount)));
            return new StageResult
            {
                OutputPng = png,
                OutputWidth = canvas.Width,
                OutputHeight = canvas.Height,
                Rows = new[]
                {
                    CountRow("Triangles", result.TriangleCount, max, TriangleColor),
                    CountRow("Rectangles", result.RectangleCount, max, RectangleColor),
                    CountRow("Circles", result.CircleCount, max, CircleColor),
                    CountRow("Lines", result.LineCount, max, LineColor),
                },
                Summary = $"{shapes} {(shapes == 1 ? "shape" : "shapes")} · {result.LineCount} {(result.LineCount == 1 ? "line" : "lines")} · {infer.Elapsed.TotalMilliseconds:0} ms",
                InferenceMs = infer.Elapsed.TotalMilliseconds,
                PrePostMs = total.Elapsed.TotalMilliseconds - infer.Elapsed.TotalMilliseconds,
            };
        }

        private static StageRow CountRow(string label, int count, int max, Color color) =>
            new StageRow(label, count.ToString(), (double)count / max, color);

        // ShapeDetector.Detect returns each stage as its own image on white: triangles
        // (OrangeRed) and rectangles (RoyalBlue) together, circles, and lines. Turn
        // those drawings into masks and paint them, in the page's colors, over a
        // darkened copy of the scene. Lines go first so shapes stay on top.
        private static Mat RenderFound(Mat scene, ShapeDetectionResult result)
        {
            Mat canvas = new Mat();
            scene.ConvertTo(canvas, DepthType.Cv8U, 0.35);
            using (Mat m = NonWhite(result.Lines)) Paint(canvas, m, LineColor);
            using (Mat m = NonWhite(result.Circles)) Paint(canvas, m, CircleColor);
            using (Mat m = ExactColor(result.TrianglesRectangles, DrawColor.RoyalBlue)) Paint(canvas, m, RectangleColor);
            using (Mat m = ExactColor(result.TrianglesRectangles, DrawColor.OrangeRed)) Paint(canvas, m, TriangleColor);
            return canvas;
        }

        private static Mat NonWhite(Mat image)
        {
            using Mat gray = new Mat();
            CvInvoke.CvtColor(image, gray, ColorConversion.Bgr2Gray);
            Mat mask = new Mat();
            CvInvoke.Threshold(gray, mask, 250, 255, ThresholdType.BinaryInv);
            return mask;
        }

        private static Mat ExactColor(Mat image, DrawColor color)
        {
            Mat mask = new Mat();
            using var bgr = new ScalarArray(new Bgr(color).MCvScalar);
            CvInvoke.InRange(image, bgr, bgr, mask);
            return mask;
        }

        private static void Paint(Mat canvas, Mat mask, Color color) =>
            canvas.SetTo(new MCvScalar(color.Blue * 255, color.Green * 255, color.Red * 255), mask);

        protected override string CodeSnippet => @"using Emgu.CV;
using Emgu.CV.Models;

using var detector = new ShapeDetector();
using Mat image = CvInvoke.Imread(""shapes.png"");

// Grayscale → blur → Canny → Hough → contours
using ShapeDetectionResult r = detector.Detect(image);

Console.WriteLine(
    $""{r.TriangleCount} triangles, {r.RectangleCount} rectangles, "" +
    $""{r.CircleCount} circles, {r.LineCount} lines"");

// Each stage is also returned as an image
CvInvoke.Imwrite(""circles.png"", r.Circles);
CvInvoke.Imwrite(""lines.png"", r.Lines);";

        protected override void OnClosed()
        {
            _scene?.Dispose();
            _scene = null;
            _detector?.Dispose();
            _detector = null;
        }

        // ---------- Random scene ----------

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
