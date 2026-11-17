//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

using Microsoft.Maui.Controls.Shapes;

using DrawSize = System.Drawing.Size;
using DrawPoint = System.Drawing.Point;

namespace MauiDemoApp
{
    /// <summary>
    /// Draws any Unicode text (CJK, Cyrillic, …) on an image with OpenCV's
    /// FontFace text API, on the <see cref="StagePage"/> layout. The text is typed
    /// in the panel; one colored line per row.
    /// </summary>
    public class UnicodeRenderingPage : StagePage
    {
        private const string DefaultText = "Hello\n您好\nこんにちは\n여보세요\nЗдравствуйте";

        // Line colors drawn into the image (BGR), on its fixed dark navy background.
        private static readonly MCvScalar[] Palette =
        {
            new MCvScalar(80, 220, 100), new MCvScalar(70, 220, 230), new MCvScalar(200, 150, 240),
            new MCvScalar(230, 210, 120), new MCvScalar(230, 130, 200)
        };
        private static readonly MCvScalar Background = new MCvScalar(26, 16, 12);

        private static readonly (string label, int px)[] Sizes = { ("S", 32), ("M", 44), ("L", 60) };

        private Editor _editor;
        private SegmentedControl _sizeControl;
        private volatile int _sizeIndex = 1;
        private volatile string _text = DefaultText;
        private FontFace _fontFace;

        public UnicodeRenderingPage() : base(new StageConfig
        {
            Title = "unicode",
            Category = "Image & 3D",
            Module = "imgproc · FontFace",
            Description = "Draw text in any language on an image.",
            About = "OpenCV's classic Hershey fonts only cover ASCII. The FontFace API renders TrueType / OpenType fonts instead, so any Unicode text (Chinese, Japanese, Korean, Cyrillic and more) can be drawn straight onto an image.\n\nThis page uses the built-in \"uni\" font, which includes CJK characters. Type one line per row; each line gets its own color and the image grows to fit.",
            HasCamera = false,
            AutoRun = true,
            RunLabel = "Render",
            OptionsTitle = "Your text",
            ResultsTitle = "Lines",
            ItemSingular = "line",
            ItemPlural = "lines",
            EmptyCaption = "render to see each line's script here",
        })
        {
        }

        protected override View BuildOptions()
        {
            _editor = new Editor
            {
                Text = _text,
                FontFamily = DemoTheme.BodyFont,
                FontSize = 16,
                HeightRequest = 140,
                AutoSize = EditorAutoSizeOption.Disabled,
                BackgroundColor = Colors.Transparent,
                TextColor = DemoTheme.PrimaryText,
                PlaceholderColor = DemoTheme.MutedText,
                Placeholder = "Type one line per row",
            };
            _editor.TextChanged += (s, e) => _text = e.NewTextValue ?? "";
            var editorBox = new Border
            {
                BackgroundColor = DemoTheme.PageBackground,
                Stroke = DemoTheme.Hairline,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) },
                Padding = new Thickness(10, 4),
                Content = _editor
            };

            _sizeControl = new SegmentedControl(Sizes.Select(x => x.label), _sizeIndex);
            _sizeControl.SelectionChanged += (s, e) =>
            {
                _sizeIndex = _sizeControl.SelectedIndex;
                UpdateOptionsValue();
                _ = RunAsync();   // generated demo: refresh right away
            };
            var sizeRow = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            sizeRow.Add(new Label { Text = "Text size", FontFamily = DemoTheme.BodyFont, FontSize = 13, TextColor = DemoTheme.SecondaryText, VerticalOptions = LayoutOptions.Center }, 0, 0);
            sizeRow.Add(_sizeControl, 1, 0);

