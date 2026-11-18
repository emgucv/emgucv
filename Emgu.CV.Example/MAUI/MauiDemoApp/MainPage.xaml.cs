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

            
            maskRcnnButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new MaskRcnnPage());
            };


            sceneTextDetectionButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new ModelStagePage(
                    "Scene Text",
                    "Find and read English and Chinese text in a photo.",
                    () => new SceneTextDetector(),
                    new[]
                    {
                        new StageSample("cars_license_plate.png", "license plate"),
                        new StageSample("test_image.png", "document"),
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
                    category: "Detection",
                    module: "dnn module",
                    itemSingular: "word", itemPlural: "words",
                    codeSnippet: @"using Emgu.CV;
using Emgu.CV.Models;

using var detector = new SceneTextDetector();
await detector.Init();

using Mat image = CvInvoke.Imread(""plate.png"");
foreach (DetectedObject word in detector.Detect(image))
    Console.WriteLine($""{word.Label} at {word.Region}"");"));
            };
            yoloButton.Clicked += (sender, args) =>
            {
                this.Navigation.PushAsync(new YoloPage());
            };

            superresButton.Clicked += (sender, args) =>
            {
                // The model name must be the first token; the text after it is a
                // description (Superres.Init only parses the leading token).
                this.Navigation.PushAsync(new ModelStagePage(
                    "Super Resolution",
                    "Upscale a photo with a neural network.",
                    () => new Superres(),
                    new[]
                    {
                        new StageSample("dog416.png", "dog & bike"),
                        new StageSample("lena.jpg", "portrait"),
                    },
                    optionsTitle: "Model",
                    options: new[]
                    {
                        "FsrcnnX2",
                        "FsrcnnX3",
                        "EspcnX2",
                    },
                    hasCamera: false,
                    about: "Uses a neural network to upscale and sharpen an image, reconstructing fine detail (2×–3×).\n\nFSRCNN ×2 and ×3 are tiny and fastest; ESPCN ×2 is small and fast. Use Compare to drag between the original and the upscaled result.",
                    runLabel: "Upscale",
                    tag: "DNN",
                    category: "Image & 3D",
                    module: "dnn_superres",
                    codeSnippet: @"using Emgu.CV;
using Emgu.CV.Models;

using var superres = new Superres();
await superres.Init(null, ""{option}"");

using Mat image = CvInvoke.Imread(""dog.png"");
using Mat bigger = new Mat();
superres.Upsample(image, bigger);"));
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
                    this.Navigation.PushAsync(new ModelStagePage(
                        "Tesseract OCR",
                        "Read printed text from an image.",
                        () => new TesseractModel(),
                        new[]
                        {
                            new StageSample("test_image.png", "document"),
                        },
                        hasCamera: false,
                        about: "Classic document OCR — reads printed text from an image into editable text.",
                    tag: "Tesseract",
                    category: "Detection",
                    module: "tesseract",
                    runLabel: "Read text",
                    itemSingular: "line", itemPlural: "lines",
                    codeSnippet: @"using Emgu.CV;
using Emgu.CV.Models;

using var ocr = new TesseractModel(""eng"");
await ocr.Init();

using Mat image = CvInvoke.Imread(""document.png"");
using Mat render = image.Clone();
Console.WriteLine(ocr.ProcessAndRender(image, render));"));
                };
            }

            if (haveDNN)
            {
                Button paddleOcrButton = new Button();
                paddleOcrButton.Text = "PaddleOCR";
                buttonList.Add(paddleOcrButton);

                paddleOcrButton.Clicked += (sender, args) =>
                {
                    this.Navigation.PushAsync(new ModelStagePage(
                        "PaddleOCR",
                        "Detect and read text with PP-OCRv4.",
                        () => new PaddleOCR(),
                        new[]
                        {
                            new StageSample("cars_license_plate.png", "license plate"),
                            new StageSample("test_image.png", "document"),
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
                    category: "Detection",
                    module: "dnn module",
                    runLabel: "Read text",
                    itemSingular: "line", itemPlural: "lines",
                    codeSnippet: @"using Emgu.CV;
using Emgu.CV.Models;

using var ocr = new PaddleOCR();
await ocr.Init();

using Mat image = CvInvoke.Imread(""document.png"");
foreach (PaddleOCR.OcrResult r in ocr.Recognize(image))
    Console.WriteLine(r.Text);"));
                };
            }


            if (haveVideo && haveCamera)
            {
                Button videoSurveillanceButton = new Button();
                videoSurveillanceButton.Text = "Video Surveillance";
                buttonList.Add(videoSurveillanceButton);

                videoSurveillanceButton.Clicked += (sender, args) =>
                {
                    this.Navigation.PushAsync(new VideoSurveillancePage());
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

                    this.Navigation.PushAsync(new ModelStagePage(
                        "Face Detection",
                        "Find faces in a photo.",
                        () => new FaceDetectionModel(),
                        new[]
                        {
                            new StageSample("lena.jpg", "portrait"),
                            new StageSample("pedestrian.png", "people"),
                        },
                        optionsTitle: "Face detector",
                        options: detectors.ToArray(),
                        about: "Finds faces in a photo; pick the engine — Yunet, a Haar cascade, or one that also marks facial landmarks.",
                    tag: null,
                    category: "Detection",
                    module: "objdetect",
                    codeSnippet: @"using Emgu.CV;
using Emgu.CV.Models;

// FaceDetectionModel is in this demo app; it wraps
// FaceDetectorYN / CascadeClassifier / Facemark.
using var faces = new FaceDetectionModel();
await faces.Init(null, ""{option}"");

using Mat image = CvInvoke.Imread(""portrait.jpg"");
using Mat render = image.Clone();
faces.ProcessAndRender(image, render);"));
                };

                Button pedestrianDetectionButton = new Button();
                pedestrianDetectionButton.Text = "Pedestrian Detection";
                buttonList.Add(pedestrianDetectionButton);

                pedestrianDetectionButton.Clicked += (sender, args) =>
                {
                    this.Navigation.PushAsync(new ModelStagePage(
                        "Pedestrian Detection",
                        "Detect people in a photo with HOG.",
                        () => new PedestrianDetector(),
                        new[]
                        {
                            new StageSample("pedestrian.png", "street"),
                        },
                        stageProcess: (model, input) =>
                        {
                            // HOG returns rectangles: draw them live and count people.
                            var sw = System.Diagnostics.Stopwatch.StartNew();
                            System.Drawing.Rectangle[] people = ((PedestrianDetector)model).Find(input);
                            sw.Stop();
                            var lime = DemoTheme.Lime;
                            return new StageResult
                            {
                                Boxes = people.Select(r => new StageBox(new RectF(r.X, r.Y, r.Width, r.Height), "person", lime)).ToArray(),
                                Rows = people.Select((r, i) => new StageRow($"person {i + 1}", $"{r.Width}×{r.Height} px", null, lime)).ToArray(),
                                InferenceMs = sw.Elapsed.TotalMilliseconds,
                            };
                        },
                        itemSingular: "person", itemPlural: "people",
                        about: "Detects people using HOG — a classic, lightweight person-finder for surveillance and counting.",
                    tag: null,
                    category: "Detection",
                    module: "objdetect · HOG",
                    codeSnippet: @"using Emgu.CV;
using Emgu.CV.Models;

using var detector = new PedestrianDetector();
await detector.Init();

using Mat image = CvInvoke.Imread(""street.png"");
foreach (var person in detector.Find(image))
    Console.WriteLine($""person at {person}"");"));
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
                    this.Navigation.PushAsync(new ModelStagePage(
                        "Barcode & QRCode",
                        "Detect and decode barcodes and QR codes.",
                        () => new CombinedModel(new BarcodeDetectorModel(), new WeChatQRCodeDetector()),
                        new[]
                        {
                            new StageSample("qrcode_barcode.png", "qr & barcode"),
                        },
                        about: "Detects and decodes barcodes and QR codes, returning the value they encode.",
                    tag: null,
                    category: "Detection",
                    module: "objdetect · wechat_qrcode",
                    codeSnippet: @"using Emgu.CV;
using Emgu.CV.Models;

using var model = new CombinedModel(
    new BarcodeDetectorModel(), new WeChatQRCodeDetector());
await model.Init();

using Mat image = CvInvoke.Imread(""codes.png"");
using Mat render = image.Clone();
Console.WriteLine(model.ProcessAndRender(image, render));"));
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
                    this.Navigation.PushAsync(new UnicodeRenderingPage());
                };
            }
            
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

            BuildHome(buttonList);
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