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
    /// Chat demo using a Qwen language model running on the dnn module. The
    /// model can be selected in the picker; the conversation history is kept
    /// per session and each turn re-prefills the transcript with KV-cached
    /// generation for the response.
    /// </summary>
    public class QwenChatPage : ButtonTextImagePage
    {
        private const String OptionQwen3Small = "Qwen3 0.6B - fast, default";
        private const String OptionQwen3Large = "Qwen3 1.7B - better answers, slower, 7.6 GB";
        private const String OptionQwen25 = "Qwen2.5 0.5B";

        private IDisposable _model = null;
        private String _modelOption = null;
        private Editor _promptEditor;
        private List<View> _bubbles = new List<View>();

        public QwenChatPage()
            : base(new Microsoft.Maui.Controls.Button[] { new Microsoft.Maui.Controls.Button() })
        {
            this.Title = "Qwen Chat";

            var picker = this.Picker;
            picker.Title = "Model";
            picker.Items.Add(OptionQwen3Small);
            picker.Items.Add(OptionQwen3Large);
            picker.Items.Add(OptionQwen25);
            picker.SelectedIndex = 0;
            picker.IsVisible = true;

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
                picker.IsEnabled = false;
                try
                {
                    String option = picker.Items[picker.SelectedIndex];

                    //Switching the model starts a fresh conversation.
                    if (_model != null && option != _modelOption)
                    {
                        _model.Dispose();
                        _model = null;
                        ClearBubbles();
                    }

                    if (_model == null)
                    {
                        SetMessage("Initializing the model, the first run downloads the model files...");
                        if (option == OptionQwen25)
                        {
                            Qwen25 qwen25 = new Qwen25();
                            await qwen25.Init(DownloadManager_OnDownloadProgressChanged);
                            _model = qwen25.Initialized ? qwen25 : null;
                        }
                        else
                        {
                            Qwen3 qwen3 = new Qwen3();
                            await qwen3.Init(
                                option == OptionQwen3Large ? Qwen3.Qwen3Version.Qwen3_1_7B : Qwen3.Qwen3Version.Qwen3_0_6B,
                                DownloadManager_OnDownloadProgressChanged);
                            _model = qwen3.Initialized ? qwen3 : null;
                        }
                        if (_model == null)
                        {
                            SetMessage("Failed to initialize the model.");
                            return;
                        }
                        _modelOption = option;
                    }

                    _promptEditor.Text = String.Empty;
                    AddBubble(prompt, true);
                    SetMessage("Generating...");

                    String response = await Task.Run(() => Chat(prompt, 128));
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
                    picker.IsEnabled = true;
                }
            };

            newChatButton.Clicked += (sender, args) =>
            {
                ResetChat();
                ClearBubbles();
                SetMessage(null);
            };
        }

        private String Chat(String prompt, int maxNewTokens)
        {
            if (_model is Qwen3 qwen3)
                return qwen3.Chat(prompt, maxNewTokens);
            if (_model is Qwen25 qwen25)
                return qwen25.Chat(prompt, maxNewTokens);
            throw new InvalidOperationException("The model is not initialized.");
        }

        private void ResetChat()
        {
            if (_model is Qwen3 qwen3)
                qwen3.ResetChat();
            else if (_model is Qwen25 qwen25)
                qwen25.ResetChat();
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
