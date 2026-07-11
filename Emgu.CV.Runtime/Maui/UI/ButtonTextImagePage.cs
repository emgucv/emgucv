//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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

        /// <summary>
        /// Set the image to be displayed
        /// </summary>
        /// <param name="image">The image to be displayed</param>
        public virtual void SetImage(IInputArray image)
        {
            //No need to dispatch here, ImageView will handle the proper dispatch
            this.DisplayImage.SetImage(image);
        }


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
        public ButtonTextImagePage(Microsoft.Maui.Controls.Button[] additionalButtons = null)
        {
            // ---------- Palette (matches the home screen) ----------
            var pageBackground = Microsoft.Maui.Graphics.Color.FromArgb("#EEF1F8");
            var cardBackground = Microsoft.Maui.Graphics.Colors.White;
            var primaryText = Microsoft.Maui.Graphics.Color.FromArgb("#1A1C2E");
            var secondaryText = Microsoft.Maui.Graphics.Color.FromArgb("#8A8FA3");
            var accent = Microsoft.Maui.Graphics.Color.FromArgb("#3D7BF7");
            var rowBorder = Microsoft.Maui.Graphics.Color.FromArgb("#ECEEF5");
            var imageBackground = Microsoft.Maui.Graphics.Color.FromArgb("#F2F2F7");
            const string bodyFont = "InterRegular";
            const string mediumFont = "InterSemiBold";

            // Accent "pill" button used for the primary action and any extras.
            Action<Microsoft.Maui.Controls.Button> stylePrimaryButton = (button) =>
            {
                button.BackgroundColor = accent;
                button.TextColor = Microsoft.Maui.Graphics.Colors.White;
                button.FontFamily = mediumFont;
                button.FontSize = 16;
                button.CornerRadius = 14;
                button.HeightRequest = 50;
                button.Padding = new Thickness(20, 0);
                button.HorizontalOptions = LayoutOptions.Fill;
            };

            TopButton.Text = "Run";
            TopButton.IsEnabled = true;
            stylePrimaryButton(TopButton);

            MessageLabel.Text = "";
            MessageLabel.FontFamily = bodyFont;
            MessageLabel.FontSize = 14;
            MessageLabel.TextColor = secondaryText;
            MessageLabel.HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Center;
            MessageLabel.HorizontalOptions = LayoutOptions.Center;

            _mainLayout.Spacing = 14;

            _mainLayout.Children.Add(Picker);
            Picker.IsVisible = false;
            Picker.FontFamily = bodyFont;
            Picker.TextColor = primaryText;
            Picker.TitleColor = secondaryText;

            _mainLayout.Children.Add(TopButton);
            if (additionalButtons != null)
            {
                foreach (Microsoft.Maui.Controls.Button button in additionalButtons)
                {
                    stylePrimaryButton(button);
                    _mainLayout.Children.Add(button);
                }
            }

            _additionalButtons = additionalButtons;

            _mainLayout.Children.Add(MessageLabel);

            DisplayImage.BackgroundColor = imageBackground;

            // Wrap the result image in a rounded white "card" to match the theme.
            var imageCard = new Border
            {
                BackgroundColor = cardBackground,
                Stroke = rowBorder,
                StrokeThickness = 1,
                Padding = new Thickness(6),
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = new CornerRadius(18) },
                Content = DisplayImage
            };
            _mainLayout.Children.Add(imageCard);

            LogEditor.Text = "";
            LogEditor.FontFamily = bodyFont;
            LogEditor.TextColor = secondaryText;
            LogEditor.BackgroundColor = cardBackground;
            LogEditor.HorizontalOptions = LayoutOptions.Fill;
            LogEditor.VerticalOptions = LayoutOptions.Center;
            LogEditor.FontSize = LogEditor.FontSize / 2;
            _mainLayout.Children.Add(LogEditor);

            SetLog(null);

            _mainLayout.Padding = new Thickness(16, 16, 16, 16);

            this.BackgroundColor = pageBackground;
            this.Content = new Microsoft.Maui.Controls.ScrollView()
            {
                Content = _mainLayout
            };
        }

        /// <summary>
        /// A flag that indicates if the file picker allow getting image from camera
        /// </summary>
        private bool? _hasCameraOption = null;

        /// <summary>
        /// Get the default camera option
        /// </summary>
        /// <returns>A flag that indicates if the file picker allow getting image from camera, will be used by HasCameraOption getter if no value is set.</returns>
        protected virtual bool GetDefaultCameraOption()
        {
            return false;
        }

        /// <summary>
        /// A flag that indicates if the file picker allow getting image from camera
        /// </summary>
        public bool HasCameraOption
        {
            get
            {
                if (_hasCameraOption == null)
                {
                    _hasCameraOption = GetDefaultCameraOption();
                }

                return _hasCameraOption.Value;
            }
            set
            {
                _hasCameraOption = value;
            }
        }




        /// <summary>
        /// Load the images and return them asynchronously
        /// </summary>
        /// <param name="imageNames">The name of the images</param>
        /// <param name="labels">The labels of the images</param>
        /// <returns>The images loaded</returns>
        public virtual async Task<Mat[]> LoadImages(String[] imageNames, String[] labels = null)
        {
            Mat[] mats = new Mat[imageNames.Length];

            SetMessage("Checking if camera option is available, please wait...");
            //Run it once in case it need to check if camera is available, which could take a long time to run
            await Task.Run(() => { bool cameraOption = this.HasCameraOption; });
            SetMessage(null);

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
                if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.Android)
                {
                    //Overwrite MediaPicker if there is no camera on this Android device.
                    if (!this.HasCameraOption)
                    {
                        captureSupported = false;
                    }
                }
            }

            for (int i = 0; i < mats.Length; i++)
            {
                String pickImgString = "Use Image from";
                if (labels != null && labels.Length > i)
                    pickImgString = labels[i];

                String action;
                List<String> options = new List<string>();
                options.Add("Default");

                options.Add("Photo Library");

                if (captureSupported)
                    options.Add("Photo from Camera");

                if (this.HasCameraOption)
                {
                    if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        if (captureSupported)
                        {
                            options.Add("Camera");
                            /*
#if __ANDROID__
                            if (this.CameraBackend == AndroidCameraBackend.AndroidCamera2)
                            {
                                foreach (String cameraId in AndroidCameraManager.GetAvailableCameraIds())
                                {
                                    options.Add(String.Format("Camera {0}", cameraId));
                                }
                            }
                            else
                            {
                                options.Add("Camera");
                            }
#else
                            options.Add("Camera");
#endif
                            */
                        }
                    } else if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.iOS)
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
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        Mat m = new Mat();
                        CvInvoke.Imdecode(ms.ToArray(), ImreadModes.ColorBgr, m);
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
                        FileResult fileResult = await FilePicker.PickAsync(PickOptions.Images);
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
                        try
                        {
                            var takePhotoResult = await MediaPicker.CapturePhotoAsync();

                            if (takePhotoResult == null) //canceled
                                return null;
                            using (Stream stream = await takePhotoResult.OpenReadAsync())
                                mats[i] = await ReadStream(stream);
                        }
                        catch (FeatureNotSupportedException)
                        {
                            //e.g. no camera is available on this device
                            SetMessage("Capturing photo from camera is not supported on this device.");
                            return null;
                        }
                        catch (Exception e)
                        {
                            //The exception would otherwise be lost in the async void
                            //button click handler, leaving the user with no feedback.
                            SetMessage(String.Format("Failed to capture photo: {0}", e.Message));
                            return null;
                        }
                    }
                }
                else if (action.Equals("Camera"))
                {
                    mats = new Mat[0];
#if __ANDROID__ 
                    String cameraIdCandidate = action.Replace("Camera ", "");
                    if (AndroidCameraManager.GetAvailableCameraIds().Contains(cameraIdCandidate))
                    {
                        _preferredCameraId = cameraIdCandidate;
                    }
                    else
                    {
                        _preferredCameraId = null;
                    }
#endif
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
                CvInvoke.Imdecode(data, ImreadModes.ColorBgr, m);
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

#if __IOS__
        /// <summary>
        /// Set the message to be displayed. Overrides the AvCaptureSessionPage no-op so
        /// capture session errors (e.g. "Capture device not found.") reach the message label.
        /// </summary>
        /// <param name="message">The message to be displayed</param>
        /// <param name="heightRequest">Not used by this page.</param>
        public override void SetMessage(String message, int heightRequest = 60)
        {
            SetMessage(message);
        }
#endif

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
                    //The label may have been hidden by pages created without a description
                    //text. Make sure a non-empty message is actually visible.
                    label.IsVisible = !String.IsNullOrEmpty(message);

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
