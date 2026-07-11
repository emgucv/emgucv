//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Freetype;
using Emgu.CV.Models;
using Emgu.CV.Platform.Maui.UI;
using Emgu.Util;

using Size = System.Drawing.Size;
using Point = System.Drawing.Point;

namespace MauiDemoApp
{
    public class UnicodeRenderingPage : ButtonTextImagePage
    {
        private const String _renderWithFontFace = "FontFace";
        private const String _renderWithFreetype = "FreeType";

        private static readonly String[] _helloTexts = new String[]
        {
            "Hello",
            "您好",
            "こんにちは",
            "여보세요",
            "Здравствуйте"
        };

        private FontFace _fontFace;
        private FreetypeNotoSansCJK _freetype2;

        public UnicodeRenderingPage()
            : base()
        {
            this.Title = "Unicode Rendering";

            var picker = this.Picker;
            picker.Title = "Render with";
            picker.Items.Add(_renderWithFontFace);
            bool haveFreetype = CvInvoke.ConfigDict["HAVE_OPENCV_FREETYPE"] != 0;
            if (haveFreetype)
                picker.Items.Add(_renderWithFreetype);
            picker.SelectedIndex = 0;
            //No need to show the picker if FontFace is the only option
            picker.IsVisible = haveFreetype;

            var button = this.GetButton();
            button.Text = "Draw Unicode Text";
            button.Clicked += async (sender, args) =>
            {
                String renderOption = picker.Items[picker.SelectedIndex];

                Mat img = new Mat(new Size(640, 480), DepthType.Cv8U, 3);
                img.SetTo(new MCvScalar(0, 0, 0, 0));
                MCvScalar color = new MCvScalar(255, 255, 0);

                if (renderOption.Equals(_renderWithFreetype))
                {
                    if (_freetype2 == null)
                    {
                        _freetype2 = new FreetypeNotoSansCJK();
                        try
                        {
                            await Task.Run(() => _freetype2.Init(DownloadManager_OnDownloadProgressChanged));
                        }
                        catch (Exception ex)
                        {
                            SetMessage(String.Format("Failed to initialize FreeType: {0}", ex.Message));
                            _freetype2 = null;
                            return;
                        }
                    }

                    for (int i = 0; i < _helloTexts.Length; i++)
                        _freetype2.PutText(img, _helloTexts[i], new Point(100, 50 + i * 50), 36, color, 1,
                            LineType.EightConnected, false);
                    SetMessage("\"Hello\" in 5 languages rendered with the freetype module");
                }
                else
                {
                    if (_fontFace == null)
                    {
                        //The built-in Unicode font ("uni", WenQuanYi Micro Hei) is
                        //required for the CJK text; fall back to the default embedded
                        //font if the native library was built without WITH_UNIFONT.
                        _fontFace = new FontFace();
                        _fontFace.Set("uni");
                    }

                    for (int i = 0; i < _helloTexts.Length; i++)
                        CvInvoke.PutText(img, _helloTexts[i], new Point(100, 50 + i * 50), color, _fontFace, 36);
                    SetMessage(String.Format(
                        "\"Hello\" in 5 languages rendered with FontFace (\"{0}\")",
                        _fontFace.Name));
                }

                SetImage(img);
            };
        }

        private void DownloadManager_OnDownloadProgressChanged(long? totalBytesToReceive, long bytesReceived, double? progressPercentage)
        {
            if (totalBytesToReceive == null)
                SetMessage(String.Format("{0} bytes downloaded.", bytesReceived));
            else
                SetMessage(String.Format("{0} of {1} bytes downloaded ({2}%)", bytesReceived, totalBytesToReceive, progressPercentage));
        }
    }
}
