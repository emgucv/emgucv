//----------------------------------------------------------------------------
//  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util.TypeEnum;

using System.Runtime.InteropServices;

using Microsoft.Maui.Controls.PlatformConfiguration;

#if __MACCATALYST__
using AppKit;
using CoreGraphics;
#elif __IOS__
using UIKit;
using CoreGraphics;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
#elif __ANDROID__
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Android.Hardware.Lights;
#elif WINDOWS
using Visibility = Microsoft.UI.Xaml.Visibility;
using Microsoft.UI.Xaml.Media.Imaging;
#endif

namespace Emgu.CV.Platform.Maui.UI
{
    /// <summary>
    /// A simple page with a button, message text and an image
    /// </summary>
    public class ButtonTextImagePage
#if __IOS__
        : Emgu.Util.AvCaptureSessionPage
#elif __ANDROID__
        : AndroidCameraPage
#else
        : ContentPage
#endif
    {
        private Picker _picker = new Picker();

        /// <summary>
        /// Get the file picker
        /// </summary>
        public Picker Picker
        {
            get { return _picker; }
        }

        private Microsoft.Maui.Controls.Button _topButton = new Microsoft.Maui.Controls.Button();

        /// <summary>
        /// Get the button at the top
        /// </summary>
        public Microsoft.Maui.Controls.Button TopButton
        {
            get { return _topButton; }
        }

        private Label _messageLabel = new Label();

        /// <summary>
        /// Get the message label
        /// </summary>
        public Label MessageLabel
        {
            get { return _messageLabel; }
        }
		
		private Editor _logEditor = new Editor();
        /// <summary>
        /// The log message editor
        /// </summary>
        public Editor LogEditor
        {
            get { return _logEditor; }
        }

        private CvImageView _displayImage = new CvImageView();

        /// <summary>
        /// Get the displayed image
        /// </summary>
        public CvImageView DisplayImage
        {
            get { return _displayImage; }
            //set { _displayImage = value; }
        }

#if __ANDROID__
        private Bitmap[] _renderBuffer = new Bitmap[2];
        private int _renderBufferIdx = 0;

        /// <summary>
        /// Set the image to be displayed
        /// </summary>
        /// <param name="image">The image to be displayed</param>
        public virtual void SetImage(IInputArray image)
        {
            if (image == null)
            {
                this.Dispatcher.Dispatch(
                    () =>
                    {
                        DisplayImage.ImageView.SetImageBitmap(null);
                    });
                return;
            }

            int bufferIdx = _renderBufferIdx;
            Bitmap buffer;
            _renderBufferIdx = (_renderBufferIdx + 1) % _renderBuffer.Length;

            using (InputArray iaImage = image.GetInputArray())
            using (Mat mat = iaImage.GetMat())
            {
                if (_renderBuffer[bufferIdx] == null)
                {
                    buffer = mat.ToBitmap();
                    _renderBuffer[bufferIdx] = buffer;
                }
                else
                {
                    var size = iaImage.GetSize();
                    buffer = _renderBuffer[bufferIdx];
                    if (buffer.Width != size.Width || buffer.Height != size.Height)
                    {
                        buffer.Dispose();
                        _renderBuffer[bufferIdx] = mat.ToBitmap();
                    }
                    else
                    {
                        mat.ToBitmap(buffer);
                    }
                }
            }

            this.Dispatcher.Dispatch(
                () =>
                {
                    DisplayImage.ImageView.SetImageBitmap(buffer);
                });
        }
#else

        /// <summary>
        /// Set the image to be displayed
        /// </summary>
        /// <param name="image">The image to be displayed</param>
        public virtual void SetImage(IInputArray image)
        {
            this.DisplayImage.SetImage(image);
            if (image == null)
                this.DisplayImage.IsVisible = false;
            else
            {
                this.DisplayImage.IsVisible = true;

                using (InputArray iaImage = image.GetInputArray())
                {
                    System.Drawing.Size size = iaImage.GetSize();
                    this.DisplayImage.WidthRequest = Math.Min(this.Width, size.Width);
                    this.DisplayImage.HeightRequest = size.Height;
                }
            }
        }
#endif

        private StackLayout _mainLayout = new StackLayout();

        /// <summary>
        /// Get the main layout
        /// </summary>
        public StackLayout MainLayout
        {
            get { return _mainLayout; }
        }

        private Microsoft.Maui.Controls.Button[] _additionalButtons;

        /// <summary>
        /// Get the list of additional buttons
        /// </summary>
        public Microsoft.Maui.Controls.Button[] AdditionalButtons
        {
            get
            {
                return _additionalButtons;
            }
        }

        /// <summary>
        /// Create a simple page with a button, message label and image.
        /// </summary>
        /// <param name="additionalButtons">Additional buttons added below the standard button.</param>
        public ButtonTextImagePage(Microsoft.Maui.Controls.Button[] additionalButtons=null)
        {
            var horizontalLayoutOptions = LayoutOptions.Center;

            TopButton.Text = "Click me";
            TopButton.IsEnabled = true;
            TopButton.HorizontalOptions = horizontalLayoutOptions;

            MessageLabel.Text = "";
            MessageLabel.HorizontalOptions = horizontalLayoutOptions;


            //DisplayImage.HeightRequest = 100;
            //DisplayImage.WidthRequest = 100;
            //DisplayImage.MinimumHeightRequest = 10;

            //StackLayout mainLayout = new StackLayout();
            _mainLayout.Children.Add(Picker);
            Picker.IsVisible = false;

            _mainLayout.Children.Add(TopButton);
            if (additionalButtons != null)
            {
                foreach (Microsoft.Maui.Controls.Button button in additionalButtons)
                {
                    button.HorizontalOptions = horizontalLayoutOptions;
                    _mainLayout.Children.Add(button);
                }
            }

            _additionalButtons = additionalButtons;

            _mainLayout.Children.Add(MessageLabel);


            //MessageLabel.BackgroundColor = Color.AliceBlue;
            //DisplayImage.BackgroundColor = Color.Aqua;
            //_mainLayout.BackgroundColor = Color.Blue;

            _mainLayout.Children.Add(DisplayImage);
            DisplayImage.BackgroundColor = 
                Microsoft.Maui.Graphics.Color.FromRgb(1.0, 0.0, 0.0);

            //_mainLayout.Children.Add(MessageLabel);
			
			LogEditor.Text = "";
            LogEditor.HorizontalOptions = LayoutOptions.Center;
            LogEditor.VerticalOptions = LayoutOptions.Center;
            LogEditor.FontSize = LogEditor.FontSize / 2;
			_mainLayout.Children.Add(LogEditor);
			
			SetLog(null);
			
            _mainLayout.Padding = new Thickness(10, 10, 10, 10);

            this.Content = new Microsoft.Maui.Controls.ScrollView()
            {
                Content = _mainLayout
            };
        }

        /// <summary>
        /// A flag that indicates if the file picker allow getting image from camera
        /// </summary>
        public bool HasCameraOption { get; set; }

        /// <summary>
        /// Load the images and return them asynchronously
        /// </summary>
        /// <param name="imageNames">The name of the images</param>
        /// <param name="labels">The labels of the images</param>
        /// <returns>The images loaded</returns>
        public virtual async Task<Mat[]> LoadImages(String[] imageNames, String[] labels = null)
        {
            Mat[] mats = new Mat[imageNames.Length];

            for (int i = 0; i < mats.Length; i++)
            {
                String pickImgString = "Use Image from";
                if (labels != null && labels.Length > i)
                    pickImgString = labels[i];

                bool captureSupported;

                if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.WinUI
                    || Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.macOS)
                {
                    //Pick image from camera is not implemented on WPF.
                    captureSupported = false;
                }
                else
                {
                    captureSupported = MediaPicker.IsCaptureSupported;
                }

                String action;
                List<String> options = new List<string>();
                options.Add("Default");

                options.Add("Photo Library");

                if (captureSupported)
                    options.Add("Photo from Camera");

                if (this.HasCameraOption)
                {
                    if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.Android
                        || Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.iOS
                        || Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        if (captureSupported)
                            options.Add("Camera");
                    }
                    else if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.WinUI
                             || Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.macOS)
                    {
                        var openCVConfigDict = CvInvoke.ConfigDict;
                        bool haveVideoio = (openCVConfigDict["HAVE_OPENCV_VIDEOIO"] != 0);
                        if (haveVideoio)
                            options.Add("Camera");
                    }
                }


