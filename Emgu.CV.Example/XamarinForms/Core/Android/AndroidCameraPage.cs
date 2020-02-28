using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
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

        public AndroidCameraPage()
            : base()
        {
            
            _imageView = new ImageView(Android.App.Application.Context);
            MainLayout.Children.Add(_imageView.ToView());
            
            _cameraManager = new AndroidCameraManager();
            _cameraManager.OnImageCaptured += (sender, mat) =>
            {
                //int width = mat.Width;
                SetImage(mat);
            };

            var button = this.GetButton();
            button.Text = "Open Camera";
            button.Clicked += (Object sender, EventArgs args) => { _cameraManager.CreateCaptureSession(); };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _cameraManager.StartBackgroundThread();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _cameraManager.CloseCamera();
            _cameraManager.StopBackgroundThread();

        }

        //private CameraCaptureSessionCallback sessionStateCallback = new CameraCaptureSessionCallback();

        public override void SetImage(IInputArray image)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                using (InputArray iaImage = image.GetInputArray())
                    using(Mat mat = iaImage.GetMat())
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

                _imageView.SetImageBitmap(_renderBuffer[_renderBufferIdx]);
            });
        }
    }
}