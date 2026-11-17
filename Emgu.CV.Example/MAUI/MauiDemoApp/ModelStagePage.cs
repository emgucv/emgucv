//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.Models;

namespace MauiDemoApp
{
    /// <summary>
    /// <see cref="StagePage"/> for any <see cref="IProcessAndRenderModel"/>: shows
    /// the model's rendered output on the stage (with a before / after split) and
    /// either the module's own result items or the model's message in the panel.
    /// Takes the same arguments the old ModelShowcasePage did.
    /// </summary>
    public class ModelStagePage : StagePage
    {
        private readonly Func<IProcessAndRenderModel> _modelFactory;
        private readonly Func<IProcessAndRenderModel, Mat, (byte[] png, IReadOnlyList<string> items)> _customProcess;
        private readonly Func<IProcessAndRenderModel, Mat, StageResult> _stageProcess;
        private readonly string[] _options;
        private readonly string _codeSnippet;
        private readonly string _backend;
        private IProcessAndRenderModel _model;
        private string _modelLoaded;
        private int _optionIndex;
        private readonly List<ChipView> _optionChips = new List<ChipView>();

        public ModelStagePage(
            string title,
            string subtitle,
            Func<IProcessAndRenderModel> modelFactory,
            StageSample[] samples,
            string optionsTitle = null,
            string[] options = null,
            bool hasCamera = true,
            Func<IProcessAndRenderModel, Mat, (byte[] png, IReadOnlyList<string> items)> customProcess = null,
            string about = null,
            string tag = null,
            string category = null,
            string module = null,
            string runLabel = "Detect",
            string itemSingular = "item",
            string itemPlural = "items",
            string codeSnippet = null,
            string backend = "CPU · OpenCV",
            Func<IProcessAndRenderModel, Mat, StageResult> stageProcess = null)
            : base(new StageConfig
            {
                Title = title,
                Tag = tag,
                Category = category,
                Module = module,
                Description = subtitle,
                About = about,
                Samples = samples ?? Array.Empty<StageSample>(),
                HasCamera = hasCamera,
                RunLabel = runLabel,
                OptionsTitle = optionsTitle ?? "Model",
                ItemSingular = itemSingular,
                ItemPlural = itemPlural,
                EmptyCaption = "run it to see results here",
            })
        {
            _modelFactory = modelFactory;
            _customProcess = customProcess;
            _stageProcess = stageProcess;
            _options = options;
            _codeSnippet = codeSnippet;
            _backend = backend;
            _model = modelFactory();
        }

        protected override string BackendName => _backend;

        protected override string CodeSnippet => _codeSnippet?.Replace("{option}", CurrentOption ?? "");

        private string CurrentOption => _options != null && _options.Length > 0 ? _options[_optionIndex] : null;

        protected override View BuildOptions()
        {
            if (_options == null || _options.Length == 0)
                return null;
            var flex = new FlexLayout { Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap };
            for (int i = 0; i < _options.Length; i++)
            {
                int idx = i;
                var chip = new ChipView(_options[i], i == 0, 11) { Margin = new Thickness(0, 0, 8, 8) };
                chip.Tapped += (s, e) =>
                {
                    if (idx == _optionIndex)
                        return;
                    _optionIndex = idx;
                    for (int j = 0; j < _optionChips.Count; j++)
                        _optionChips[j].IsSelected = j == idx;
                    // A different option means the model must be re-initialized.
                    _model?.Clear();
                    _modelLoaded = null;
                    ClearResult();
                    UpdateStatus();
                };
                _optionChips.Add(chip);
                flex.Children.Add(chip);
            }
            return flex;
        }

        protected override void OnStatusInit() => UpdateStatus();

        private void UpdateStatus()
        {
            SetOptionsValue(CurrentOption ?? "");
            if (_model != null && _model.Initialized)
                SetStatus("Loaded");
            else
                SetStatus("Loads on first run", DemoTheme.MutedText);
        }

        protected override async Task<bool> PrepareAsync()
        {
            if (_model == null)
                return false;
            string sel = CurrentOption;
            if (_model.Initialized && _modelLoaded == (sel ?? ""))
                return true;
            SetStatus("Loading model…", DemoTheme.Lime);
            await Task.Run(() => _model.Init(OnDownloadProgress, sel));
            if (!_model.Initialized)
            {
                SetStatus("Could not load · check your connection", DemoTheme.MutedText);
                return false;
            }
            _modelLoaded = sel ?? "";
            UpdateStatus();
            return true;
        }

        protected override StageResult RunCore(Mat input)
        {
            if (_stageProcess != null)
            {
                // The module returns structured boxes / rows; boxes are drawn live.
                var sw = Stopwatch.StartNew();
                StageResult r = _stageProcess(_model, input);
                sw.Stop();
                if (r.InferenceMs <= 0)
                    r.InferenceMs = sw.Elapsed.TotalMilliseconds;
                return r;
            }
            var result = new StageResult();
            var infer = Stopwatch.StartNew();
            byte[] png;
            if (_customProcess != null)
            {
                IReadOnlyList<string> items;
                (png, items) = _customProcess(_model, input);
                infer.Stop();
                result.Rows = (items ?? Array.Empty<string>()).Select(t => new StageRow(t)).ToArray();
            }
            else
            {
                using var render = new Mat();
                input.CopyTo(render);
                string message = _model.ProcessAndRender(input, render);
                infer.Stop();
                var post = Stopwatch.StartNew();
                png = MaskRcnnPage.Encode(render);
                post.Stop();
                result.PrePostMs = post.Elapsed.TotalMilliseconds;
                result.Note = CleanMessage(message);
            }
            result.InferenceMs = infer.Elapsed.TotalMilliseconds;
            result.OutputPng = png;
            if (png != null)
            {
                using var decoded = new Mat();
                CvInvoke.Imdecode(png, Emgu.CV.CvEnum.ImreadModes.Unchanged, decoded);
                result.OutputWidth = decoded.Width;
                result.OutputHeight = decoded.Height;
            }
            if (result.Rows.Length == 0)
            {
                result.Summary = $"done · {result.InferenceMs:0} ms";
                // Most models only report timing; their results are on the image.
                if (string.IsNullOrEmpty(result.Note))
                    result.Note = "Results are drawn on the image.";
            }
            return result;
        }

        // Remove any timing phrase from a model's message (the panel shows its own
        // timing). Returns null when nothing meaningful is left.
        private static string CleanMessage(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
                return null;
            string s = System.Text.RegularExpressions.Regex.Replace(
                msg, @"\(?\s*in\s+\d+\s*(ms|milliseconds)\s*\)?",
                "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Tesseract prefixes its text with a version / settings header.
            int td = s.IndexOf("Text Detected:", StringComparison.OrdinalIgnoreCase);
            if (td >= 0)
                s = s.Substring(td + "Text Detected:".Length);
            s = s.Trim().Trim('.', ' ', '(', ')').Trim();
            if (s.Length == 0 || s.Equals("Detected", StringComparison.OrdinalIgnoreCase))
                return null;
            return s;
        }

        protected override async Task OpenLiveCameraAsync()
        {
#if __IOS__ || __MACCATALYST__
            await Navigation.PushModalAsync(new ModelShowcaseLivePage(Config.Title, _modelFactory, CurrentOption));
#else
            await Navigation.PushAsync(new Emgu.CV.Platform.Maui.UI.ProcessAndRenderPage(
                _modelFactory(), Config.Title + " (Live)", null, "Real-time detection from the camera."));
#endif
        }

        protected override void OnClosed()
        {
            _model?.Dispose();
            _model = null;
        }
    }
}
