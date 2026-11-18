//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Models;

namespace MauiDemoApp
{
    /// <summary>
    /// YOLO object detection on the shared <see cref="StagePage"/> layout: model
    /// version and size pickers, download status, a confidence slider that filters
    /// instantly, boxes over the photo and measured timings.
    /// </summary>
    public class YoloPage : StagePage
    {
        // Version label -> enum prefix and the sizes that version ships in.
        private static readonly (string label, string prefix, string[] sizes)[] Versions =
        {
            ("v12", "Yolo12", new[] { "n", "s", "m", "l", "x" }),
            ("v11", "Yolo11", new[] { "n", "s", "m", "l", "x" }),
            ("v10", "YoloV10", new[] { "n", "s", "m", "b", "l", "x" }),
            ("v8", "YoloV8", new[] { "n" }),
        };

        /// <summary>Preferences key: the last measured inference time (ms), shown on the home screen.</summary>
        internal const string LastInferenceKey = "yolo_last_inference_ms";


        private Yolo _model = new Yolo();
        private string _modelLoaded;          // selection the model was initialized with
        private SegmentedControl _versionControl, _sizeControl;

        public YoloPage() : base(new StageConfig
        {
            Title = "yolo",
            Tag = "DNN",
            Category = "Detection",
            Module = "dnn module",
            Description = "Detect and label objects in a photo or live camera feed.",
            About = "Fast, general-purpose object detection: one neural network finds and labels every object in a single pass, hence \"You Only Look Once.\" It recognizes 80 everyday kinds of objects (people, animals, vehicles, household items).\n\nSizes go from n (nano, fastest) to x (extra large, most accurate).",
            Samples = new[]
            {
                new StageSample("dog416.png", "dog.png"),
                new StageSample("pedestrian.png", "street.png"),
                new StageSample("cars_license_plate.png", "cars.png"),
            },
            HasCamera = true,
            HasConfidence = true,
            ConfidenceDefault = 0.25,
            RunLabel = "Detect",
            EmptyCaption = "run a detection to see labels here",
        })
        {
        }

        protected override string BackendName
        {
            get
            {
#if __IOS__ || __MACCATALYST__ || __ANDROID__
                return "CPU · OpenCV DNN";
#else
                bool gpu = CvInvoke.HaveOnnxRuntime && Emgu.CV.Cuda.CudaInvoke.HasCuda;
                return gpu ? "GPU · ONNX Runtime" : "CPU · OpenCV DNN";
#endif
            }
        }

        protected override View BuildOptions()
        {
            _versionControl = new SegmentedControl(Versions.Select(v => v.label), 0);
            _sizeControl = new SegmentedControl(Versions[0].sizes, 0);
            _versionControl.SelectionChanged += (s, e) =>
            {
                var sizes = Versions[_versionControl.SelectedIndex].sizes;
                int keep = Array.IndexOf(sizes, _sizeControl.SelectedItem);
                _sizeControl.SetItems(sizes, keep < 0 ? 0 : keep);
                OnModelChanged();
            };
            _sizeControl.SelectionChanged += (s, e) => OnModelChanged();
            _versionControl.Margin = new Thickness(0, 0, 8, 8);
            _sizeControl.Margin = new Thickness(0, 0, 0, 8);
            return new FlexLayout
            {
                Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap,
                JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceBetween,
                Children = { _versionControl, _sizeControl }
            };
        }

        protected override void OnStatusInit() => UpdateModelLabels();

        private string Selection
        {
            get
            {
                var v = Versions[Math.Max(0, _versionControl.SelectedIndex)];
                return v.prefix + (_sizeControl.SelectedItem ?? v.sizes[0]).ToUpperInvariant();
            }
        }

        // e.g. "YOLO12n", "YOLOv10b"
        private string DisplayModelName
        {
            get
            {
                var v = Versions[Math.Max(0, _versionControl.SelectedIndex)];
                return "YOLO" + (v.prefix.Contains("V") ? "v" : "") + v.label.Substring(1) + (_sizeControl.SelectedItem ?? v.sizes[0]);
            }
        }

        // Local file the model downloads to (same folder / name Yolo.Init uses).
        private string ModelFilePath
        {
            get
            {
                string s = Selection.ToLowerInvariant();
                string file = s.StartsWith("yolov10") ? s + "_no_post_process.onnx" : s + ".onnx";
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "emgu", "yolo", file);
            }
        }