            return new VerticalStackLayout { Spacing = 12, Children = { editorBox, sizeRow } };
        }

        protected override void OnStatusInit()
        {
            UpdateOptionsValue();
            SetStatus("English, 中文, 日本語, 한국어, Русский…");
        }

        private void UpdateOptionsValue() => SetOptionsValue($"{Sizes[_sizeIndex].px} px");

        protected override StageResult RunCore(Mat input)
        {
            var lines = new List<string>();
            foreach (string raw in (_text ?? "").Replace("\r", "").Split('\n'))
                if (!string.IsNullOrWhiteSpace(raw))
                    lines.Add(raw.Trim());
            if (lines.Count == 0)
                lines.Add("Hello");
            int px = Sizes[_sizeIndex].px;

            var render = Stopwatch.StartNew();
            FontFace font = GetFontFace();
            // Size the image to the widest line.
            int margin = (int)(px * 0.9);
            int rowH = (int)(px * 1.9);
            int width = 700;
            foreach (string line in lines)
            {
                var box = CvInvoke.GetTextSize(DrawSize.Empty, line, new DrawPoint(0, 0), font, px);
                width = Math.Max(width, box.Width + margin * 2);
            }
            width = Math.Min(width, 2400);
            // First baseline at margin + px; one rowH per further line; margin under the last.
            int height = margin + px + (lines.Count - 1) * rowH + margin;
            using Mat img = new Mat(new DrawSize(width, height), DepthType.Cv8U, 3);
            img.SetTo(Background);
            var rows = new StageRow[lines.Count];
            for (int i = 0; i < lines.Count; i++)
            {
                MCvScalar color = Palette[i % Palette.Length];
                int baseline = margin + i * rowH + px;
                CvInvoke.PutText(img, lines[i], new DrawPoint(margin, baseline), color, font, px);
                rows[i] = new StageRow(lines[i], ScriptOf(lines[i]), null, Color.FromRgb((int)color.V2, (int)color.V1, (int)color.V0));
            }
            render.Stop();

            var encode = Stopwatch.StartNew();
            byte[] png = MaskRcnnPage.Encode(img);
            encode.Stop();

            return new StageResult
            {
                OutputPng = png,
                OutputWidth = img.Width,
                OutputHeight = img.Height,
                Rows = rows,
                Summary = $"{lines.Count} {(lines.Count == 1 ? "line" : "lines")} · {render.Elapsed.TotalMilliseconds:0} ms",
                InferenceMs = render.Elapsed.TotalMilliseconds,
                PrePostMs = encode.Elapsed.TotalMilliseconds,
            };
        }

        private FontFace GetFontFace()
        {
            if (_fontFace == null)
            {
                _fontFace = new FontFace();
                _fontFace.Set("uni");   // built-in Unicode font (CJK-capable)
            }
            return _fontFace;
        }

        // The main writing system of a line, from its first non-ASCII letters.
        private static string ScriptOf(string s)
        {
            bool han = false, other = false;
            foreach (char c in s)
            {
                if (c < 0x80)
                    continue;
                if (c >= 0x3040 && c <= 0x30FF) return "Japanese";
                if ((c >= 0xAC00 && c <= 0xD7AF) || (c >= 0x1100 && c <= 0x11FF) || (c >= 0x3130 && c <= 0x318F)) return "Korean";
                if (c >= 0x0400 && c <= 0x04FF) return "Cyrillic";
                if (c >= 0x0370 && c <= 0x03FF) return "Greek";
                if (c >= 0x0590 && c <= 0x05FF) return "Hebrew";
                if (c >= 0x0600 && c <= 0x06FF) return "Arabic";
                if (c >= 0x0900 && c <= 0x097F) return "Devanagari";
                if (c >= 0x0E00 && c <= 0x0E7F) return "Thai";
                if ((c >= 0x4E00 && c <= 0x9FFF) || (c >= 0x3400 && c <= 0x4DBF)) han = true;
                else if (char.IsLetter(c) && c < 0x0250) { }   // accented Latin
                else other = true;
            }
            // Han without kana is Chinese (Japanese returns above on the first kana).
            if (han) return "Chinese";
            return other ? "Symbols" : "Latin";
        }

        protected override string CodeSnippet => $@"using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

using FontFace font = new FontFace();
font.Set(""uni"");   // built-in Unicode font with CJK

using Mat image = new Mat(new Size(700, 200), DepthType.Cv8U, 3);
image.SetTo(new MCvScalar(26, 16, 12));

CvInvoke.PutText(image, ""您好, Здравствуйте"",
    new Point(40, 100), new MCvScalar(80, 220, 100),
    font, {Sizes[_sizeIndex].px});   // size in pixels

CvInvoke.Imwrite(""unicode.png"", image);";

        protected override void OnClosed()
        {
            _fontFace?.Dispose();
            _fontFace = null;
        }
    }
}
