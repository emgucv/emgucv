//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Models;
using Emgu.CV.Platform.Maui.UI;
using Emgu.Util;

namespace MauiDemoApp
{
    /// <summary>
    /// Text generation demo using the Qwen2.5 0.5B language model running on
    /// the dnn module with KV-cached autoregressive decoding.
    /// </summary>
    public class Qwen25Page : ButtonTextImagePage
    {
        private Qwen25 _model = new Qwen25();
        private Editor _promptEditor;

        public Qwen25Page()
            : base()
        {
            this.Title = "Qwen2.5 (LLM)";

            _promptEditor = new Editor();
            _promptEditor.Text = "What is OpenCV?";
            _promptEditor.HeightRequest = 100;
            _promptEditor.BackgroundColor = Microsoft.Maui.Graphics.Colors.White;
            _promptEditor.TextColor = Microsoft.Maui.Graphics.Color.FromArgb("#1A1C2E");
            _promptEditor.FontFamily = "InterRegular";
            MainLayout.Children.Insert(0, _promptEditor);

            var button = this.GetButton();
            button.Text = "Generate";
            button.Clicked += async (sender, args) =>
            {
                button.IsEnabled = false;
                try
                {
                    if (!_model.Initialized)
                    {
                        SetMessage("Initializing the model, this downloads ~2.4 GB on the first run...");
                        await _model.Init(DownloadManager_OnDownloadProgressChanged);
                        if (!_model.Initialized)
                        {
                            SetMessage("Failed to initialize the Qwen2.5 model.");
                            return;
                        }
                    }

                    String prompt = _promptEditor.Text;
                    SetMessage("Generating...");
                    String response = await Task.Run(() => _model.Generate(prompt, 128));
                    SetMessage(response);
                }
                catch (Exception ex)
                {
                    SetMessage(String.Format("Error: {0}", ex.Message));
                }
                finally
                {
                    button.IsEnabled = true;
                }
            };
        }

        private void DownloadManager_OnDownloadProgressChanged(long? totalBytesToReceive, long bytesReceived, double? progressPercentage)
        {
            if (totalBytesToReceive == null)
                SetMessage(String.Format("{0} MB downloaded.", bytesReceived / (1024 * 1024)));
            else
                SetMessage(String.Format("{0} of {1} MB downloaded ({2}%)", bytesReceived / (1024 * 1024), totalBytesToReceive / (1024 * 1024), progressPercentage));
        }
    }
}
