//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Emgu.CV.Models;

namespace MauiDemoApp
{
    /// <summary>
    /// Chat demo using a Qwen language model running on the dnn module. The model
    /// can be swapped in the model card; the conversation history is kept per
    /// session and each turn re-prefills the transcript with KV-cached generation
    /// for the response.
    ///
    /// The layout comes from <see cref="ChatShowcasePage"/>, so this page only
    /// declares its models and wires up initialization and generation.
    /// </summary>
    public class QwenChatPage : ChatShowcasePage
    {
        // Selected via ChatModel.Tag so the picker never has to be matched on its
        // display string.
        private enum Variant
        {
            Qwen3Small,
            Qwen3Large,
            Qwen25
        }

        // Glyph the main menu already uses for both language-model modules.
        private const string GlyphText = ""; // text_fields

        private IDisposable _model;
        private Variant? _loaded;

        public QwenChatPage()
            : base(
                title: "Qwen Chat",
                heroTitle: "Chat with Qwen",
                heroSubtitle: "Local language models for fast, private and secure conversations.",
                glyph: GlyphText,
                // Sizes are the exact byte counts of the published files, so a
                // half-finished download can never be reported as installed. A 0
                // means the size has not been recorded yet.
                models: new[]
                {
                    new ChatModel("Qwen3 0.6B", "2.8 GB • Fastest (~150 ms/token)", "qwen3_0.6b_onnx",
                        new[] { ("tokenizer.json", 11422654L), ("model.onnx", 1430582L), ("model.onnx_data", 3006529792L) },
                        true, Variant.Qwen3Small, weightBytes: 3019383028L),
                    new ChatModel("Qwen3 1.7B", "7.6 GB • Better quality, slower", "qwen3_1.7b_onnx",
                        new[] { ("model.onnx", 0L), ("model.onnx_data", 0L) },
                        false, Variant.Qwen3Large, weightBytes: 7600000000L),
                    // 2.4 GB, not the "~1 GB" a 0.5B model suggests: these weights
                    // are exported at full 32-bit precision, so the download is
                    // roughly four bytes per parameter.
                    new ChatModel("Qwen2.5 0.5B", "2.4 GB • Previous generation / reference port", "qwen2.5_0.5b_instruct_onnx",
                        new[] { ("tokenizer.json", 7031645L), ("model.onnx", 1125445L), ("model.onnx_data", 2520669824L) },
                        false, Variant.Qwen25, weightBytes: 2528826914L)
                },
                actionGlyph: MaskRcnnPage.GlyphPlay,
                actionText: "New Chat",
                composerPlaceholder: "Type your message…")
        {
        }

        protected override string EmptyStateHint => "Ask anything — the model runs entirely on your device.";

        protected override async Task<bool> EnsureModelReadyAsync()
        {
            var variant = (Variant)SelectedModel.Tag;

            // Switching models starts a fresh conversation.
            if (_model != null && _loaded != variant)
            {
                _model.Dispose();
                _model = null;
                _loaded = null;
                ClearTranscript();
            }

            if (_model != null)
                return true;

            BeginModelLoad("Preparing the model… the first run downloads it.");

            if (variant == Variant.Qwen25)
            {
                Qwen25 qwen25 = new Qwen25();
                await qwen25.Init(OnDownloadProgress);
                _model = qwen25.Initialized ? qwen25 : null;
            }
            else
            {
                Qwen3 qwen3 = new Qwen3();
                await qwen3.Init(
                    variant == Variant.Qwen3Large ? Qwen3.Qwen3Version.Qwen3_1_7B : Qwen3.Qwen3Version.Qwen3_0_6B,
                    OnDownloadProgress);
                _model = qwen3.Initialized ? qwen3 : null;
            }

            if (_model == null)
            {
                SetStatus("Could not load the model. Check your connection and try again.");
                return false;
            }

            _loaded = variant;
            UpdateModelSummary();
            return true;
        }

        protected override string Generate(string prompt)
        {
            if (_model is Qwen3 qwen3)
                return qwen3.Chat(prompt, 128);
            if (_model is Qwen25 qwen25)
                return qwen25.Chat(prompt, 128);
            throw new InvalidOperationException("The model is not initialized.");
        }

        protected override void OnModelDeleted(ChatModel model)
        {
            // Only the loaded variant matters; deleting a different one leaves the
            // live model alone.
            if (_model == null || _loaded == null || !_loaded.Equals((Variant)model.Tag))
                return;

            _model.Dispose();
            _model = null;
            _loaded = null;
            ClearTranscript();
        }

        protected override void OnHeaderAction(object sender, EventArgs e)
        {
            if (_model is Qwen3 qwen3)
                qwen3.ResetChat();
            else if (_model is Qwen25 qwen25)
                qwen25.ResetChat();

            ClearTranscript();
            SetStatus(null);
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            base.OnNavigatedFrom(args);
            if (!Navigation.NavigationStack.Contains(this) && !Navigation.ModalStack.Contains(this))
            {
                // Leaving mid-answer would otherwise free the model while the
                // background generation is still inside it — a native crash.
                IDisposable model = _model;
                _model = null;
                _loaded = null;
                CleanupWhenIdle(() => model?.Dispose());
            }
        }
    }
}
