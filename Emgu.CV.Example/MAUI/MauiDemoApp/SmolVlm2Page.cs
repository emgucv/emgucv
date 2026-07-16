//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Models;
using Emgu.CV.Platform.Maui.UI;
using Emgu.Util;

namespace MauiDemoApp
{
    /// <summary>
    /// Chat with an image using the SmolVLM2 vision-language model running on
    /// the dnn module. Each question is answered about the currently selected
    /// image with KV-cached generation.
    /// </summary>
    public class SmolVlm2Page : ButtonTextImagePage
    {
        private SmolVlm2 _model = new SmolVlm2();
        private Mat _image = null;
        private Editor _promptEditor;
        private List<View> _bubbles = new List<View>();

        public SmolVlm2Page()
            : base(new Microsoft.Maui.Controls.Button[] { new Microsoft.Maui.Controls.Button() })
        {
            this.Title = "Chat with Image";

            _promptEditor = new Editor();
            _promptEditor.Placeholder = "Ask a question about the image...";
            _promptEditor.Text = "Describe this image briefly.";
            _promptEditor.HeightRequest = 80;
            _promptEditor.BackgroundColor = Microsoft.Maui.Graphics.Colors.White;
            _promptEditor.TextColor = Microsoft.Maui.Graphics.Color.FromArgb("#1A1C2E");
            _promptEditor.FontFamily = "InterRegular";
            MainLayout.Children.Insert(0, _promptEditor);

            var sendButton = this.GetButton();
            sendButton.Text = "Send";

            var pickImageButton = this.AdditionalButtons[0];
            pickImageButton.Text = "Pick Image";

            //Show the default sample image when the page opens.
            this.Loaded += async (sender, args) =>
            {
                if (_image == null)
                {
                    try
                    {
                        await LoadDefaultImage();
                    }
                    catch (Exception ex)
                    {
                        SetMessage(String.Format("Failed to load the sample image: {0}", ex.Message));
                    }
                }
            };

            sendButton.Clicked += async (sender, args) =>
            {
                String prompt = _promptEditor.Text;
                if (String.IsNullOrWhiteSpace(prompt) || _image == null)
                    return;

                sendButton.IsEnabled = false;
                pickImageButton.IsEnabled = false;
                try
                {
                    if (!_model.Initialized)
                    {
                        SetMessage("Initializing the model, this downloads ~1.1 GB on the first run...");
                        await _model.Init(DownloadManager_OnDownloadProgressChanged);
                        if (!_model.Initialized)
                        {
                            SetMessage("Failed to initialize the SmolVLM2 model.");
                            return;
                        }
                    }

                    _promptEditor.Text = String.Empty;
                    AddBubble(prompt, true);
                    SetMessage("Generating...");

                    String response = await Task.Run(() => _model.Generate(_image, prompt, 96));
                    AddBubble(response.Trim(), false);
                    SetMessage(null);
                }
                catch (Exception ex)
                {
                    SetMessage(String.Format("Error: {0}", ex.Message));
                }
                finally
                {
                    sendButton.IsEnabled = true;
                    pickImageButton.IsEnabled = true;
                }
            };

            pickImageButton.Clicked += async (sender, args) =>
            {
                try
                {
                    FileResult fileResult = await FilePicker.PickAsync(PickOptions.Images);
                    if (fileResult == null)
                        return;
                    Mat picked;
                    using (Stream stream = await fileResult.OpenReadAsync())
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await stream.CopyToAsync(ms);
                        picked = new Mat();
                        CvInvoke.Imdecode(ms.ToArray(), ImreadModes.ColorBgr, picked);
                    }
                    if (picked.IsEmpty)
                    {
                        picked.Dispose();
                        SetMessage("Failed to decode the selected image.");
                        return;
                    }

                    if (_image != null)
                        _image.Dispose();
                    _image = picked;
                    SetImage(_image);

                    //Each question is answered about the current image only, so
                    //start a fresh conversation for the new image.
                    ClearBubbles();
                    SetMessage("Image loaded. Ask a question about it.");
                }
                catch (Exception ex)
                {
                    SetMessage(String.Format("Failed to pick an image: {0}", ex.Message));
                }
            };
        }

        private async Task LoadDefaultImage()
        {
            using (Stream stream = await FileSystem.OpenAppPackageFileAsync("dog416.png"))
            using (MemoryStream ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                Mat m = new Mat();
                CvInvoke.Imdecode(ms.ToArray(), ImreadModes.ColorBgr, m);
                _image = m;
                SetImage(_image);
            }
        }

        private void ClearBubbles()
        {
            foreach (View bubble in _bubbles)
                MainLayout.Children.Remove(bubble);
            _bubbles.Clear();
        }

        /// <summary>
        /// Append a chat bubble to the transcript, right-aligned accent for the
        /// user and left-aligned white for the model.
        /// </summary>
        private void AddBubble(String text, bool isUser)
        {
            var label = new Label
            {
                Text = text,
                TextColor = isUser
                    ? Microsoft.Maui.Graphics.Colors.White
                    : Microsoft.Maui.Graphics.Color.FromArgb("#1A1C2E"),
                FontFamily = "InterRegular",
                FontSize = 15,
                LineBreakMode = LineBreakMode.WordWrap
            };

            var timestampLabel = new Label
            {
                Text = DateTime.Now.ToString("t"),
                TextColor = isUser
                    ? Microsoft.Maui.Graphics.Color.FromArgb("#D7E1FF")
                    : Microsoft.Maui.Graphics.Color.FromArgb("#8A8FA3"),
                FontFamily = "InterRegular",
                FontSize = 11,
                HorizontalOptions = isUser ? LayoutOptions.End : LayoutOptions.Start
            };

            var bubbleContent = new VerticalStackLayout
            {
                Spacing = 2,
                Children = { label, timestampLabel }
            };

            var bubble = new Border
            {
                BackgroundColor = isUser
                    ? Microsoft.Maui.Graphics.Color.FromArgb("#3D7BF7")
                    : Microsoft.Maui.Graphics.Colors.White,
                Stroke = Microsoft.Maui.Graphics.Colors.Transparent,
                StrokeThickness = 0,
                Padding = new Thickness(12, 8),
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(12) },
                HorizontalOptions = isUser ? LayoutOptions.End : LayoutOptions.Start,
                MaximumWidthRequest = 460,
                Content = bubbleContent
            };

            //Insert the bubble above the (status) message label so the
            //transcript grows between the buttons and the status area.
            int index = MainLayout.Children.IndexOf(MessageLabel);
            MainLayout.Children.Insert(index < 0 ? MainLayout.Children.Count : index, bubble);
            _bubbles.Add(bubble);

            if (this.Content is ScrollView scrollView)
            {
                try
                {
                    scrollView.ScrollToAsync(MessageLabel, ScrollToPosition.End, false);
                }
                catch (Exception)
                {
                }
            }
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
