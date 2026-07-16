//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

using DrawPoint = System.Drawing.Point;

namespace MauiDemoApp
{
    public class HelloWorldPage : SimpleDemoPage
    {
        private static readonly Color Green = Color.FromArgb("#2BA84A");

        private readonly Image _img = new Image { Aspect = Aspect.AspectFit, HeightRequest = 240 };
        private readonly Random _rng = new Random();

        public HelloWorldPage(string glyph)
            : base("Hello World", "Library smoke test", glyph)
        {
            Redraw();

            AddCard(Card(new VerticalStackLayout
            {
                Spacing = 14,
                Children =
                {
                    SectionHeader("Output Image", PillButton(MaskRcnnPage.GlyphPlay, "Draw Again", Redraw)),
                    ImageFrame(_img),
                    Caption("Tap “Draw Again” to re-run the draw with random colors.")
                }
            }));

            AddCard(BuildSystemCheck());

            AddAbout("Creates a blue image and writes “Hello, world” in green — the classic smoke test that the whole OpenCV pipeline (create → draw → display) works end to end.");
        }

        // ---- Draw "Hello, world" with a random background/text colour and position ----
        private void Redraw()
        {
            using Mat m = new Mat(200, 400, DepthType.Cv8U, 3);
            MCvScalar bg = new MCvScalar(_rng.Next(40, 210), _rng.Next(40, 210), _rng.Next(40, 210));
            m.SetTo(bg);
            double lum = 0.114 * bg.V0 + 0.587 * bg.V1 + 0.299 * bg.V2;
            MCvScalar fg = lum > 140 ? new MCvScalar(25, 25, 25) : new MCvScalar(240, 240, 240);
            int x = 12 + _rng.Next(0, 90);
            int y = 95 + _rng.Next(0, 55);
            CvInvoke.PutText(m, "Hello, world", new DrawPoint(x, y), HersheyFonts.Complex, 1.0, fg, 2, LineType.AntiAlias);
            _img.Source = ToImageSource(m);
        }

        // ---- "Is it working?" — the smoke test made explicit ----
        private static View BuildSystemCheck()
        {
            var cfg = CvInvoke.ConfigDict;
            var rows = new VerticalStackLayout { Spacing = 0 };
            rows.Children.Add(InfoRow("OpenCV version", OpenCvVersion(), true));
            rows.Children.Add(InfoRow("Worker threads", SafeThreads(), true));
            rows.Children.Add(CheckRow("DNN module", Have(cfg, "HAVE_OPENCV_DNN"), true));
            rows.Children.Add(CheckRow("Features (SIFT / KAZE)", Have(cfg, "HAVE_OPENCV_FEATURES"), true));
            rows.Children.Add(CheckRow("Object detection", Have(cfg, "HAVE_OPENCV_OBJDETECT"), true));
            rows.Children.Add(CheckRow("Tesseract OCR", Have(cfg, "HAVE_EMGUCV_TESSERACT"), true));
            rows.Children.Add(CheckRow("Video", Have(cfg, "HAVE_OPENCV_VIDEO"), true));
            rows.Children.Add(CheckRow("QR / Barcode", Have(cfg, "HAVE_OPENCV_WECHAT_QRCODE"), false));

            return Card(new VerticalStackLayout { Spacing = 12, Children = { SectionHeader("System Check"), rows } });
        }

        private static bool Have(Dictionary<string, double> cfg, string key) => cfg.TryGetValue(key, out double v) && v != 0;

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

        private static View InfoRow(string label, string value, bool divider)
        {
            var grid = new Grid { Padding = new Thickness(0, 12), ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            grid.Add(new Label { Text = label, FontFamily = MaskRcnnPage.BodyFont, FontSize = 16, TextColor = MaskRcnnPage.PrimaryText, VerticalOptions = LayoutOptions.Center }, 0, 0);
            grid.Add(new Label { Text = value, FontFamily = MaskRcnnPage.TitleFont, FontSize = 15, TextColor = MaskRcnnPage.Accent, VerticalOptions = LayoutOptions.Center }, 1, 0);
            return Wrap(grid, divider);
        }

        private static View CheckRow(string label, bool ok, bool divider)
        {
            var grid = new Grid { Padding = new Thickness(0, 12), ColumnSpacing = 12, ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            grid.Add(MaskRcnnPage.MakeIcon(ok ? MaskRcnnPage.GlyphCheck : MaskRcnnPage.GlyphClose, ok ? Green : MaskRcnnPage.SecondaryText, 18), 0, 0);
            grid.Add(new Label { Text = label, FontFamily = MaskRcnnPage.BodyFont, FontSize = 16, TextColor = MaskRcnnPage.PrimaryText, VerticalOptions = LayoutOptions.Center }, 1, 0);
            grid.Add(new Label { Text = ok ? "Available" : "Not built", FontFamily = MaskRcnnPage.TitleFont, FontSize = 13, TextColor = ok ? Green : MaskRcnnPage.SecondaryText, VerticalOptions = LayoutOptions.Center }, 2, 0);
            return Wrap(grid, divider);
        }

        private static View Wrap(View row, bool divider)
        {
            var stack = new VerticalStackLayout();
            stack.Children.Add(row);
            if (divider)
                stack.Children.Add(new BoxView { HeightRequest = 1, Color = MaskRcnnPage.RowBorder });
            return stack;
        }
    }
}
