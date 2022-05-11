using System;
using System.Collections.Generic;

using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Models;
using Xamarin.Forms;

#if !(__MACOS__ || __ANDROID__ || __IOS__ || NETFX_CORE)
using System.Drawing.Printing;
#endif

namespace Emgu.CV.XamarinForms
{
    public class CameraCalibrationPage : ProcessAndRenderPage
    {
        private CharucoCameraCalibrationModel _model;

        private Button _printCharucoBoardButton;
        private Button _useThisFrameButton;
        private Button _calibrateButton;

        public CameraCalibrationPage()
            : base(
                new CharucoCameraCalibrationModel(),
                "Perform Camera Calibration",
                null,
                null,
                new Xamarin.Forms.Button[] { new Xamarin.Forms.Button(), new Xamarin.Forms.Button(), new Xamarin.Forms.Button()}
            )
        {
            _model = (CharucoCameraCalibrationModel) base.Model;
            _printCharucoBoardButton = base.AdditionalButtons[0];
            _printCharucoBoardButton.Text = "Print Charuco Board";

            if (Device.RuntimePlatform == Device.WPF)
            {
                _printCharucoBoardButton.Clicked += PrintCharucoBoardButton_Clicked;
            }
            else
            {
                _printCharucoBoardButton.IsVisible = false;
            }

            _useThisFrameButton = base.AdditionalButtons[1];
            _calibrateButton = base.AdditionalButtons[2];

            _useThisFrameButton.Text = "Use this Frame";
            _calibrateButton.Text = "Calibrate";
            _calibrateButton.IsEnabled = false;

            _useThisFrameButton.Clicked += UseThisFrameButton_Clicked;
            _useThisFrameButton.IsEnabled = false;

            _model.FrameUsableChanged += ModelFrameUsableChanged;

            _model.OnFrameAdded += ModelFrameAdded;

            _calibrateButton.Clicked += CalibrateButtonOnClicked;
        }

        private async void PrintCharucoBoardButton_Clicked(object sender, EventArgs e)
        {
#if !(__MACOS__ || __ANDROID__ || __IOS__ || NETFX_CORE)
            using (Mat boardImage = new Mat())
            using (CharucoCameraCalibrationModel model = new CharucoCameraCalibrationModel())
            {
                await model.Init(null, null);
                model.GetCharucoBoard(boardImage);

                var image = boardImage.ToBitmapSource();

                var vis = new System.Windows.Media.DrawingVisual();
                var dc = vis.RenderOpen();
                dc.DrawImage(image, new System.Windows.Rect { Width = image.Width, Height = image.Height });
                dc.Close();

                var pdialog = new System.Windows.Controls.PrintDialog();
                if (pdialog.ShowDialog() == true)
                {
                    pdialog.PrintVisual(vis, "Charuco Board");
                }
            }
#endif
        }
        
        private void ModelFrameAdded(object sender, EventArgs e)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(
                () =>
                {
                    _calibrateButton.IsEnabled = true;
                }
            );
        }

        private void CalibrateButtonOnClicked(object sender, EventArgs e)
        {
            String message = _model.CalibrateCamera();
            SetLog(message);
        }

        private void ModelFrameUsableChanged(object sender, bool e)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(
                () =>
                {
                    _useThisFrameButton.IsEnabled = e;
                }
            );
            
        }

        private void UseThisFrameButton_Clicked(object sender, EventArgs e)
        {
            _model.UseOneFrame = true;
        }
    }
}
