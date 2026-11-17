//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Models;
using Emgu.CV.Platform.Maui.UI;

namespace MauiDemoApp
{
    public partial class MainPage : ContentPage
    {
        private static bool _dnnDiagnosticsEnabled;

        //int count = 0;

        public MainPage()
        {
            InitializeComponent();

            
#if DEBUG
            CvInvoke.LogLevel = LogLevel.Verbose; //LogLevel.Debug;
#endif

            // Hide the top navigation bar on the home page for a cleaner, more spacious look.
            // (The "About" page is reached from a row at the bottom of the list instead.)
            Shell.SetNavBarIsVisible(this, false);

            Button helloWorldButton = new Button();
            helloWorldButton.Text = "Hello world";

            Button planarSubdivisionButton = new Button();
            planarSubdivisionButton.Text = "Planar Subdivision";

            Button sceneTextDetectionButton = new Button();
            sceneTextDetectionButton.Text = "Scene Text detection (DNN Module)";

            Button featureDetectionButton = new Button();
            featureDetectionButton.Text = "Feature Matching";

            Button shapeDetectionButton = new Button();
            shapeDetectionButton.Text = "Shape Detection";

            Button maskRcnnButton = new Button();
            maskRcnnButton.Text = "Mask RCNN (DNN module)";

            Button yoloButton = new Button();
            yoloButton.Text = "Yolo (DNN module)";

            Button superresButton = new Button();
            superresButton.Text = "Super resolution (DNN Module)";

            List<View> buttonList = new List<View>()
            {
                helloWorldButton,
                planarSubdivisionButton,
                sceneTextDetectionButton,
                featureDetectionButton,
                shapeDetectionButton,
                maskRcnnButton,
                yoloButton,
                superresButton
            };

            var openCVConfigDict = CvInvoke.ConfigDict;
            bool haveViz = (openCVConfigDict["HAVE_OPENCV_VIZ"] != 0);
            bool haveDNN = (openCVConfigDict["HAVE_OPENCV_DNN"] != 0);
            bool haveFace = (openCVConfigDict["HAVE_OPENCV_FACE"] != 0);
            bool haveWechatQRCode = (openCVConfigDict["HAVE_OPENCV_WECHAT_QRCODE"] != 0);
            //bool haveBarcode = (openCVConfigDict["HAVE_OPENCV_BARCODE"] != 0);
            bool haveObjdetect = (openCVConfigDict["HAVE_OPENCV_OBJDETECT"] != 0);
            bool haveTesseract = (openCVConfigDict["HAVE_EMGUCV_TESSERACT"] != 0);
            bool haveFeatures = (openCVConfigDict["HAVE_OPENCV_FEATURES"] != 0);
            bool haveVideo = (openCVConfigDict["HAVE_OPENCV_VIDEO"] != 0);
           // bool haveOptFlow = (openCVConfigDict["HAVE_OPENCV_OPTFLOW"] != 0);


            if (haveDNN)
            {
#if DEBUG
                // Only once per app run: the home page is rebuilt when the theme
                // changes, and enabling diagnostics a second time throws "Layer
                // NotImplemented already was registered" (fatal on iOS).
                if (!_dnnDiagnosticsEnabled)
                {
                    DnnInvoke.EnableModelDiagnostics(true);
                    _dnnDiagnosticsEnabled = true;
                }
#endif
            }

            bool haveCamera = true;

            // Forward-declared here so the button Clicked handlers (which run
            // after construction) can capture it; it is assigned further below.
            Func<string, string> demoGlyph = null;

            /*
            if (haveOptFlow && haveCamera)
            {
#if !(__MACCATALYST__ || __ANDROID__ || __IOS__ || NETFX_CORE)
                Button motionDetectionButton = new Button();
                motionDetectionButton.Text = "Motion Detection";
                buttonList.Add(motionDetectionButton);
                motionDetectionButton.Clicked += (sender, args) =>
                {
                    ProcessAndRenderPage motionDetectionPage = new ProcessAndRenderPage(
                        new MotionDetectionModel(),
                        "Open Camera",
                        null,
                        "This demo use MotionHistory for motion detection. The 3 images shown once it is up and running: 1. original image; 2. Foreground image; 3. Motion history");
                    MainPage.Navigation.PushAsync(motionDetectionPage);
                };
#endif
            }*/

            helloWorldButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new HelloWorldPage(demoGlyph("hello")));
            };

            
            planarSubdivisionButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new PlanarSubdivisionPage(demoGlyph("planar")));
            };


            
            shapeDetectionButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new ShapeDetectionPage());
            };

            
            featureDetectionButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new FeatureMatchingPage(demoGlyph("feature matching")));
            };

            
            maskRcnnButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new MaskRcnnPage());
            };


            sceneTextDetectionButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new ModelShowcasePage(
                    "Scene Text",
                    "Find and read English and Chinese text in a photo.",
                    demoGlyph("scene text"),
                    () => new SceneTextDetector(),
                    new[]
                    {
                        new ModelShowcasePage.Sample("cars_license_plate.png", "License plate", MaskRcnnPage.GlyphImage),
                        new ModelShowcasePage.Sample("test_image.png", "Document", MaskRcnnPage.GlyphImage),
                    },
                    customProcess: (model, input) =>
                    {
                        // Scene text recognizes the words (DetectedObject.Label);
                        // list them in the Results card, not just on the image.
                        var std = (SceneTextDetector)model;
                        DetectedObject[] objs = std.Detect(input);
                        using Mat annotated = input.Clone();
                        std.Render(annotated, objs);
                        var items = new List<string>();
                        foreach (DetectedObject o in objs)
                            if (!String.IsNullOrWhiteSpace(o.Label))
                                items.Add(o.Label);
                        return (MaskRcnnPage.Encode(annotated), (IReadOnlyList<string>)items);
                    },
                    about: "Finds text anywhere in a photo — signs, labels, plates — and reads it, even at odd angles.",
                    tag: "DNN",
                    category: "Detection"));
            };
            yoloButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new YoloPage());
            };

            superresButton.Clicked += (sender, args) =>
            {
                // The model name must be the first token; the text after it is a
                // description (Superres.Init only parses the leading token).
                this.Navigation.PushAsync(new ModelShowcasePage(
                    "Super Resolution",
                    "Upscale a photo with a neural network.",
                    demoGlyph("super resolution"),
                    () => new Superres(),
                    new[]
                    {
                        new ModelShowcasePage.Sample("dog416.png", "Dog & bike", MaskRcnnPage.GlyphImage),
                        new ModelShowcasePage.Sample("lena.jpg", "Portrait", MaskRcnnPage.GlyphImage),
                    },
                    pickerTitle: "Super resolution version",
                    pickerOptions: new[]
                    {
                        "FsrcnnX2 - 2x, fastest, tiny model",
                        "FsrcnnX3 - 3x, fastest, tiny model",
                        "EspcnX2 - 2x, fast, small model",
                    },
                    hasCamera: false,
                    about: "Uses a neural network to upscale and sharpen an image, reconstructing fine detail (2×–3×).",
                    tag: "DNN",
                    category: "Image & 3D"));
            };

            if (haveDNN)
            {
                Button qwenChatButton = new Button();
                qwenChatButton.Text = "Qwen Chat (LLM)";
                buttonList.Add(qwenChatButton);

                qwenChatButton.Clicked += (sender, args) =>
                {
                    this.Navigation.PushAsync(new QwenChatPage());
                };
            }

            if (haveDNN)
            {
                Button smolVlm2Button = new Button();
                smolVlm2Button.Text = "Chat with Image (SmolVLM2)";
                buttonList.Add(smolVlm2Button);

                smolVlm2Button.Clicked += (sender, args) =>
                {
                    this.Navigation.PushAsync(new SmolVlm2Page());
                };
            }

            maskRcnnButton.IsVisible = haveDNN;
            //faceLandmarkDetectionButton.IsVisible = haveDNN;
            yoloButton.IsVisible = haveDNN;
            superresButton.IsVisible = haveDNN;
            //Scene text rendering uses the FontFace class from the imgproc module,
            //no longer requires the freetype contrib module.
            sceneTextDetectionButton.IsVisible = haveDNN;
            featureDetectionButton.IsVisible = haveFeatures;
            
            
            if (haveTesseract)
            {
                Button ocrButton = new Button();
                ocrButton.Text = "Tesseract OCR";
                buttonList.Add(ocrButton);

                ocrButton.Clicked += (sender, args) =>
                {
                    this.Navigation.PushAsync(new ModelShowcasePage(
                        "Tesseract OCR",
                        "Read printed text from an image.",
                        demoGlyph("ocr"),
                        () => new TesseractModel(),
                        new[]
                        {
                            new ModelShowcasePage.Sample("test_image.png", "Document", MaskRcnnPage.GlyphImage),
                        },
                        hasCamera: false,
                        about: "Classic document OCR — reads printed text from an image into editable text.",
                    tag: "Tesseract",
                    category: "Detection"));
                };
            }

            if (haveDNN)
            {
                Button paddleOcrButton = new Button();
                paddleOcrButton.Text = "PaddleOCR";
                buttonList.Add(paddleOcrButton);

                paddleOcrButton.Clicked += (sender, args) =>
                {
                    this.Navigation.PushAsync(new ModelShowcasePage(
                        "PaddleOCR",
                        "Detect and read text with PP-OCRv4.",
                        demoGlyph("ocr"),
                        () => new PaddleOCR(),
                        new[]
                        {
                            new ModelShowcasePage.Sample("cars_license_plate.png", "License plate", MaskRcnnPage.GlyphImage),
                            new ModelShowcasePage.Sample("test_image.png", "Document", MaskRcnnPage.GlyphImage),
                        },
                        customProcess: (model, input) =>
                        {
                            // PP-OCRv4 detects + reads text: draw the regions and
                            // list the recognized text in the Results card.
                            var paddle = (PaddleOCR)model;
                            PaddleOCR.OcrResult[] results = paddle.Recognize(input);
                            using Mat annotated = input.Clone();
                            var items = new List<string>();
                            foreach (PaddleOCR.OcrResult r in results)
                            {
                                System.Drawing.Point[] corners = Array.ConvertAll(r.Region, System.Drawing.Point.Round);
                                using (var vp = new Emgu.CV.Util.VectorOfPoint(corners))
                                    CvInvoke.Polylines(annotated, vp, true, new Emgu.CV.Structure.MCvScalar(0, 0, 255), 2);
                                if (!String.IsNullOrWhiteSpace(r.Text))
                                    items.Add(r.Text);
                            }
                            return (MaskRcnnPage.Encode(annotated), (IReadOnlyList<string>)items);
                        },
                        about: "A newer, more accurate OCR engine (PP-OCRv4) that finds and reads text — strong on dense and multilingual text.",
                    tag: "DNN",
                    category: "Detection"));
                };
            }


            if (haveVideo && haveCamera)
            {
                Button videoSurveillanceButton = new Button();
                videoSurveillanceButton.Text = "Video Surveillance";
                buttonList.Add(videoSurveillanceButton);

                videoSurveillanceButton.Clicked += (sender, args) =>
                {
                    this.Navigation.PushAsync(new ModelShowcasePage(
                        "Video Surveillance",
                        "Track moving foreground objects from the camera.",
                        demoGlyph("video"),
                        () => new VideoSurveillanceModel(),
                        null,
                        hasCamera: true,
                        about: "Learns the static background from the camera, then flags and tracks anything that moves.",
                    tag: null,
                    category: "Video"));
                };
            }

            
            if (haveObjdetect)
            {
                Button faceDetectionButton = new Button();
                faceDetectionButton.Text = "Face Detection";
                buttonList.Add(faceDetectionButton);

                faceDetectionButton.Clicked += (sender, args) =>
                {
                    // Only offer the detectors whose required modules are
                    // available. Yunet (objdetect + dnn) is the preferred
                    // default, followed by the cascade classifier (objdetect).
                    var detectors = new List<string>();
                    if (haveDNN)
                        detectors.Add(FaceDetectionModel.Yunet);
                    detectors.Add(FaceDetectionModel.CascadeClassifier);
                    if (haveFace && haveDNN)
                        detectors.Add(FaceDetectionModel.FaceLandmark);

                    this.Navigation.PushAsync(new ModelShowcasePage(
                        "Face Detection",
                        "Find faces in a photo.",
                        demoGlyph("face"),
                        () => new FaceDetectionModel(),
                        new[]
                        {
                            new ModelShowcasePage.Sample("lena.jpg", "Portrait", MaskRcnnPage.GlyphImage),
                            new ModelShowcasePage.Sample("pedestrian.png", "People", MaskRcnnPage.GlyphImage),
                        },
                        pickerTitle: "Face detector",
                        pickerOptions: detectors.ToArray(),
                        about: "Finds faces in a photo; pick the engine — Yunet, a Haar cascade, or one that also marks facial landmarks.",
                    tag: null,
                    category: "Detection"));
                };

                Button pedestrianDetectionButton = new Button();
                pedestrianDetectionButton.Text = "Pedestrian Detection";
                buttonList.Add(pedestrianDetectionButton);

                pedestrianDetectionButton.Clicked += (sender, args) =>
                {
                    this.Navigation.PushAsync(new ModelShowcasePage(
                        "Pedestrian Detection",
                        "Detect people in a photo with HOG.",
                        demoGlyph("pedestrian"),
                        () => new PedestrianDetector(),
                        new[]
                        {
                            new ModelShowcasePage.Sample("pedestrian.png", "Street", MaskRcnnPage.GlyphImage),
                        },
                        about: "Detects people using HOG — a classic, lightweight person-finder for surveillance and counting.",
                    tag: null,
                    category: "Detection"));
                };

            }

            

            
            if (haveWechatQRCode && haveObjdetect
              //The WeChatQRCode detector now works on iOS since the model was
              //switched from Caffe to ONNX (2026-06), so the previous
              //iOS-only exclusion (added 2022) has been removed.
              )
            {
                Button barcodeQrcodeDetectionButton = new Button();
                barcodeQrcodeDetectionButton.Text = "Barcode and QRCode Detection";
                buttonList.Add(barcodeQrcodeDetectionButton);
                barcodeQrcodeDetectionButton.Clicked += (sender, args) =>
                {
                    this.Navigation.PushAsync(new ModelShowcasePage(
                        "Barcode & QRCode",
                        "Detect and decode barcodes and QR codes.",
                        demoGlyph("barcode"),
                        () => new CombinedModel(new BarcodeDetectorModel(), new WeChatQRCodeDetector()),
                        new[]
                        {
                            new ModelShowcasePage.Sample("qrcode_barcode.png", "QR & barcode", MaskRcnnPage.GlyphImage),
                        },
                        about: "Detects and decodes barcodes and QR codes, returning the value they encode.",
                    tag: null,
                    category: "Detection"));
                };
            }

            if (haveViz)
            {
                Button viz3dButton = new Button();
                viz3dButton.Text = "Simple 3D reconstruction";

                buttonList.Add(viz3dButton);

                viz3dButton.Clicked += async (sender, args) =>
                {
                    using (Mat left = new Mat())
                    using (Stream streamL = await FileSystem.OpenAppPackageFileAsync("imL.png"))
                    using (Mat right = new Mat())
                    using (Stream streamR = await FileSystem.OpenAppPackageFileAsync("imR.png"))
                    using (Mat points = new Mat())
                    using (Mat colors = new Mat())
                    {
                        CvInvoke.Imdecode(streamL, ImreadModes.ColorBgr, left);
                        CvInvoke.Imdecode(streamR, ImreadModes.ColorBgr, right);
                        Simple3DReconstruct.GetPointAndColor(left, right, points, colors);
                        Viz3d v = Simple3DReconstruct.GetViz3d(points, colors);
                        v.Spin();
                    }
                };
            }

            {
                //FontFace rendering is always available (imgproc module).
                Button unicodeRenderingButton = new Button();
                unicodeRenderingButton.Text = "Unicode Rendering";

                buttonList.Add(unicodeRenderingButton);

                unicodeRenderingButton.Clicked += (sender, args) =>
                {
                    this.Navigation.PushAsync(new UnicodeRenderingPage(demoGlyph("unicode")));
                };
            }
            
            // ---------- Palette: plain white storefront, black type, grey details ----------
            string boldFont = "InterSemiBold";
            string bodyFont = "InterRegular";
            const string IconFont = "MaterialSymbols";

            Color pageBackground = DemoTheme.PageBackground;
            Color primaryText = DemoTheme.PrimaryText;
            Color secondaryText = DemoTheme.SecondaryText;
            Color mutedText = DemoTheme.MutedText;
            Color bannerBackground = DemoTheme.BannerBackground;
            Color pillBackground = DemoTheme.PillBackground;
            Color hairline = DemoTheme.Hairline;
            const double SideMargin = 28;
            // ---------- Icon helper (Material Symbols glyphs) ----------
            const string gInfo = "", gSearch = "", gSparkle = "", gChevron = "";

            Func<string, Color, double, Image> makeIcon = (glyph, color, size) => new Image
            {
                Source = new FontImageSource { FontFamily = IconFont, Glyph = glyph, Color = color, Size = size },
                WidthRequest = size,
                HeightRequest = size,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            demoGlyph = (text) =>
            {
                string t = (text ?? "").ToLowerInvariant();
                if (t.Contains("shape")) return "";                              // crop_free
                if (t.Contains("mask rcnn")) return "";                          // theater_comedy
                if (t.Contains("stop sign")) return "";                          // report
                if (t.Contains("yolo")) return "";                               // gps_fixed
                if (t.Contains("scene text")) return "";                         // match_case
                if (t.Contains("tesseract") || t.Contains("ocr")) return "";     // text_fields
                if (t.Contains("license")) return "";                            // text_fields
                if (t.Contains("landmark")) return "";                           // tag_faces
                if (t.Contains("yunet")) return "";                              // face_retouching_natural
                if (t.Contains("face")) return "";                               // face
                if (t.Contains("pedestrian")) return "";                         // directions_walk
                if (t.Contains("barcode") || t.Contains("qrcode")) return "";    // grid_on
                if (t.Contains("hello")) return "";                              // waving_hand
                if (t.Contains("planar")) return "";                             // polyline
                if (t.Contains("feature matching")) return "";                   // compare
                if (t.Contains("super resolution")) return "";                   // auto_fix_high
                if (t.Contains("3d")) return "";                                 // view_in_ar
                if (t.Contains("smolvlm")) return "";                          // text_fields (VLM)
                if (t.Contains("qwen")) return "";                             // text_fields (LLM)
                if (t.Contains("unicode")) return "";                          // font_download
                if (t.Contains("video")) return "";                             // videocam
                return "";                                                       // widgets
            };

            // ---------- Sort the demos into the menu's groups ----------
            // Group 0 is "All"; the rest match categorize().
            string[] categoryNames = { "All", "Detection", "Image & 3D", "Video", "LLM" };

            Func<string, int> categorize = (text) =>
            {
                string t = (text ?? "").ToLowerInvariant();
                if (t.Contains("qwen") || t.Contains("smolvlm")) return 4;   // LLM
                if (t.Contains("video")) return 3;
                if (t.Contains("hello") || t.Contains("planar") || t.Contains("feature matching")
                    || t.Contains("super resolution") || t.Contains("3d") || t.Contains("unicode"))
                    return 2;
                return 1;
            };

            // "Yolo (DNN module)" -> name "Yolo", tag "DNN".
            Func<string, (string name, string tag)> splitName = (text) =>
            {
                text = text ?? "";
                int open = text.IndexOf('(');
                if (open < 0 || !text.EndsWith(")"))
                    return (text.Trim(), null);
                string tag = text.Substring(open + 1, text.Length - open - 2);
                tag = System.Text.RegularExpressions.Regex.Replace(tag, @"\s*module\s*$", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();
                return (text.Substring(0, open).Trim(), tag.Length > 0 ? tag : null);
            };

            // ---------- One product-style tile per demo ----------
            var tiles = new List<(View tile, string name, int category)>();
            var tilePictures = new List<(Grid picture, Image icon)>();
            foreach (View v in buttonList)
            {
                if (!(v is Button b) || !b.IsVisible)
                    continue;

                string fullText = b.Text;
                var (name, tag) = splitName(fullText);
                int category = categorize(fullText);

                var picture = new Grid { HeightRequest = 150, BackgroundColor = pageBackground };
                var tileIcon = makeIcon(demoGlyph(fullText), primaryText, 76);
                picture.Add(tileIcon);
                tilePictures.Add((picture, tileIcon));
                if (tag != null)
                {
                    picture.Add(new Border
                    {
                        BackgroundColor = pillBackground,
                        Stroke = Colors.Transparent,
                        StrokeThickness = 0,
                        StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(14) },
                        Padding = new Thickness(11, 5),
                        HorizontalOptions = LayoutOptions.End,
                        VerticalOptions = LayoutOptions.Start,
                        Content = new Label { Text = tag, FontFamily = bodyFont, FontSize = 13, TextColor = primaryText }
                    });
                }

                var tile = new VerticalStackLayout
                {
                    Spacing = 3,
                    Children =
                    {
                        picture,
                        new Label
                        {
                            Text = name,
                            FontFamily = boldFont,
                            FontSize = 18,
                            TextColor = primaryText,
                            Margin = new Thickness(0, 14, 0, 0),
                            LineBreakMode = LineBreakMode.WordWrap,
                            // Long names like "Scene Text detection" wrap rather than get cut off.
                            MaxLines = 2
                        },
                        new Label { Text = categoryNames[category], FontFamily = bodyFont, FontSize = 14, TextColor = secondaryText },
                        new Label { Text = tag != null ? tag.ToLowerInvariant() + " module" : "opencv", FontFamily = bodyFont, FontSize = 12, TextColor = mutedText }
                    }
                };

                // Tapping the tile fires the original button's Clicked handler.
                Button source = b;
                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) => ((Microsoft.Maui.IButton)source).Clicked();
                tile.GestureRecognizers.Add(tap);

                tiles.Add((tile, name, category));
            }

            // ---------- Tile grid; the column count follows the page width ----------
            var tileGrid = new Grid { ColumnSpacing = 24, RowSpacing = 40, Margin = new Thickness(SideMargin, 0, SideMargin, 48) };
            var emptyLabel = new Label
            {
                Text = "no matching demos",
                FontFamily = bodyFont,
                FontSize = 14,
                TextColor = secondaryText,
                Margin = new Thickness(SideMargin, 0, SideMargin, 48),
                IsVisible = false
            };

            var searchEntry = new Entry
            {
                Placeholder = "search",
                FontFamily = bodyFont,
                FontSize = 15,
                TextColor = primaryText,
                PlaceholderColor = mutedText,
                BackgroundColor = Colors.Transparent
            };
            var searchBar = new VerticalStackLayout
            {
                IsVisible = false,
                Margin = new Thickness(SideMargin, 0, SideMargin, 0),
                Children = { searchEntry, new BoxView { HeightRequest = 1, Color = primaryText } }
            };

            int currentCategory = 0;
            double lastWidth = -1;
            Action layoutTiles = () =>
            {
                double width = this.Width > 0 ? this.Width : 400;
                int columns = Math.Max(2, (int)((width - 2 * SideMargin + 24) / (210 + 24)));

                // Phones get shorter picture boxes, smaller icons and tighter rows, so
                // a two-column grid isn't mostly white space.
                bool compact = width < 600;
                foreach (var (picture, icon) in tilePictures)
                {
                    picture.HeightRequest = compact ? 96 : 150;
                    icon.WidthRequest = icon.HeightRequest = compact ? 52 : 76;
                    ((FontImageSource)icon.Source).Size = compact ? 52 : 76;
                }
                tileGrid.RowSpacing = compact ? 28 : 40;

                string q = (searchEntry.Text ?? "").Trim().ToLowerInvariant();
                var shown = tiles.Where(t => q.Length > 0
                    ? t.name.ToLowerInvariant().Contains(q)
                    : currentCategory == 0 || t.category == currentCategory).ToList();

                tileGrid.Children.Clear();
                tileGrid.ColumnDefinitions.Clear();
                tileGrid.RowDefinitions.Clear();
                for (int c = 0; c < columns; c++)
                    tileGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                for (int r = 0; r < (shown.Count + columns - 1) / columns; r++)
                    tileGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                for (int i = 0; i < shown.Count; i++)
                    tileGrid.Add(shown[i].tile, i % columns, i / columns);

                emptyLabel.IsVisible = shown.Count == 0;
            };

            // ---------- Top banner ----------
            var banner = new Label
            {
                Text = "OpenCV 5.1 · free & open source on every platform",
                FontFamily = bodyFont,
                FontSize = 12,
                TextColor = primaryText,
                BackgroundColor = bannerBackground,
                HorizontalTextAlignment = TextAlignment.Center,
                Padding = new Thickness(12, 7)
            };

            // ---------- Group filter: a row of pills under the wordmark ----------
            // The selected group is a solid black pill; the others are grey. Each
            // shows how many demos it holds, so the row reads clearly as a filter.
            var pills = new List<(Border pill, Label label, int category)>();
            var pillRow = new HorizontalStackLayout { Spacing = 10 };
            Action refreshMenu = () =>
            {
                bool searching = (searchEntry.Text ?? "").Length > 0;
                foreach (var (pill, label, category) in pills)
                {
                    bool selected = !searching && category == currentCategory;
                    pill.BackgroundColor = selected ? primaryText : pillBackground;
                    label.TextColor = selected ? pageBackground : primaryText;
                }
            };
            for (int i = 0; i < categoryNames.Length; i++)
            {
                int count = i == 0 ? tiles.Count : tiles.Count(t => t.category == i);
                // Hide a group that has no demos on this build (e.g. no LLM support).
                if (count == 0)
                    continue;
                int idx = i;
                var label = new Label
                {
                    FontFamily = bodyFont,
                    FontSize = 14,
                    CharacterSpacing = 0.5,
                    VerticalOptions = LayoutOptions.Center,
                    FormattedText = new FormattedString
                    {
                        Spans =
                        {
                            new Span { Text = categoryNames[i].ToUpperInvariant() },
                            new Span { Text = "  " + count, FontSize = 11 }
                        }
                    }
                };
                var pill = new Border
                {
                    Stroke = Colors.Transparent,
                    StrokeThickness = 0,
                    StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(20) },
                    Padding = new Thickness(18, 10),
                    Content = label
                };
                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) =>
                {
                    currentCategory = idx;
                    searchEntry.Text = "";
                    refreshMenu();
                    layoutTiles();
                };
                pill.GestureRecognizers.Add(tap);
                pills.Add((pill, label, i));
                pillRow.Children.Add(pill);
            }
            var categoryBar = new ScrollView
            {
                Orientation = ScrollOrientation.Horizontal,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                Margin = new Thickness(SideMargin, 0, 0, 44),
                Content = pillRow
            };

            Func<string, Action, View> iconButton = (glyph, onTap) =>
            {
                var icon = makeIcon(glyph, primaryText, 24);
                icon.Margin = new Thickness(6, 0);
                var t = new TapGestureRecognizer();
                t.Tapped += (s, e) => onTap();
                icon.GestureRecognizers.Add(t);
                return icon;
            };

            searchEntry.TextChanged += (s, e) =>
            {
                refreshMenu();
                layoutTiles();
            };

            // ---------- Top row: blurb on the left, search / info on the right ----------
            var blurb = new Label
            {
                // Phones get a shorter line; bigger screens have room for the full one.
                Text = DeviceInfo.Idiom == DeviceIdiom.Phone
                    ? "opencv for .net, real-time vision and on-device ai on every platform."
                    : "a cross-platform .net wrapper for opencv: real-time computer vision and on-device ai on windows, mac, linux, ios and android.",
                FontFamily = bodyFont,
                FontSize = 11,
                LineHeight = 1.35,
                TextColor = secondaryText,
                // Wider on big screens so it sits in two short lines instead of four.
                MaximumWidthRequest = DeviceInfo.Idiom == DeviceIdiom.Phone ? 250 : 420,
                HorizontalOptions = LayoutOptions.Start
            };

            var topRow = new Grid
            {
                Padding = new Thickness(SideMargin, 22, SideMargin - 6, 12),
                ColumnSpacing = 12,
                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) }
            };
            topRow.Add(blurb, 0, 0);
            topRow.Add(new HorizontalStackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Children =
                {
                    iconButton(gSearch, () =>
                    {
                        searchBar.IsVisible = !searchBar.IsVisible;
                        if (searchBar.IsVisible)
                            searchEntry.Focus();
                        else
                            searchEntry.Text = "";
                    }),
                    iconButton(SettingsPage.GlyphSettings, () => this.Navigation.PushAsync(new SettingsPage()))
                }
            }, 1, 0);

            // ---------- The giant wordmark ----------

            var wordmark = new Label
            {
                Text = "emgu cv",
                FontFamily = boldFont,
                TextColor = primaryText,
                LineBreakMode = LineBreakMode.NoWrap,
                Margin = new Thickness(SideMargin - 4, 56, SideMargin, 28)
            };

            // Size the wordmark to span the page: Inter SemiBold's "emgu cv" is about
            // 3.8 em wide with the tightened spacing below.
            Action fitWordmark = () =>
            {
                double width = this.Width > 0 ? this.Width : 400;
                // Capped so on a big window it no longer eats half the screen.
                double size = Math.Clamp((width - 2 * SideMargin) / 3.85, 48, 150);
                wordmark.FontSize = size;
                wordmark.CharacterSpacing = -size * 0.06;
                wordmark.Padding = new Thickness(0, 0, size * 0.1, 0);
                // The font reserves room above lowercase letters for capitals; pull the
                // wordmark up by that much. Phones also get much less padding around it.
                bool compact = width < 600;
                wordmark.Margin = new Thickness(SideMargin - 4, (compact ? 12 : 20) - size * 0.2, SideMargin, compact ? 16 : 24);
                categoryBar.Margin = new Thickness(SideMargin, 0, 0, compact ? 24 : 44);
            };

            this.SizeChanged += (s, e) =>
            {
                if (Math.Abs(this.Width - lastWidth) < 1)
                    return;
                lastWidth = this.Width;
                fitWordmark();
                layoutTiles();
            };

            refreshMenu();
            fitWordmark();
            layoutTiles();

            this.BackgroundColor = pageBackground;
            this.Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Children = { banner, topRow, searchBar, wordmark, categoryBar, tileGrid, emptyLabel }
                }
            };
        }


        private enum DnnBackendType
        {
            Default,
            InferenceEngineOnly
        }

        private String[] GetDnnBackends(DnnBackendType backendType = DnnBackendType.Default)
        {
            var openCVConfigDict = CvInvoke.ConfigDict;
            bool haveDNN = (openCVConfigDict["HAVE_OPENCV_DNN"] != 0);

            if (haveDNN)
            {
                var dnnBackends = DnnInvoke.AvailableBackends;
                List<String> dnnBackendsText = new List<string>();
                foreach (var dnnBackend in dnnBackends)
                {
                    if (backendType == DnnBackendType.InferenceEngineOnly &&
                        !((dnnBackend.Backend == Emgu.CV.Dnn.Backend.InferenceEngine)
                          || (dnnBackend.Backend == Emgu.CV.Dnn.Backend.InferenceEngineNgraph)
                          || (dnnBackend.Backend == Emgu.CV.Dnn.Backend.InferenceEngineNnBuilder2019)))
                        continue;
                    dnnBackendsText.Add(String.Format("{0};{1}", dnnBackend.Backend, dnnBackend.Target));
                }

                return dnnBackendsText.ToArray();
            }
            else
            {
                return new string[0];
            }
        }

    }
}