        private void OnModelChanged()
        {
            if (_modelLoaded != null && _modelLoaded != Selection)
            {
                _model?.Clear();
                _modelLoaded = null;
            }
            UpdateModelLabels();
        }

        private void UpdateModelLabels()
        {
            SetOptionsValue(DisplayModelName);
            var fi = new System.IO.FileInfo(ModelFilePath);
            if (fi.Exists)
                SetStatus($"Downloaded · {fi.Length / (1024.0 * 1024.0):0.0} MB");
            else
                SetStatus("Not downloaded", DemoTheme.MutedText, showDownload: true);
        }

        protected override async Task<bool> PrepareAsync()
        {
            if (_model == null)
                return false;
            string sel = Selection;
            if (_modelLoaded == sel && _model.Initialized)
                return true;
            _model.Clear();
            SetStatus("Loading model…", DemoTheme.Lime);
            await Task.Run(() => _model.Init(OnDownloadProgress, sel));
            if (!_model.Initialized)
            {
                UpdateModelLabels();
                SetStatus("Download failed · check your connection", DemoTheme.MutedText, showDownload: true);
                return false;
            }
            _modelLoaded = sel;
            UpdateModelLabels();
            return true;
        }

        protected override StageResult RunCore(Mat input)
        {
            var total = Stopwatch.StartNew();
            // "pre": make sure the input is a 3-channel BGR image.
            using var bgr = new Mat();
            if (input.NumberOfChannels == 4)
                CvInvoke.CvtColor(input, bgr, ColorConversion.Bgra2Bgr);
            else
                input.CopyTo(bgr);
            var infer = Stopwatch.StartNew();
            // Ask for everything above the slider's minimum; the slider then
            // filters instantly without running the network again.
            DetectedObject[] found = _model.Detect(bgr, Config.ConfidenceMin, 0.5);
            infer.Stop();
            // "post": sort for display.
            found = found.OrderByDescending(o => o.Confident).ToArray();
            total.Stop();
            Preferences.Default.Set(LastInferenceKey, infer.Elapsed.TotalMilliseconds);
            return new StageResult
            {
                Boxes = found.Select(o => new StageBox(new RectF(o.Region.X, o.Region.Y, o.Region.Width, o.Region.Height), $"{o.Label} {o.Confident:0.00}", ClassColor(o.ClassId), o.Confident)).ToArray(),
                Rows = found.Select(o => ScoreRow(o.Label, o.Confident, ClassColor(o.ClassId))).ToArray(),
                InferenceMs = infer.Elapsed.TotalMilliseconds,
                PrePostMs = total.Elapsed.TotalMilliseconds - infer.Elapsed.TotalMilliseconds,
            };
        }

        protected override Task OpenLiveCameraAsync() => OpenLiveCamera(Navigation, Selection);

        /// <summary>
        /// Open YOLO's full-screen live camera. Also used straight from the home
        /// screen (Camera tab, "Live camera"), so closing it returns there; opening
        /// it while a page is still sliding in can leave that page frozen on iOS.
        /// </summary>
        internal static async Task OpenLiveCamera(INavigation navigation, string selection = null)
        {
#if __IOS__ || __MACCATALYST__
            await navigation.PushModalAsync(new ModelShowcaseLivePage("YOLO", () => new Yolo(), selection));
#else
            await navigation.PushAsync(new Emgu.CV.Platform.Maui.UI.ProcessAndRenderPage(
                new Yolo(), "YOLO (Live)", null, "Real-time detection from the camera."));
#endif
        }

        protected override string CodeSnippet => $@"using Emgu.CV;
using Emgu.CV.Models;

using var yolo = new Yolo();
await yolo.Init(Yolo.YoloVersion.{Selection});

using Mat image = CvInvoke.Imread(""dog.png"");
DetectedObject[] objects =
    yolo.Detect(image, confThreshold: {Confidence:0.00});

foreach (DetectedObject o in objects)
    Console.WriteLine(
        $""{{o.Label}} {{o.Confident:0.00}} at {{o.Region}}"");";

        protected override void OnClosed()
        {
            _model?.Dispose();
            _model = null;
        }
    }
}
