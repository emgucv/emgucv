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
    /// Chat demo using the Qwen2.5 0.5B language model running on the dnn
    /// module. The conversation history is kept in the KV-cache, so each turn
    /// only processes the newly typed message and the model remembers the
    /// previous turns of the session.
    /// </summary>
    public class Qwen25Page : ButtonTextImagePage
    {
        private Qwen25 _model = new Qwen25();
        private Editor _promptEditor;
        private List<View> _bubbles = new List<View>();

        public Qwen25Page()
            : base(new Microsoft.Maui.Controls.Button[] { new Microsoft.Maui.Controls.Button() })
        {
            this.Title = "Qwen2.5 Chat";

            _promptEditor = new Editor();
            _promptEditor.Placeholder = "Type a message...";
            _promptEditor.Text = "What is OpenCV?";
            _promptEditor.HeightRequest = 80;
            _promptEditor.BackgroundColor = Microsoft.Maui.Graphics.Colors.White;
            _promptEditor.TextColor = Microsoft.Maui.Graphics.Color.FromArgb("#1A1C2E");
            _promptEditor.FontFamily = "InterRegular";
            MainLayout.Children.Insert(0, _promptEditor);

            //The image view is not used by this page.
            if (DisplayImage.Parent is View imageCard)
                imageCard.IsVisible = false;

            var sendButton = this.GetButton();
            sendButton.Text = "Send";

            var newChatButton = this.AdditionalButtons[0];
            newChatButton.Text = "New Chat";

            sendButton.Clicked += async (sender, args) =>
            {
                String prompt = _promptEditor.Text;
                if (String.IsNullOrWhiteSpace(prompt))
                    return;

                sendButton.IsEnabled = false;
                newChatButton.IsEnabled = false;
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

                    _promptEditor.Text = String.Empty;
                    AddBubble(prompt, true);
                    SetMessage("Generating...");

                    String response = await Task.Run(() => _model.Chat(prompt, 128));
                    AddBubble(response, false);
                    SetMessage(null);
                }
                catch (Exception ex)
                {
                    SetMessage(String.Format("Error: {0}", ex.Message));
                }
                finally
                {
                    sendButton.IsEnabled = true;
                    newChatButton.IsEnabled = true;
                }
            };

            newChatButton.Clicked += (sender, args) =>
            {
                _model.ResetChat();
                foreach (View bubble in _bubbles)
                    MainLayout.Children.Remove(bubble);
                _bubbles.Clear();
                SetMessage(null);
            };
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
                Content = label
            };

            //Insert the bubble above the (status) message label so the
            //transcript grows between the buttons and the status area.
            int index = MainLayout.Children.IndexOf(MessageLabel);
            MainLayout.Children.Insert(index < 0 ? MainLayout.Children.Count : index, bubble);
            _bubbles.Add(bubble);

            if (this.Content is ScrollView scrollView)
            {
                //Best effort scroll to the latest message.
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
