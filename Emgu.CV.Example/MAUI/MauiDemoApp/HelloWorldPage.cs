//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

using DrawPoint = System.Drawing.Point;

namespace MauiDemoApp
{
    /// <summary>
    /// The classic smoke test on the <see cref="StagePage"/> layout: draws
    /// "Hello, world" on a randomly colored image, and lists which parts of
    /// OpenCV this build includes.
    /// </summary>
    public class HelloWorldPage : StagePage
    {
        private static readonly (string label, HersheyFonts font, string code)[] Fonts =
        {
            ("Simplex", HersheyFonts.Simplex, "Simplex"),
            ("Complex", HersheyFonts.Complex, "Complex"),
            ("Script", HersheyFonts.ScriptComplex, "ScriptComplex"),
        };

        private readonly Random _rng = new Random();
        private SegmentedControl _fontControl;
        private volatile int _fontIndex = 1;

        public HelloWorldPage() : base(new StageConfig
        {
            Title = "hello world",
            Category = "Image & 3D",
            Module = "core + imgproc",
            Description = "Draw \"Hello, world\" on an image: the library smoke test.",
            About = "Creates an image, fills it with a random color and writes \"Hello, world\" on it with one of OpenCV's built-in Hershey fonts. If you can see the text, the whole pipeline works end to end: create, draw, encode, display.\n\nThe results list is a system check: the OpenCV version, how many worker threads it uses, and which optional modules were built into this copy of the library.",
            HasCamera = false,
            AutoRun = true,
            RunLabel = "Render",
            OptionsTitle = "Font",
            ResultsTitle = "System check",
            ItemSingular = "check",
            ItemPlural = "checks",
            EmptyCaption = "render to run the system check",
        })
        {
        }

        protected override View BuildOptions()
        {
            _fontControl = new SegmentedControl(Fonts.Select(f => f.label), _fontIndex);
            _fontControl.SelectionChanged += (s, e) =>
            {
                _fontIndex = _fontControl.SelectedIndex;
                SetOptionsValue("Hershey " + Fonts[_fontIndex].label);
                _ = RunAsync();   // generated demo: refresh right away
            };
            return _fontControl;
        }

        protected override void OnStatusInit()
        {
            SetOptionsValue("Hershey " + Fonts[_fontIndex].label);
            SetStatus("OpenCV " + OpenCvVersion());
        }

        protected override StageResult RunCore(Mat input)
        {
            var draw = Stopwatch.StartNew();
            using Mat m = new Mat(400, 800, DepthType.Cv8U, 3);
            MCvScalar bg = new MCvScalar(_rng.Next(40, 210), _rng.Next(40, 210), _rng.Next(40, 210));
            m.SetTo(bg);
            // Dark text on a light background, light text on a dark one.
            double lum = 0.114 * bg.V0 + 0.587 * bg.V1 + 0.299 * bg.V2;
            MCvScalar fg = lum > 140 ? new MCvScalar(25, 25, 25) : new MCvScalar(240, 240, 240);
            int x = 30 + _rng.Next(0, 180);
            int y = 180 + _rng.Next(0, 120);
            CvInvoke.PutText(m, "Hello, world", new DrawPoint(x, y), Fonts[_fontIndex].font, 2.0, fg, 3, LineType.AntiAlias);
            draw.Stop();

            var post = Stopwatch.StartNew();
            byte[] png = MaskRcnnPage.Encode(m);
            var rows = SystemCheck();
            post.Stop();

            return new StageResult
            {
                OutputPng = png,
                OutputWidth = m.Width,
                OutputHeight = m.Height,
                Rows = rows,
                Summary = $"rendered · {draw.Elapsed.TotalMilliseconds:0.0} ms",
                InferenceMs = draw.Elapsed.TotalMilliseconds,
                PrePostMs = post.Elapsed.TotalMilliseconds,
            };
        }

        // ---- "Is it working?": the smoke test made explicit ----
        private static StageRow[] SystemCheck()
        {
            var cfg = CvInvoke.ConfigDict;
            return new[]
            {
                new StageRow("OpenCV version", OpenCvVersion()),
                new StageRow("Worker threads", SafeThreads()),
                Check("DNN module", cfg, "HAVE_OPENCV_DNN"),
                Check("Features (SIFT / KAZE)", cfg, "HAVE_OPENCV_FEATURES"),
                Check("Object detection", cfg, "HAVE_OPENCV_OBJDETECT"),
                Check("Tesseract OCR", cfg, "HAVE_EMGUCV_TESSERACT"),
                Check("Video", cfg, "HAVE_OPENCV_VIDEO"),
                Check("QR / Barcode", cfg, "HAVE_OPENCV_WECHAT_QRCODE"),
            };
        }

        private static StageRow Check(string label, Dictionary<string, double> cfg, string key)
        {
            bool ok = cfg.TryGetValue(key, out double v) && v != 0;
            return new StageRow(label, ok ? "Available" : "Not built", null, ok ? DemoTheme.StatusGreen : DemoTheme.MutedText);
        }

        private static string SafeThreads()
        {
            try { return CvInvoke.NumThreads.ToString(); } catch { return "—"; }
        }

        private static string OpenCvVersion()
        {
            try
            {
                var m = System.Text.RegularExpressions.Regex.Match(CvInvoke.BuildInformation ?? "", @"OpenCV\s+(\d+\.\d+\.\d+)");
                return m.Success ? m.Groups[1].Value : "5.0";
            }
            catch { return "—"; }
        }

        protected override string CodeSnippet => $@"using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

using Mat image = new Mat(400, 800, DepthType.Cv8U, 3);
image.SetTo(new MCvScalar(180, 120, 60));   // B, G, R

CvInvoke.PutText(image, ""Hello, world"",
    new System.Drawing.Point(40, 220),
    HersheyFonts.{Fonts[_fontIndex].code}, 2.0,
    new MCvScalar(240, 240, 240), 3, LineType.AntiAlias);

CvInvoke.Imwrite(""hello.png"", image);";
    }
}
