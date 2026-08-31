//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Models;

using Microsoft.Maui.Controls.Shapes;

namespace MauiDemoApp
{
    /// <summary>
    /// Chat with an image using the SmolVLM2 vision-language model running on the
    /// dnn module. Each question is answered about the currently selected image
    /// with KV-cached generation.
    ///
    /// The layout comes from <see cref="ChatShowcasePage"/>; this page adds the
    /// image card the conversation is about.
    /// </summary>
    public class SmolVlm2Page : ChatShowcasePage
    {
        // Glyph the main menu already uses for both language-model modules.
        private const string GlyphText = ""; // text_fields

        private readonly SmolVlm2 _model = new SmolVlm2();
        private Image _preview;
        private Mat _image;
        private bool _defaultLoaded;

        public SmolVlm2Page()
            : base(
                title: "Chat with Image",
                heroTitle: "Chat with Image",
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
                actionGlyph: MaskRcnnPage.GlyphImage,
                actionText: "Change",
                composerPlaceholder: "Ask a question about the image…")
        {
        }

        protected override string EmptyStateHint => "Ask something about the photo above.";

        protected override View BuildExtraCard()
        {
            _preview = new Image { Aspect = Aspect.AspectFit, HeightRequest = 240 };

            var frame = new Border
            {
                BackgroundColor = MaskRcnnPage.ImageBackground,
                Stroke = MaskRcnnPage.RowBorder,
                StrokeThickness = 1,
                Padding = new Thickness(10),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
                Content = _preview
            };

            var header = new Grid { ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) } };
            header.Add(new Label
            {
                Text = "Image",
                FontFamily = MaskRcnnPage.TitleFont,
                FontSize = 17,
                TextColor = MaskRcnnPage.PrimaryText,
                VerticalOptions = LayoutOptions.Center
            }, 0, 0);

            return new Border
            {
                BackgroundColor = MaskRcnnPage.CardBackground,
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(22) },
                Padding = new Thickness(16),
                Content = new VerticalStackLayout { Spacing = 12, Children = { header, frame } }
            };
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
                SetImage(m);
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

            SetStatus("Preparing the model… the first run downloads about 1.1 GB.");
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

                SetImage(picked);

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

        private void SetImage(Mat image)
        {
            _image?.Dispose();
            _image = image;
            if (_preview != null)
                _preview.Source = MaskRcnnPage.MatToImageSource(image);
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
