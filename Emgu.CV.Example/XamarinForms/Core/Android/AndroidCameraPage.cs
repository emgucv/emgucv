using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Media;
using Android.OS;
using Android.Renderscripts;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Paint = Android.Graphics.Paint;
using Java.Util.Concurrent;
using Xamarin.Forms.Platform.Android;
using Camera = Android.Hardware.Camera;
using Type = System.Type;


namespace Emgu.CV.XamarinForms
{
    public class AndroidCameraPage : ButtonTextImagePage
    {
        private AndroidCameraManager _cameraManager;
        private Bitmap[] _renderBuffer = new Bitmap[2];
        private int _renderBufferIdx = 0;

        private ImageView _imageView;

        private String _defaultButtonText = "Open Camera";
        private String _stopCameraText = "Stop Camera";

        public AndroidCameraPage()
            : base()
        {
            
            _imageView = new ImageView(Android.App.Application.Context);
            MainLayout.Children.Add(_imageView.ToView());
            
            var button = this.GetButton();
            button.Text = _defaultButtonText;
            button.Clicked += async (Object sender, EventArgs args) =>
            {
                if (button.Text.Equals(_defaultButtonText))
                {
                    button.Text = _stopCameraText;
                    StartCapture(delegate(Object sender, Mat mat) { SetImage(mat); });
                }
                else
                {
                    StopCapture();
                    button.Text = _defaultButtonText;
                }
            };
        }

        public void StartCapture(EventHandler<Mat> matHandler)
        {
            if (_cameraManager == null)
            {
                //prefer preview image that is slightly smaller than the screen resolution
                int preferredSize = (int) Math.Round(MainLayout.Width * MainLayout.Width) / 2;
                preferredSize = Math.Max(preferredSize, 480 * 600);
                _cameraManager = new AndroidCameraManager( preferredSize );
                _cameraManager.OnImageCaptured += matHandler;
                _cameraManager.StartBackgroundThread();
            }
            _cameraManager.CreateCaptureSession();
        }

        public void StopCapture()
        {
            if (_cameraManager != null)
            {
                _cameraManager.CloseCamera();
                _cameraManager.StopBackgroundThread();
                _cameraManager = null;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }

        protected override void OnDisappearing()
        {
            StopCapture();
            base.OnDisappearing();
        }

        //private CameraCaptureSessionCallback sessionStateCallback = new CameraCaptureSessionCallback();

        public override void SetImage(IInputArray image)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (Mat mat = iaImage.GetMat())
            {
                if (_renderBuffer[_renderBufferIdx] == null)
                {

                    _renderBuffer[_renderBufferIdx] = mat.ToBitmap();
                }
                else
                {
                    var size = iaImage.GetSize();
                    Bitmap buffer = _renderBuffer[_renderBufferIdx];
                    if (buffer.Width != size.Width || buffer.Height != size.Height)
                    {
                        buffer.Dispose();
                        _renderBuffer[_renderBufferIdx] = mat.ToBitmap();
                    }
                    else
                    {
                        mat.ToBitmap(buffer);
                    }
                }
            }

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                _imageView.SetImageBitmap(_renderBuffer[_renderBufferIdx]);
            });
        }
    }
}