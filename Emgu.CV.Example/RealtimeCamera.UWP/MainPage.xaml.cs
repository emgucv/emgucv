using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace RealtimeCamera
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            textBlock.Text = String.Empty;

            Window.Current.VisibilityChanged += (sender, args) =>
            {
                WinrtInvoke.WinrtOnVisibilityChanged(args.Visible);
            };

            //render the captured image in the bottom image view
            WinrtInvoke.WinrtSetFrameContainer(this.imageCamera);

            WinrtInvoke.WinrtStartMessageLoop(Process);

        }

        private Mat _cameraMatrix;
        private Mat _distCoeffs;
        private Matrix<float> mapx, mapy;
        private Mat _capturedMat = new Mat(new System.Drawing.Size(640, 480), DepthType.Cv8U, 3);
        private Mat _processed = new Mat();
        private VideoCapture _capture;

        public void Process()
        {
            while (true)
            {
                if (_capture != null)
                {
                    bool grabbed = _capture.Grab();

                    //Read the camera data to the mat
                    //Note that m is in 3 channel RGB color space, 
                    //Open CV's default color space is BGR for 3 channel Mat
                    bool retrieved = _capture.Retrieve(_capturedMat);

                    if (grabbed && retrieved)
                        try
                        {
                            if (!_capturedMat.IsEmpty)
                            {
                                //The data in the mat that is read from the camera will 
                                //be drawn to the Image control
                                WinrtInvoke.WinrtImshow();

                                //Extra processing
                                //Try to distort the image and render the processed result ....
                                if (_cameraMatrix == null || _distCoeffs == null)
                                {
                                    //Create a dummy camera calibration matrix for testing
                                    //Use your own if you have calibrated your camera
                                    _cameraMatrix = new Mat(new System.Drawing.Size(3, 3), DepthType.Cv32F, 1);

                                    int centerY = _capturedMat.Width >> 1;
                                    int centerX = _capturedMat.Height >> 1;
                                    //CvInvoke.SetIdentity(_cameraMatrix, new MCvScalar(1.0));
                                    _cameraMatrix.SetTo(new float[]
                                    {
                                    1f, 0f, (float)centerY,
                                    0f, 1f, (float)centerX,
                                    0f, 0f, 1f
                                    });

                                    _distCoeffs = new Mat(new System.Drawing.Size(5, 1), DepthType.Cv32F, 1);
                                    _distCoeffs.SetTo(new float[] { -0.000003f, 0f, 0f, 0f, 0f });
                                    mapx = new Matrix<float>(_capturedMat.Height, _capturedMat.Width);
                                    mapy = new Matrix<float>(_capturedMat.Height, _capturedMat.Width);
                                    CvInvoke.InitUndistortRectifyMap(
                                        _cameraMatrix,
                                        _distCoeffs,
                                        null,
                                        _cameraMatrix,
                                        _capturedMat.Size,
                                        DepthType.Cv32F,
                                        mapx,
                                        mapy);

                                }

                                _capturedMat.CopyTo(_processed);
                                CvInvoke.Undistort(_capturedMat, _processed, _cameraMatrix, _distCoeffs);

                                //mProcess is in the same color space as m, which is RGB, 
                                //needed to change to BGR
                                CvInvoke.CvtColor(_processed, _processed, ColorConversion.Rgb2Bgr);


                                //Can apply simple image processing to the captured image, let just invert the pixels
                                //CvInvoke.BitwiseNot(m, m);

                                //render the processed image on the top image view
                                CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                                    async () =>
                                    {
                                        var wb = _processed.ToWritableBitmap();
                                        imageProcessed.Source = wb;
                                    });
                            }
                        }
                        catch (Exception e)
                        {
                            SetText(e.Message);
                        }
                }
            }
        }

        private void SetText(String msg)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                async () =>
                {
                    textBlock.Text = msg;
                });
        }

        private void captureButton_Click(object sender, RoutedEventArgs e)
        {

            if (_capture == null)
            {
                _capture = new VideoCapture();
                if (!_capture.IsOpened)
                {
                    //Release the capture that failed to open
                    _capture.Dispose();
                    _capture = null;
                }
                else
                    captureButton.Content = "Stop";
            }
            else
            {
                _capture.Dispose();
                _capture = null;
                captureButton.Content = "Start Capture";
            }

        }
    }
}
