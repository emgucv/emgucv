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

            Button licensePlateRecognitionButton = new Button();
            licensePlateRecognitionButton.Text = "License Plate Recognition (DNN Module)";

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
                licensePlateRecognitionButton,
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


            bool hasInferenceEngine = false;
            if (haveDNN)
            {
                var dnnBackends = DnnInvoke.AvailableBackends;
                hasInferenceEngine = Array.Exists(dnnBackends, dnnBackend =>
                    (dnnBackend.Backend == Emgu.CV.Dnn.Backend.InferenceEngine
                     || dnnBackend.Backend == Emgu.CV.Dnn.Backend.InferenceEngineNgraph
                     || dnnBackend.Backend == Emgu.CV.Dnn.Backend.InferenceEngineNnBuilder2019));

#if DEBUG
                DnnInvoke.EnableModelDiagnostics(true);
#endif
            }

            bool haveCamera = true;

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
                this.Navigation.PushAsync(new HelloWorldPage());
            };

            
            planarSubdivisionButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new PlanarSubdivisionPage());
            };


            
            shapeDetectionButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new ShapeDetectionPage());
            };

            
            featureDetectionButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new FeatureMatchingPage());
            };

            
            //licensePlateRecognitionButton.Clicked += (sender, args) =>
            //{
            //    ProcessAndRenderPage vehicleLicensePlateDetectorPage = new ProcessAndRenderPage(
            //        new VehicleLicensePlateDetector(),
            //        "Perform License Plate Recognition",
            //        "cars_license_plate.png",
            //        "This demo is based on the security barrier camera demo in the OpenVino model zoo. The models is trained with BIT-vehicle dataset. License plate is trained based on Chinese license plate that has white character on blue background. You will need to re-train your own model if you intend to use this in other countries.");
            //    Picker p = vehicleLicensePlateDetectorPage.Picker;
            //    p.IsVisible = true;
            //    p.Title = "Preferred DNN backend & target";

            //    foreach (String option in GetDnnBackends(DnnBackendType.InferenceEngineOnly))
            //    {
            //        p.Items.Add(option);
            //    }

            //    this.Navigation.PushAsync(vehicleLicensePlateDetectorPage);
            //};

            maskRcnnButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new MaskRcnnPage());
            };


            sceneTextDetectionButton.Clicked += (sender, args) =>
            {
                ProcessAndRenderPage sceneTextDetectionPage = new ProcessAndRenderPage(
                    new SceneTextDetector(),
                    "Perform Scene Text Detection",
                    "cars_license_plate.png",
                    "This model is trained on MSRA-TD500, so it can detect both English and Chinese text instances.");
                this.Navigation.PushAsync(sceneTextDetectionPage);
            };
            yoloButton.Clicked += (sender, args) =>
            {
                ProcessAndRenderPage yoloPage = new ProcessAndRenderPage(
                    new Yolo(),
                    "Yolo Detection",
                    "dog416.png",
                    "");
                Picker p = yoloPage.Picker;
                p.Title = "Yolo model version";
                p.IsVisible = true;
                p.Items.Add("Yolo12N");
                p.Items.Add("Yolo12S");
                p.Items.Add("Yolo12M");
                p.Items.Add("Yolo12L");
                p.Items.Add("Yolo12X");
                p.Items.Add("Yolo11N");
                p.Items.Add("Yolo11S");
                p.Items.Add("Yolo11M");
                p.Items.Add("Yolo11L");
                p.Items.Add("Yolo11X");
                p.Items.Add("YoloV10N");
                p.Items.Add("YoloV10S");
                p.Items.Add("YoloV10M");
                p.Items.Add("YoloV10B");
                p.Items.Add("YoloV10L");
                p.Items.Add("YoloV10X");
                p.Items.Add("YoloV8N");
                this.Navigation.PushAsync(yoloPage);
            };

            superresButton.Clicked += (sender, args) =>
            {
                ProcessAndRenderPage superresPage = new ProcessAndRenderPage(
                    new Superres(),
                    "Super resolution",
                    "dog416.png",
                    "");
                Picker p = superresPage.Picker;
                p.Title = "Super resolution version";
                p.IsVisible = true;
                //The model name must be the first token; the text after it is a
                //description (Superres.Init only parses the leading token).
                p.Items.Add("EdsrX2 - 2x, best quality, slowest, large download");
                p.Items.Add("EdsrX3 - 3x, best quality, slowest, large download");
                p.Items.Add("EdsrX4 - 4x, best quality, slowest, large download");
                p.Items.Add("EspcnX2 - 2x, fast, small model");
                p.Items.Add("EspcnX3 - 3x, fast, small model");
                p.Items.Add("EspcnX4 - 4x, fast, small model");
                p.Items.Add("FsrcnnX2 - 2x, fastest, tiny model");
                p.Items.Add("FsrcnnX3 - 3x, fastest, tiny model");
                p.Items.Add("FsrcnnX4 - 4x, fastest, tiny model");
                p.Items.Add("LapsrnX2 - 2x, balanced speed and quality");
                p.Items.Add("LapsrnX4 - 4x, balanced speed and quality");
                p.Items.Add("LapsrnX8 - 8x, balanced speed and quality");


                this.Navigation.PushAsync(superresPage);
            };

            maskRcnnButton.IsVisible = haveDNN;
            //faceLandmarkDetectionButton.IsVisible = haveDNN;
            yoloButton.IsVisible = haveDNN;
            superresButton.IsVisible = haveDNN;
            //Scene text rendering uses the FontFace class from the imgproc module,
            //no longer requires the freetype contrib module.
            sceneTextDetectionButton.IsVisible = haveDNN;
            //licensePlateRecognitionButton.IsVisible = hasInferenceEngine;
            licensePlateRecognitionButton.IsVisible = false;
            featureDetectionButton.IsVisible = haveFeatures;
            
            
            if (haveTesseract)
            {
                Button ocrButton = new Button();
                ocrButton.Text = "Tesseract OCR";
                buttonList.Add(ocrButton);

                ocrButton.Clicked += (sender, args) =>
                {
                    ProcessAndRenderPage ocrPage = new ProcessAndRenderPage(
                        new TesseractModel(),
                        "Perform Text Detection",
                        "test_image.png",
                        "");
                    ocrPage.HasCameraOption = false;
                    this.Navigation.PushAsync(ocrPage);
                };
            } 

            if (haveVideo && haveCamera)
            {
                Button videoSurveillanceButton = new Button();
                videoSurveillanceButton.Text = "Video Surveillance";
                buttonList.Add(videoSurveillanceButton);

                videoSurveillanceButton.Clicked += (sender, args) =>
                {
                    ProcessAndRenderPage videoPage = new ProcessAndRenderPage(
                        new VideoSurveillanceModel(),
                        "Open Camera",
                        null,
                        "");
                    videoPage.HasCameraOption = true;
                    this.Navigation.PushAsync(videoPage);
                };
            }

            
            if (haveObjdetect)
            {
                Button faceDetectionButton = new Button();
                faceDetectionButton.Text = "Face Detection";
                buttonList.Add(faceDetectionButton);

                faceDetectionButton.Clicked += (sender, args) =>
                {
                    ProcessAndRenderPage faceDetectionPage = new ProcessAndRenderPage(
                        new FaceDetectionModel(),
                        "Face Detection",
                        "lena.jpg",
                        "");
                    Picker p = faceDetectionPage.Picker;
                    p.Title = "Face detector";
                    //Only offer the detectors whose required modules are
                    //available. Yunet (objdetect + dnn) is the preferred
                    //default, followed by the cascade classifier (objdetect).
                    if (haveDNN)
                        p.Items.Add(FaceDetectionModel.Yunet);
                    p.Items.Add(FaceDetectionModel.CascadeClassifier);
                    if (haveFace && haveDNN)
                        p.Items.Add(FaceDetectionModel.FaceLandmark);
                    p.SelectedIndex = 0;
                    p.IsVisible = p.Items.Count > 1;
                    this.Navigation.PushAsync(faceDetectionPage);
                };

                Button pedestrianDetectionButton = new Button();
                pedestrianDetectionButton.Text = "Pedestrian Detection";
                buttonList.Add(pedestrianDetectionButton);

                pedestrianDetectionButton.Clicked += (sender, args) =>
                {
                    ProcessAndRenderPage pedestrianDetectorPage = new ProcessAndRenderPage(
                        new PedestrianDetector(),
                        "Pedestrian detection",
                        "pedestrian.png",
                        "HOG pedestrian detection");
                    this.Navigation.PushAsync(pedestrianDetectorPage);
                };

            }

            

            
            if (haveWechatQRCode && haveObjdetect
              //TODO: WeChatQRCode detector doesn't work on iOS, probably a bug in iOS
              //Will need to figure out why.
              && (Microsoft.Maui.Devices.DeviceInfo.Platform != DevicePlatform.iOS)
              )
            {
                Button barcodeQrcodeDetectionButton = new Button();
                barcodeQrcodeDetectionButton.Text = "Barcode and QRCode Detection";
                buttonList.Add(barcodeQrcodeDetectionButton);
                barcodeQrcodeDetectionButton.Clicked += (sender, args) =>
                {
                    BarcodeDetectorModel barcodeDetector = new BarcodeDetectorModel();
                    WeChatQRCodeDetector qrcodeDetector = new WeChatQRCodeDetector();
                    CombinedModel combinedModel = new CombinedModel(barcodeDetector, qrcodeDetector);

                    ProcessAndRenderPage barcodeQrcodeDetectionPage = new ProcessAndRenderPage(
                        combinedModel,
                        "Perform Barcode and QRCode Detection",
                        "qrcode_barcode.png",
                        "");
                    this.Navigation.PushAsync(barcodeQrcodeDetectionPage);
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
                //FontFace rendering is always available (imgproc module); the page
                //offers the freetype module as an additional option when present.
                Button unicodeRenderingButton = new Button();
                unicodeRenderingButton.Text = "Unicode Rendering";

                buttonList.Add(unicodeRenderingButton);

                unicodeRenderingButton.Clicked += (sender, args) =>
                {
                    this.Navigation.PushAsync(new UnicodeRenderingPage());
                };
            }
            
            // ---------- Palette ----------
            string titleFont = "InterSemiBold";
            string bodyFont = "InterRegular";
            string mediumFont = "InterSemiBold";
            const string IconFont = "MaterialSymbols";

            Color pageBackground = Color.FromArgb("#EEF1F8");
            Color cardBackground = Colors.White;
            Color primaryText = Color.FromArgb("#1A1C2E");
            Color secondaryText = Color.FromArgb("#8A8FA3");
            Color accent = Color.FromArgb("#3D7BF7");
            Color rowBorder = Color.FromArgb("#ECEEF5");
            Color tileBackground = Color.FromArgb("#E8EFFE");
            Color chevronColor = Color.FromArgb("#C2C7D6");

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

            Func<string, string> demoGlyph = (text) =>
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
                if (t.Contains("free type")) return "";                          // font_download
                if (t.Contains("video")) return "";                             // videocam
                return "";                                                       // widgets
            };

            // ---------- Sort the demo buttons into broad groups ----------
            string[] categoryNames = { "Detection", "Image & 3D", "Video" };
            string[] pillGlyphs = { "", "", "" }; // crop_free, view_in_ar, smart_display // crop_free, view_in_ar, smart_display
            var categoryDemos = new List<Button>[] { new List<Button>(), new List<Button>(), new List<Button>() };

            Func<string, int> categorize = (text) =>
            {
                string t = (text ?? "").ToLowerInvariant();
                if (t.Contains("video")) return 2;
                if (t.Contains("hello") || t.Contains("planar") || t.Contains("feature matching")
                    || t.Contains("super resolution") || t.Contains("3d") || t.Contains("free type"))
                    return 1;
                return 0;
            };

            var demoNames = new Dictionary<Button, string>();
            foreach (View v in buttonList)
            {
                if (v is Button b && b.IsVisible)
                {
                    demoNames[b] = b.Text;
                    categoryDemos[categorize(b.Text)].Add(b);
                }
            }

            // ---------- Build a rich row (icon tile + name + chevron) for a demo ----------
            Func<Button, View> buildRow = (b) =>
            {
                string name = demoNames.TryGetValue(b, out var nm) ? nm : b.Text;

                var tile = new Border
                {
                    WidthRequest = 42,
                    HeightRequest = 42,
                    BackgroundColor = tileBackground,
                    Stroke = Colors.Transparent,
                    StrokeThickness = 0,
                    StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(11) },
                    VerticalOptions = LayoutOptions.Center,
                    Content = makeIcon(demoGlyph(name), accent, 22)
                };

                var nameLabel = new Label
                {
                    Text = name,
                    FontFamily = bodyFont,
                    FontSize = 16,
                    TextColor = primaryText,
                    VerticalOptions = LayoutOptions.Center,
                    LineBreakMode = LineBreakMode.TailTruncation
                };

                var chevron = makeIcon(gChevron, chevronColor, 22);
                chevron.HorizontalOptions = LayoutOptions.End;

                var grid = new Grid
                {
                    Padding = new Thickness(12, 12),
                    ColumnSpacing = 12,
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(GridLength.Auto),
                        new ColumnDefinition(GridLength.Star),
                        new ColumnDefinition(GridLength.Auto)
                    }
                };
                grid.Add(tile, 0, 0);
                grid.Add(nameLabel, 1, 0);
                grid.Add(chevron, 2, 0);

                // Reuse the original button as a transparent tap layer over the whole row.
                b.Text = "";
                b.BackgroundColor = Colors.Transparent;
                b.BorderWidth = 0;
                b.CornerRadius = 14;
                b.Margin = new Thickness(0);
                b.Padding = new Thickness(0);
                b.HorizontalOptions = LayoutOptions.Fill;
                b.VerticalOptions = LayoutOptions.Fill;
                grid.Add(b, 0, 0);
                Grid.SetColumnSpan(b, 3);

                return new Border
                {
                    BackgroundColor = cardBackground,
                    Stroke = rowBorder,
                    StrokeThickness = 1,
                    StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(14) },
                    Content = grid
                };
            };

            // Pre-build all rows once (avoids re-parenting issues when switching categories).
            var categoryRows = new List<(View row, string name)>[categoryNames.Length];
            for (int c = 0; c < categoryNames.Length; c++)
            {
                categoryRows[c] = new List<(View, string)>();
                foreach (Button b in categoryDemos[c])
                    categoryRows[c].Add((buildRow(b), demoNames[b]));
            }

            // ---------- Section card (header + rows) ----------
            var demoStack = new VerticalStackLayout { Spacing = 10 };

            var sectionTitle = new Label
            {
                FontFamily = mediumFont,
                FontSize = 20,
                TextColor = primaryText,
                VerticalOptions = LayoutOptions.Center
            };
            var sectionHeader = new HorizontalStackLayout
            {
                Spacing = 10,
                Padding = new Thickness(6, 4, 6, 12),
                Children = { makeIcon(gSparkle, accent, 22), sectionTitle }
            };
            var sectionCard = new Border
            {
                BackgroundColor = cardBackground,
                Stroke = Colors.Transparent,
                StrokeThickness = 0,
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(24) },
                Padding = new Thickness(14, 14, 14, 16),
                Margin = new Thickness(16, 0, 16, 28),
                Content = new VerticalStackLayout { Children = { sectionHeader, demoStack } }
            };

            // ---------- Search box (toggled by the header search button) ----------
            var searchEntry = new Entry
            {
                Placeholder = "Search modules",
                FontFamily = bodyFont,
                FontSize = 16,
                TextColor = primaryText,
                PlaceholderColor = secondaryText,
                BackgroundColor = Colors.Transparent,
                VerticalOptions = LayoutOptions.Center
            };
            var searchGrid = new Grid
            {
                ColumnSpacing = 8,
                Padding = new Thickness(14, 2),
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Star)
                }
            };
            searchGrid.Add(makeIcon(gSearch, secondaryText, 20), 0, 0);
            searchGrid.Add(searchEntry, 1, 0);
            var searchCard = new Border
            {
                IsVisible = false,
                BackgroundColor = cardBackground,
                Stroke = rowBorder,
                StrokeThickness = 1,
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(14) },
                Margin = new Thickness(16, 0, 16, 0),
                Content = searchGrid
            };

            // ---------- Category pills ----------
            int currentCategory = 0;
            Action<int> selectCategory = null;

            var carouselRow = new HorizontalStackLayout { Spacing = 12, Padding = new Thickness(16, 0, 16, 0) };
            var categoryCards = new Border[categoryNames.Length];
            var categoryLabels = new Label[categoryNames.Length];
            var categoryIcons = new Image[categoryNames.Length];

            for (int i = 0; i < categoryNames.Length; i++)
            {
                int idx = i;
                var ic = makeIcon(pillGlyphs[i], primaryText, 20);
                var lbl = new Label
                {
                    Text = categoryNames[i],
                    FontFamily = mediumFont,
                    FontSize = 15,
                    TextColor = primaryText,
                    VerticalOptions = LayoutOptions.Center
                };
                var hs = new HorizontalStackLayout { Spacing = 8, VerticalOptions = LayoutOptions.Center, Children = { ic, lbl } };
                var catCard = new Border
                {
                    StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(16) },
                    Stroke = rowBorder,
                    StrokeThickness = 1,
                    BackgroundColor = cardBackground,
                    Padding = new Thickness(18, 12),
                    Content = hs
                };
                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) => selectCategory(idx);
                catCard.GestureRecognizers.Add(tap);
                categoryCards[i] = catCard;
                categoryLabels[i] = lbl;
                categoryIcons[i] = ic;
                carouselRow.Children.Add(catCard);
            }

            var carousel = new ScrollView
            {
                Orientation = ScrollOrientation.Horizontal,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                Content = carouselRow
            };

            selectCategory = (idx) =>
            {
                currentCategory = idx;
                for (int i = 0; i < categoryCards.Length; i++)
                {
                    bool sel = (i == idx);
                    categoryCards[i].BackgroundColor = sel ? accent : cardBackground;
                    categoryCards[i].Stroke = sel ? Colors.Transparent : rowBorder;
                    categoryLabels[i].TextColor = sel ? Colors.White : primaryText;
                    ((FontImageSource)categoryIcons[i].Source).Color = sel ? Colors.White : primaryText;
                }

                sectionTitle.Text = categoryNames[idx] + " Modules";

                string q = (searchEntry.Text ?? "").Trim().ToLowerInvariant();
                demoStack.Children.Clear();
                foreach (var (row, name) in categoryRows[idx])
                {
                    row.IsVisible = q.Length == 0 || name.ToLowerInvariant().Contains(q);
                    demoStack.Children.Add(row);
                }
            };

            searchEntry.TextChanged += (s, e) => selectCategory(currentCategory);

            // ---------- Header (title + subtitle + info/search buttons) ----------
            Func<string, Action, Border> circleButton = (glyph, onTap) =>
            {
                var cb = new Border
                {
                    WidthRequest = 46,
                    HeightRequest = 46,
                    BackgroundColor = cardBackground,
                    Stroke = rowBorder,
                    StrokeThickness = 1,
                    StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(23) },
                    VerticalOptions = LayoutOptions.Center,
                    Content = makeIcon(glyph, primaryText, 22)
                };
                var t = new TapGestureRecognizer();
                t.Tapped += (s, e) => onTap();
                cb.GestureRecognizers.Add(t);
                return cb;
            };

            var infoBtn = circleButton(gInfo, () => this.Navigation.PushAsync(new AboutPage()));
            var searchBtn = circleButton(gSearch, () =>
            {
                searchCard.IsVisible = !searchCard.IsVisible;
                if (searchCard.IsVisible)
                    searchEntry.Focus();
                else
                {
                    searchEntry.Text = "";
                    selectCategory(currentCategory);
                }
            });

            var titleLabel = new Label { Text = "Emgu CV", FontFamily = titleFont, FontSize = 40, TextColor = primaryText };
            var subtitleLabel = new Label
            {
                Text = "Computer Vision made simple",
                FontFamily = bodyFont,
                FontSize = 15,
                TextColor = secondaryText,
                Margin = new Thickness(0, 2, 0, 0)
            };
            var titleStack = new VerticalStackLayout { VerticalOptions = LayoutOptions.Center, Children = { titleLabel, subtitleLabel } };

            var header = new Grid
            {
                Padding = new Thickness(24, 22, 20, 8),
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                }
            };
            header.Add(titleStack, 0, 0);
            header.Add(new HorizontalStackLayout { Spacing = 10, VerticalOptions = LayoutOptions.Center, Children = { infoBtn, searchBtn } }, 1, 0);

            selectCategory(0);

            var contentLayout = new VerticalStackLayout
            {
                Spacing = 16,
                Children = { header, searchCard, carousel, sectionCard }
            };

            this.BackgroundColor = pageBackground;
            this.Content = new ScrollView { Content = contentLayout };
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