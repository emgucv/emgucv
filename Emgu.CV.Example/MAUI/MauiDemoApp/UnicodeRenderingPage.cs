//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Freetype;
using Emgu.CV.Models;
using Emgu.CV.Structure;

using Microsoft.Maui.Controls.Shapes;

using DrawSize = System.Drawing.Size;
using DrawPoint = System.Drawing.Point;

namespace MauiDemoApp
{
    public class UnicodeRenderingPage : SimpleDemoPage
    {
        private const string RenderFontFace = "FontFace";
        private const string RenderFreetype = "FreeType";

        private static readonly MCvScalar[] Palette =
        {
            new MCvScalar(80, 220, 100), new MCvScalar(70, 220, 230), new MCvScalar(200, 150, 240),
            new MCvScalar(230, 210, 120), new MCvScalar(230, 130, 200)
        };

        private readonly Image _image = new Image { Aspect = Aspect.AspectFit, HeightRequest = 300, HorizontalOptions = LayoutOptions.Fill };
        private readonly Editor _editor = new Editor
        {
            Text = "Hello\n您好\nこんにちは\n여보세요\nЗдравствуйте",
            FontSize = 16,
            HeightRequest = 128,
            BackgroundColor = Colors.Transparent,
            TextColor = MaskRcnnPage.PrimaryText
        };
        private readonly Label _status = new Label { Text = "Ready", FontFamily = MaskRcnnPage.TitleFont, FontSize = 13, TextColor = MaskRcnnPage.Accent };
        private readonly bool _haveFreetype = CvInvoke.ConfigDict["HAVE_OPENCV_FREETYPE"] != 0;

        private FontFace _fontFace;
        private FreetypeNotoSansCJK _freetype2;
        private string _renderOption = RenderFontFace;
        private bool _busy;
        private bool _drawn;

        public UnicodeRenderingPage(string glyph)
            : base("Unicode Rendering", "Draw text in any language on an image", glyph)
        {
            AddCard(Card(new VerticalStackLayout
            {
                Spacing = 14,
                Children =
                {
                    SectionHeader("Rendered Image"),
                    ImageFrame(_image),
                    Caption("Your text drawn with OpenCV's Unicode text rendering.")
                }
            }));

            var editorFrame = new Border
            {
                BackgroundColor = MaskRcnnPage.ImageBackground,
                Stroke = MaskRcnnPage.RowBorder,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(14) },
                Padding = new Thickness(10, 2),
                Content = _editor
            };

            var inputChildren = new VerticalStackLayout { Spacing = 14 };
            inputChildren.Children.Add(new Label { Text = "Your Text", FontFamily = MaskRcnnPage.TitleFont, FontSize = 17, TextColor = MaskRcnnPage.PrimaryText });
            inputChildren.Children.Add(Caption("Type anything — English, 中文, 日本語, 한국어, Русский — one line each."));
            inputChildren.Children.Add(editorFrame);
            if (_haveFreetype)
                inputChildren.Children.Add(SegmentedToggle(new[] { RenderFontFace, RenderFreetype }, 0, i => { _renderOption = i == 0 ? RenderFontFace : RenderFreetype; DrawText(); }));
            inputChildren.Children.Add(PrimaryButton("Draw", MaskRcnnPage.GlyphPlay, (s, e) => DrawText()));

            var statusRow = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            statusRow.Add(new Label { Text = "Renderer: " + (_haveFreetype ? "FontFace / FreeType" : "FontFace"), FontFamily = MaskRcnnPage.BodyFont, FontSize = 13, TextColor = MaskRcnnPage.SecondaryText, VerticalOptions = LayoutOptions.Center }, 0, 0);
            statusRow.Add(_status, 1, 0);
            inputChildren.Children.Add(statusRow);

            AddCard(Card(inputChildren));

            AddAbout("Plain OpenCV text only supports ASCII. This renders any Unicode text (CJK, Cyrillic, …) onto an image using the FontFace, and optionally the FreeType, text APIs.");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_drawn)
                return;
            _drawn = true;
            DrawText();
        }

        private async void DrawText()
        {
            if (_busy)
                return;
            _busy = true;
            try
            {
                var lines = new List<string>();
                foreach (string raw in (_editor.Text ?? string.Empty).Replace("\r", "").Split('\n'))
                    if (!string.IsNullOrWhiteSpace(raw))
                        lines.Add(raw.Trim());
                if (lines.Count == 0)
                    lines.Add("Hello");

                bool useFreetype = _renderOption == RenderFreetype && _haveFreetype;
                if (useFreetype && _freetype2 == null)
                {
                    SetBusy(true, "Downloading font…\n(first time only)");
                    _freetype2 = new FreetypeNotoSansCJK();
                    try { await Task.Run(() => _freetype2.Init(null)); }
                    catch (Exception ex) { SetBusy(false); _status.Text = "FreeType failed"; _freetype2 = null; useFreetype = false; System.Diagnostics.Debug.WriteLine("FreeType init: " + ex); }
                    SetBusy(false);
                }

                int rowH = 84;
                using Mat img = new Mat(new DrawSize(700, 60 + lines.Count * rowH), DepthType.Cv8U, 3);
                img.SetTo(new MCvScalar(26, 16, 12));   // dark navy

                for (int i = 0; i < lines.Count; i++)
                {
                    MCvScalar color = Palette[i % Palette.Length];
                    int y = 66 + i * rowH;
                    if (useFreetype)
                        _freetype2.PutText(img, lines[i], new DrawPoint(40, y), 44, color, 1, LineType.EightConnected, false);
                    else
                        CvInvoke.PutText(img, lines[i], new DrawPoint(40, y), color, GetFontFace(), 44);
                }

                _image.Source = ToImageSource(img);
                _status.Text = useFreetype ? "FreeType" : "FontFace";
            }
            catch (Exception ex)
            {
                _status.Text = "Error";
                await DisplayAlert("Unicode Rendering", ex.Message, "OK");
            }
            finally
            {
                _busy = false;
            }
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
    }
}
