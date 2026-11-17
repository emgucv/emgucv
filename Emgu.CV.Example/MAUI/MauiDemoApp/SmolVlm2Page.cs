//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Models;

namespace MauiDemoApp
{
    /// <summary>
    /// Chat with an image using the SmolVLM2 vision-language model running on the
    /// dnn module. Each question is answered about the currently selected image
    /// with KV-cached generation.
    ///
    /// The layout comes from <see cref="ChatShowcasePage"/>; this page adds the
    /// image section the conversation is about.
    /// </summary>
    public class SmolVlm2Page : ChatShowcasePage
    {
        // Glyph the main menu already uses for both language-model modules.
        private const string GlyphText = ""; // text_fields

        private readonly SmolVlm2 _model = new SmolVlm2();
        private Image _preview;
        private Label _imageBadgeLabel;
        private Mat _image;
        private bool _defaultLoaded;

        public SmolVlm2Page()
            : base(
                title: "Chat with Image",
                heroSubtitle: "Ask questions about a photo — the vision model answers on your device.",
                glyph: GlyphText,
                // Exact byte counts of the published files, so a half-finished
                // download is never reported as installed.
                models: new[]
                {
                    new ChatModel("SmolVLM2 256M", "1.1 GB • Vision + language", "smolvlm2_256m_video_instruct_onnx",
                        new[]
                        {
                            ("tokenizer.json", 3548256L),
                            ("vision_raw.onnx", 374230161L),
                            ("embed_tokens.onnx", 113541419L),
                            ("decoder_raw.onnx", 1244646L),
                            ("decoder_raw.onnx_data", 651896064L)
                        }, true, weightBytes: 1144460546L)
                },
                // No pill in the model section: this module's action is "Change",
                // which belongs on the image it changes. BuildExtraCard puts it
                // there.
                actionText: null,
                composerPlaceholder: "Ask a question about the image…",
                tag: "VLM",
                category: "Chat")
        {
        }

        protected override string EmptyStateHint => "Ask something about the photo above.";

        protected override bool IsModelLoaded => _model.Initialized;

        protected override string AboutText =>
            "SmolVLM2 is a small vision-language model: it looks at the photo and answers questions about it, running locally on the OpenCV dnn module.\n\n" +
            "Each question is answered about the current photo only. Changing the photo starts a new conversation.";

        // Mirrors what EnsureModelReadyAsync, the image loading and Generate call.
        protected override string CodeSnippet =>
            "using Emgu.CV;\n" +
            "using Emgu.CV.CvEnum;\n" +
            "using Emgu.CV.Models;\n\n" +
            "var vlm = new SmolVlm2();\n" +
            "await vlm.Init(onDownloadProgress);   // downloads once\n\n" +
            "using Mat image = new Mat();\n" +
            "CvInvoke.Imdecode(photoBytes, ImreadModes.ColorBgr, image);\n\n" +
            "string answer = vlm.Generate(image, \"What is in this picture?\", 96);";

        protected override View BuildExtraCard()
        {
            // The photo being discussed, in the main area above the conversation:
            // a grey well like the image demos' stage, with a name badge and a
            // floating CHANGE pill. The page sets the well's height per layout.
            _preview = new Image { Aspect = Aspect.AspectFit, Margin = new Thickness(14) };

            (Border badge, Label badgeLabel, BoxView dot) = DemoControls.Badge("Sample · dog");
            dot.IsVisible = false;
            badge.Margin = new Thickness(12);
            _imageBadgeLabel = badgeLabel;

            View change = BuildActionPill("Change");
            change.HorizontalOptions = LayoutOptions.End;
            change.VerticalOptions = LayoutOptions.End;
            change.Margin = new Thickness(12);
            change.Shadow = new Shadow { Brush = Colors.Black, Opacity = DemoTheme.IsDark ? 0.6f : 0.08f, Radius = 10, Offset = new Point(0, 2) };

            return DemoControls.Well(new Grid { Children = { _preview, badge, change } });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_defaultLoaded)
                return;
            _defaultLoaded = true;

            try
            {
                using Stream stream = await FileSystem.OpenAppPackageFileAsync("dog416.png");
                using MemoryStream ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                Mat m = new Mat();
                CvInvoke.Imdecode(ms.ToArray(), ImreadModes.ColorBgr, m);
                SetImage(m, "Sample · dog");
            }
            catch (Exception ex)
            {
                SetStatus("Could not load the sample image: " + ex.Message);
            }
        }

        protected override bool CanSend(out string reason)
        {
            if (_image == null)
            {
                reason = "Pick an image first.";
                return false;
            }
            reason = null;
            return true;
        }

        protected override async Task<bool> EnsureModelReadyAsync()
        {
            if (_model.Initialized)
                return true;

            BeginModelLoad("Preparing the model… the first run downloads about 1.1 GB.");
            await _model.Init(OnDownloadProgress);
            if (!_model.Initialized)
            {
                SetStatus("Could not load the model. Check your connection and try again.");
                return false;
            }

            UpdateModelSummary();
            return true;
        }

        protected override string Generate(string prompt)
        {
            return _model.Generate(_image, prompt, 96);
        }

        protected override async void OnHeaderAction(object sender, EventArgs e)
        {
            try
            {
                FileResult fileResult = await FilePicker.PickAsync(PickOptions.Images);
                if (fileResult == null)
                    return;

                Mat picked = new Mat();
                using (Stream stream = await fileResult.OpenReadAsync())
                using (MemoryStream ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms);
                    CvInvoke.Imdecode(ms.ToArray(), ImreadModes.ColorBgr, picked);
                }

                if (picked.IsEmpty)
                {
                    picked.Dispose();
                    SetStatus("That file could not be read as an image.");
                    return;
                }

                SetImage(picked, fileResult.FileName);

                // Each question is answered about the current image only, so a new
                // image starts a fresh conversation.
                ClearTranscript();
                SetStatus("Image loaded. Ask a question about it.");
            }
            catch (Exception ex)
            {
                SetStatus("Could not pick an image: " + ex.Message);
            }
        }

        private void SetImage(Mat image, string name)
        {
            _image?.Dispose();
            _image = image;
            if (_preview != null)
                _preview.Source = MaskRcnnPage.MatToImageSource(image);
            if (_imageBadgeLabel != null)
                _imageBadgeLabel.Text = string.IsNullOrEmpty(name) ? "Your photo" : name;
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            base.OnNavigatedFrom(args);
            if (!Navigation.NavigationStack.Contains(this) && !Navigation.ModalStack.Contains(this))
            {
                // Both the model and the image are still being read by any
                // in-flight generation on its background thread; freeing them
                // underneath it is a native crash, so wait for it to finish.
                Mat image = _image;
                _image = null;
                CleanupWhenIdle(() =>
                {
                    image?.Dispose();
                    _model?.Dispose();
                });
            }
        }
    }
}