                if (options.Count == 1)
                {
                    action = "Default";
                }
                else
                {
                    action = await DisplayActionSheet(pickImgString, "Cancel", null, options.ToArray());
                    if (
                        action == null //user clicked outside of action sheet
                        || action.Equals("Cancel") // user cancel
                    )
                        return null;
                }

                if (action.Equals("Default"))
                {

                    using (Stream stream = await FileSystem.OpenAppPackageFileAsync(imageNames[i]))
                        using(MemoryStream ms = new MemoryStream())
                        {
                        stream.CopyTo(ms);
                        Mat m = new Mat();
                        CvInvoke.Imdecode(ms.ToArray(), ImreadModes.Color, m);
                        mats[i] = m;
                        }

                }
                else if (action.Equals("Photo Library"))
                {
                    /*
                    if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.WinUI)
                    {

#if !(__MACCATALYST__ || __ANDROID__ || __IOS__ || NETFX_CORE)
                        Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                        dialog.Multiselect = false;
                        dialog.Title = "Select an Image File";
                        dialog.Filter = "Image | *.jpg;*.jpeg;*.png;*.bmp;*.gif | All Files | *";
                        if (dialog.ShowDialog() == false)
                            return null;
                        mats[i] = CvInvoke.Imread(dialog.FileName, ImreadModes.AnyColor);
#endif
                    }
                    else
                    */
                    {
                        var fileResult = await FilePicker.PickAsync(PickOptions.Images);
                        if (fileResult == null) //canceled
                            return null;
                        using (Stream s = await fileResult.OpenReadAsync())
                            mats[i] = await ReadStream(s);
                    }
                }
                else if (action.Equals("Photo from Camera"))
                {
                    PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                    if (status == PermissionStatus.Denied)
                    {
                        this.SetMessage("Please grant permission to use camera");
                    }
                    else
                    {
                        var takePhotoResult = await MediaPicker.CapturePhotoAsync();

                        if (takePhotoResult == null) //canceled
                            return null;
                        using (Stream stream = await takePhotoResult.OpenReadAsync())
                            mats[i] = await ReadStream(stream);
                    }
                }
                else if (action.Equals("Camera"))
                {
                    mats = new Mat[0];
                }
            }

            return mats;
        }

        private static async Task<Mat> ReadStream(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                byte[] data = ms.ToArray();
                Mat m = new Mat();
                CvInvoke.Imdecode(data, ImreadModes.Color, m);
                return m;
            }
        }

        /// <summary>
        /// Get the message label UI
        /// </summary>
        /// <returns>The message label ui</returns>
        public Label GetLabel()
        {
            //return null;
            return this.MessageLabel;
        }

        /// <summary>
        /// Set the message to be displayed
        /// </summary>
        /// <param name="message">The message to be displayed</param>
        public void SetMessage(String message)
        {
            this.Dispatcher.Dispatch(
                () =>
                {
                    Label label = GetLabel();
                    label.Text = message;

                    label.LineBreakMode = LineBreakMode.WordWrap;
                 //label.WidthRequest = this.Width;
             }
            );
        }

        private String _log = String.Empty;

        /// <summary>
        /// Clear the log
        /// </summary>
        public void ClearLog()
        {
            SetLog(String.Empty);
        }

        /// <summary>
        /// Set the log
        /// </summary>
        /// <param name="log">The log</param>
        public void SetLog(String log)
        {
            _log = log;
            RenderLog(_log);
        }

        /// <summary>
        /// Append text to the log
        /// </summary>
        /// <param name="log">The text to be append to the log</param>
        public void AppendLog(String log)
        {
            if (!String.IsNullOrEmpty(_log))
                _log = log + _log;
            RenderLog(_log);
        }

        /// <summary>
        /// Render the log
        /// </summary>
        /// <param name="log">The log to be rendered</param>
        private void RenderLog(String log)
        {
            this.Dispatcher.Dispatch(
                () =>
                {
                    if (String.IsNullOrEmpty(log))
                    {
                        this.LogEditor.IsVisible = false;
                    }
                    else
                    {
                        this.LogEditor.IsVisible = true;
                    }

                    this.LogEditor.Text = log;
                    this.LogEditor.WidthRequest = this.Width;
                    this.LogEditor.HeightRequest = 120;

                    //this.LogLabel.LineBreakMode = LineBreakMode.WordWrap;
                    this.LogEditor.Focus();
                }
            );
        }

        /// <summary>
        /// Get the main button
        /// </summary>
        /// <returns>The main button</returns>
        public Microsoft.Maui.Controls.Button GetButton()
        {
            //return null;
            return this.TopButton;
        }

    }


}
